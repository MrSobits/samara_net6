namespace Bars.GkhGji.ViewModel
{
    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MKDLicRequestViewModel : BaseViewModel<MKDLicRequest>
    {

        public IDomainService<MKDLicRequestRealityObject> RODomain { get; set; }
        public IDomainService<MKDLicRequestInspector> InspectorDomain { get; set; }
        public override IDataResult List(IDomainService<MKDLicRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId", 0);

            if(dateStart2 == DateTime.MinValue && dateEnd2 == DateTime.MinValue)
            {
                dateStart2 = DateTime.Now.AddMonths(-6);
                dateEnd2 = DateTime.Now;
            }

            var reqByRo = RODomain.GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId).Select(x => x.MKDLicRequest.Id).Distinct().ToList();

            var roDict = RODomain.GetAll()
                  .Where(x => x.MKDLicRequest.StatementDate >= dateStart2 && x.MKDLicRequest.StatementDate <= dateEnd2)
                  .Select(x => new
                  {
                      x.MKDLicRequest.Id,
                      RealityObject = x.RealityObject.Address
                  }).AsEnumerable()
                    .GroupBy(x => x.Id)
                 .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.RealityObject, ", "));

            var inspDict = InspectorDomain.GetAll()
               .Where(x => x.MKDLicRequest.StatementDate >= dateStart2 && x.MKDLicRequest.StatementDate <= dateEnd2)
               .Select(x => new
               {
                   x.MKDLicRequest.Id,
                   Inspector = x.Inspector.Fio
               }).AsEnumerable()
                 .GroupBy(x => x.Id)
              .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.Inspector, ", "));


            var data = domainService.GetAll()
                .Where(x => x.StatementDate >= dateStart2 && x.StatementDate <= dateEnd2)
                .WhereIf(realityObjectId > 0, x => reqByRo.Contains(x.Id))
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ConclusionDate,
                    x.ConclusionNumber,
                    x.LicStatementResult,
                    RealityObjects = roDict.ContainsKey(x.Id) ? roDict[x.Id] : "",
                    Contragent = x.Contragent != null ? x.Contragent.Name : "",
                    x.PhysicalPerson,
                    x.StatementDate,
                    x.StatementNumber,
                    Inspector = inspDict.ContainsKey(x.Id) ? inspDict[x.Id] : "",
                    ExecutantDocGji = x.ExecutantDocGji.Name,
                    MKDLicTypeRequest = x.MKDLicTypeRequest.Name,
                    x.Objection,
                    x.ObjectionResult
                }).AsQueryable()
                .Select(x=> new
                {
                    x.Id,
                    x.Inspector,
                    x.State,
                    x.ConclusionDate,
                    x.ConclusionNumber,
                    x.LicStatementResult,
                    x.RealityObjects,
                    x.PhysicalPerson,
                    x.StatementDate,
                    x.StatementNumber,
                    x.Contragent,
                    x.Objection,
                    x.ObjectionResult,
                    x.ExecutantDocGji,
                    x.MKDLicTypeRequest
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


    }
}
