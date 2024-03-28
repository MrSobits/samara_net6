namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;
    using Gkh.Utils;

    public class MassBuildContractViewModel : BaseViewModel<MassBuildContract>
    {
        public override IDataResult List(IDomainService<MassBuildContract> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var programCrId = baseParams.Params.GetAsId("programCrId");          

            var data = domainService.GetAll()         
                .WhereIf(programCrId > 0, x => x.ProgramCr.Id == programCrId)
                .Select(x => new
                {
                    x.Id,
                    InspectorName = x.Inspector.Fio,
                    BuilderName = x.Builder.Contragent.Name,
                    BuilderInn = x.Builder.Contragent.Inn,
                    x.TypeContractBuild,
                    x.DateStartWork,
                    x.DateEndWork,
                    ProgramCr = x.ProgramCr.Name,
                    x.DateInGjiRegister,
                    x.DocumentDateFrom,
                    x.ProtocolDateFrom,
                    x.DateCancelReg,
                    x.DateAcceptOnReg,
                    x.DocumentName,
                    x.ProtocolName,
                    x.DocumentNum,
                    x.ProtocolNum,
                    x.Description,
                    x.BudgetMo,
                    x.BudgetSubject,
                    x.OwnerMeans,
                    x.FundMeans,
                    x.Sum,
                    x.State,
                    x.Contragent,
                    x.TerminationDate,
                    x.TerminationDocumentFile,
                    x.TerminationReason,
                    x.GuaranteePeriod,
                    x.UrlResultTrading,
                    Text = $"№ {x.DocumentNum} от {(x.DocumentDateFrom.HasValue ? x.DocumentDateFrom.Value.ToShortDateString() : "")}"
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<MassBuildContract> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return value != null
                ? new BaseDataResult(new
                {
                    value.Id,
                    value.ProgramCr,
                    value.Inspector,
                    Builder =
                        value.Builder != null
                            ? new { value.Builder.Id, ContragentName = value.Builder.Contragent.Name }
                            : null,
                    value.TypeContractBuild,
                    value.DateStartWork,
                    value.DateEndWork,
                    value.DateInGjiRegister,
                    value.DocumentDateFrom,
                    value.DateCancelReg,
                    value.DateAcceptOnReg,
                    value.DocumentName,
                    value.DocumentNum,
                    value.Description,
                    value.BudgetMo,
                    value.BudgetSubject,
                    value.OwnerMeans,
                    value.FundMeans,
                    value.Sum,
                    value.StartSum,
                    value.DocumentFile,
                    value.ProtocolName,
                    value.ProtocolNum,
                    value.ProtocolDateFrom,
                    value.ProtocolFile,
                    value.State,
                    value.UsedInExport,
                    value.Contragent,
                    value.TerminationDate,
                    value.TerminationDocumentFile,
                    value.TerminationReason,
                    value.GuaranteePeriod,
                    value.UrlResultTrading,
                    Text = $"№ {value.DocumentNum} от {value.DocumentDateFrom.ToDateString()}",
                    value.TerminationDocumentNumber,
                    value.TerminationDictReason
                })
                : new BaseDataResult();
        }
    }
}