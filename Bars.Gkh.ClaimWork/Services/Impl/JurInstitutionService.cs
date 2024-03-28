namespace Bars.Gkh.ClaimWork.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Domain;
    using Enums;
    using Gkh.Entities;
    using Modules.ClaimWork.Entities;

    public class JurInstitutionService : IJurInstitutionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealObjs(BaseParams baseParams)
        {
            var jurInstitutionDomain = Container.ResolveDomain<JurInstitution>();
            var jurInstitutionRealObjDomain = Container.ResolveDomain<JurInstitutionRealObj>();
            var realObjDomain = Container.ResolveDomain<RealityObject>();

            try
            {
                var jurInstId = baseParams.Params.GetAsId("jurInstId");
                var realObjIds = baseParams.Params.GetAs("realObjIds", new long[0]);

                var existRealObjIds =
                    jurInstitutionRealObjDomain.GetAll()
                        .Where(x => x.JurInstitution.Id == jurInstId)
                        .Select(x => x.RealityObject.Id)
                        .ToList();

                var jurInstitution = jurInstitutionDomain.Load(jurInstId);

                var listToSave = new List<JurInstitutionRealObj>();

                foreach (var id in realObjIds)
                {
                    if (!existRealObjIds.Contains(id))
                    {
                        var newJurInstRealObj = new JurInstitutionRealObj
                        {
                            RealityObject = realObjDomain.Load(id),
                            JurInstitution = jurInstitution,
                        };

                        listToSave.Add(newJurInstRealObj);
                    }
                }

                TransactionHelper.InsertInManyTransactions(Container, listToSave, 10000, true, true);

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(jurInstitutionDomain);
                Container.Release(jurInstitutionRealObjDomain);
                Container.Release(realObjDomain);
            }
        }

        public IDataResult ListRealObj(StoreLoadParams storeParams)
        {
            var from = storeParams.Params.GetAs<int>("from");
            var to = storeParams.Params.GetAs<int>("to");
            var isEven = storeParams.Params.GetAs<bool>("isEven");
            var isOdd = storeParams.Params.GetAs<bool>("isOdd");

            var roDomain = Container.ResolveDomain<RealityObject>();

            try
            {
                var filterForAddress = string.Empty;
                if (storeParams.ComplexFilter != null && storeParams.ComplexFilter.Rule != null
                    && storeParams.ComplexFilter.Rule.Field == "Address")
                {
                    filterForAddress = storeParams.ComplexFilter.Rule.Value;
                    storeParams.ComplexFilter = null;
                }

                var data = roDomain.GetAll()
                    .WhereIf(filterForAddress != string.Empty,
                        x =>
                            x.Address != null &&
                            (x.Address.ToUpper().Contains(filterForAddress.ToUpper()) ||
                             x.Address.Replace(" ", "").ToUpper().Contains(filterForAddress.Replace(" ", "").ToUpper())))
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        Settlement = x.MoSettlement.Name,
                        x.Address,
                        x.FiasAddress.PlaceName,
                        x.FiasAddress.StreetName,
                        x.FiasAddress.House,
                        x.FiasAddress.Letter,
                        x.FiasAddress.Housing,
                        x.FiasAddress.Building
                    })
                    .Filter(storeParams, Container)
                    .OrderIf(storeParams.Order.Fields.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(storeParams.Order.Fields.Length == 0, true, x => x.Address)
                    .ToList()
                    .Where(x =>
                    {
                        var houseNum = Regex.Match(x.House, @"^\d+").Value.ToInt();

                        if (!isOdd && houseNum % 2 == 1)
                        {
                            return false;
                        }

                        if (!isEven && houseNum % 2 == 0)
                        {
                            return false;
                        }

                        return (from == 0 || from <= houseNum) && (to == 0 || houseNum <= to);
                    })
                    .AsQueryable();

                return new ListDataResult(data.Order(storeParams).Paging(storeParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(roDomain);
            }
        }

        public IDataResult ListRealObjByMunicipality(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var moId = baseParams.Params.GetAs<long>("moId");
            var jurInstitutinRoDomain = this.Container.ResolveDomain<JurInstitutionRealObj>();

            try
            {
                return jurInstitutinRoDomain.GetAll()
                    .WhereIf(moId > 0, x => x.JurInstitution.Municipality.Id == moId)
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.Address,
                            Municipality = x.RealityObject.Municipality.Name
                        })
                        .ToListDataResult(loadParam, this.Container);
            }
            finally
            {
                this.Container.Release(jurInstitutinRoDomain);
            }
        }
    }
}