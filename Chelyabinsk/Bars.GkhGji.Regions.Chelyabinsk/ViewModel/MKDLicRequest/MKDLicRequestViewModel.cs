namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel.MKDLicRequest
{
    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.smevHistoryServiceV2;
    using System;
    using System.Linq;

    public class MKDLicRequestViewModel : BaseViewModel<MKDLicRequest>
    {

        public IDomainService<MKDLicRequestRealityObject> RODomain { get; set; }
        public IDomainService<MKDLicRequestExecutant> ExecutantDomain { get; set; }
        public IDomainService<MKDLicRequestHeadInspector> InspectorDomain { get; set; }
        public override IDataResult List(IDomainService<MKDLicRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", false);
            var dateStart = baseParams.Params.GetAs("dateFromStart", new DateTime());
            var dateEnd = baseParams.Params.GetAs("dateFromEnd", new DateTime());
            var checkDateStart = baseParams.Params.GetAs("checkTimeStart", new DateTime());
            var checkDateEnd = baseParams.Params.GetAs("checkTimeEnd", new DateTime());
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId", 0);

            var reqByRo = RODomain.GetAll()
                .Where(x => x.RealityObject.Id == realityObjectId).Select(x => x.MKDLicRequest.Id).Distinct().ToList();

            var roDict = RODomain.GetAll()
                  .Select(x => new
                  {
                      x.MKDLicRequest.Id,
                      RealityObject = x.RealityObject.Address
                  })
                  .AsEnumerable()
                  .GroupBy(x => x.Id)
                  .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.RealityObject, ", "));

            var execDict = ExecutantDomain.GetAll()
               .Select(x => new
               {
                   x.MKDLicRequest.Id,
                   Inspector = x.Executant.Fio
               })
               .AsEnumerable()
               .GroupBy(x => x.Id)
               .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.Inspector, ", "));

            var inspDict = InspectorDomain.GetAll()
               .Select(x => new
               {
                   x.MKDLicRequest.Id,
                   Inspector = x.Inspector.Fio
               })
               .AsEnumerable()
               .GroupBy(x => x.Id)
               .ToDictionary(x => x.Key, y => y.Distinct().AggregateWithSeparator(x => x.Inspector, ", "));

            var data = domainService.GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.StatementDate > dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.StatementDate < dateEnd)
                .WhereIf(checkDateStart != DateTime.MinValue, x => x.CheckTime > checkDateStart)
                .WhereIf(checkDateEnd != DateTime.MinValue, x => x.CheckTime < checkDateEnd)
                .WhereIf(realityObjectId > 0, x => reqByRo.Contains(x.Id))
                .WhereIf(!showCloseAppeals, x => !x.State.FinalState)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.LicStatementResult,
                    RealityObjects = roDict.ContainsKey(x.Id) ? roDict[x.Id] : "",
                    Contragent = x.Contragent != null ? x.Contragent.Name : "",
                    x.StatementDate,
                    x.StatementNumber,
                    Executant = execDict.ContainsKey(x.Id) ? execDict[x.Id] : "",
                    Inspector = inspDict.ContainsKey(x.Id) ? inspDict[x.Id] : "",
                    ExecutantDocGji = x.ExecutantDocGji.Name,
                    MKDLicTypeRequest = x.MKDLicTypeRequest.Name,
                    x.CheckTime,
                    x.ExtensTime
                }).AsQueryable()
                .Select(x => new
                {
                    x.Id,
                    x.Inspector,
                    x.Executant,
                    x.State,
                    x.LicStatementResult,
                    x.RealityObjects,
                    x.StatementDate,
                    x.StatementNumber,
                    x.Contragent,
                    x.ExecutantDocGji,
                    x.MKDLicTypeRequest,
                    x.CheckTime,
                    x.ExtensTime
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<MKDLicRequest> domainService, BaseParams baseParams)
        {
            var licReqDomain = Container.ResolveDomain<MKDLicRequest>();

            var loadParams = baseParams.GetLoadParam();
            var id = baseParams.Params.GetAs<long>("id");

            var data = licReqDomain.GetAll()
            .Where(x => x.Id == id)
            .AsEnumerable()
            .Select(x => new
            {
                x.Id,
                x.State,
                x.ConclusionDate,
                x.ConclusionNumber,
                x.LicStatementResult,
                x.Contragent,
                x.StatementDate,
                x.StatementNumber,
                x.Inspector,
                x.ExecutantDocGji,
                x.MKDLicTypeRequest,
                x.Objection,
                x.ObjectionResult,
                x.RequestFile,

                x.NumberGji,
                x.CheckTime,
                x.QuestionsCount,
                x.Executant,
                x.SuretyResolve,
                x.PreviousRequest,
                x.RedtapeFlag,
                x.Surety,
                x.ApprovalContragent,
                x.QuestionStatus,
                x.SSTUExportState,
                x.ZonalInspection,
                ContragentAddress = x.Contragent != null ? x.Contragent.JuridicalAddress : "",
                x.KindStatement,
                x.ExtensTime,
                x.Description,
                Email = x.Contragent != null ? x.Contragent.Email : "",
                Phone = x.Contragent != null ? x.Contragent.Phone : "",
                x.RequestStatus,
                x.AmountPages,
                x.PlannedExecDate,
                x.ExecutantTakeDate,
                x.Comment,
                x.Year,
                x.ControlDateGisGkh,
                x.ConclusionFile,
                x.ChangeDate
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new BaseDataResult(data);
        }
    }
}
