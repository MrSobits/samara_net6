namespace Bars.GkhRf.Export
{
    using System.Collections;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    public class ContractRfObjectDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var typeCondition = baseParams.Params.ContainsKey("typeCondition")
                                ? baseParams.Params["typeCondition"].ToStr() == "In" && !string.IsNullOrEmpty(baseParams.Params["typeCondition"].ToStr()) ? 10 : 20
                                : 0;
            var contractRfId = baseParams.Params.GetAs<long>("contractRfId", 0);

            if (contractRfId == 0)
            {
                return null;
            }

            var contractRf = Container.Resolve<IDomainService<ContractRf>>().Get(contractRfId);

            var domain = Container.Resolve<IDomainService<ContractRfObject>>();

            var dictManOrg = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(y => domain.GetAll().Any(x => x.RealityObject.Id == y.RealityObject.Id && x.ContractRf.Id == contractRfId))
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Where(x => (
                    (contractRf.DateEnd != null && x.ManOrgContract.StartDate >= contractRf.DateBegin && contractRf.DateEnd >= x.ManOrgContract.StartDate)
                    ||
                    (x.ManOrgContract.EndDate != null && contractRf.DateBegin >= x.ManOrgContract.StartDate && x.ManOrgContract.EndDate >= contractRf.DateBegin)
                    ||
                    (x.ManOrgContract.EndDate == null && contractRf.DateEnd != null && x.ManOrgContract.StartDate <= contractRf.DateEnd)
                    ||
                    (x.ManOrgContract.EndDate != null && contractRf.DateEnd == null && contractRf.DateBegin <= x.ManOrgContract.EndDate)
                    || (x.ManOrgContract.EndDate == null && contractRf.DateEnd == null)))

                .Select(x => new
                    {
                        x.RealityObject.Id,
                        ManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Aggregate("", (x, rec) => x + (!string.IsNullOrEmpty(x) ? ", " + rec.ManOrg : rec.ManOrg)));

            return domain.GetAll()
                .Where(x => x.TypeCondition == typeCondition.To<TypeCondition>() && x.ContractRf.Id == contractRfId)
                .Select(x => new
                    {
                        x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        RealityObjectName = x.RealityObject.Address,
                        MunicipalityName = x.RealityObject.Municipality.Name,
                        x.IncludeDate,
                        x.ExcludeDate,
                        x.TypeCondition,
                        x.RealityObject.GkhCode
                    })
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id,
                        x.RealityObjectId,
                        x.RealityObjectName,
                        x.MunicipalityName,
                        ManOrgName = dictManOrg.ContainsKey(x.RealityObjectId) ? dictManOrg[x.RealityObjectId] : string.Empty,
                        x.IncludeDate,
                        x.ExcludeDate,
                        x.TypeCondition,
                        x.GkhCode
                    })
                .AsQueryable()
                .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjectName)
                .Filter(loadParams, Container)
                .Order(loadParams)
                .ToList();
        }
    }
}