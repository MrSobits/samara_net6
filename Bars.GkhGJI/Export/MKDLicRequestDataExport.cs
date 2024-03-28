namespace Bars.GkhGji.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;

    public class MKDLicRequestExport : BaseDataExportService
    {
        public IDomainService<MKDLicRequest> MKDLicRequestDomain { get; set; }

        public IDomainService<MKDLicRequestRealityObject> MKDLicRequestRealityObjectDomain { get; set; }

        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectDomain { get; set; }
        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId", 0);

            var reqByRo = MKDLicRequestRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId).Select(x => x.MKDLicRequest.Id).Distinct().ToList();

            var roDict = MKDLicRequestRealityObjectDomain.GetAll()
                   .Where(x => x.MKDLicRequest.StatementDate >= dateStart2 && x.MKDLicRequest.StatementDate <= dateEnd2)
                   .Select(x => new
                   {
                       Id = x.MKDLicRequest.Id,
                       RealityObject = x.RealityObject.Address
                   }).AsEnumerable().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Select(x => x.RealityObject).AggregateWithSeparator("; "));

            var data = MKDLicRequestDomain.GetAll()
                .Where(x => x.StatementDate >= dateStart2 && x.StatementDate <= dateEnd2)
                .WhereIf(realityObjectId > 0, x => reqByRo.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ConclusionDate,
                    x.ConclusionNumber,
                    x.LicStatementResult,
                    RealityObjects = roDict.ContainsKey(x.Id)? roDict[x.Id]:"",
                    Contragent = x.Contragent != null ? x.Contragent.Name : "",
                    x.PhysicalPerson,
                    x.StatementDate,
                    x.StatementNumber,
                    Inspector = x.Inspector.Fio,
                    ExecutantDocGji = x.ExecutantDocGji.Name,
                    MKDLicTypeRequest = x.MKDLicTypeRequest.Name,
                    x.Objection,
                    x.ObjectionResult
                })
                   .Filter(loadParams, Container)
                    .Order(loadParams)
                    .ToList();

            return data;
        }
    }
}