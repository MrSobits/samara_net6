namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;
    using Gkh.Utils;

    public class SpecialBuildContractViewModel : BaseViewModel<SpecialBuildContract>
    {
        public override IDataResult List(IDomainService<SpecialBuildContract> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var programCrId = baseParams.Params.GetAsId("programCrId");
            var twId = baseParams.Params.GetAsId("twId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAs("objectCrId", 0L);
            }

            if (objectCrId <= 0 && programCrId <= 0 && twId <= 0)
            {
                return new BaseDataResult(false, "Необходимо указать либо Id объекта КР, либо Id программы КР");
            }

            return domainService.GetAll()
                .WhereIf(objectCrId > 0, x => x.ObjectCr.Id == objectCrId)
                .WhereIf(programCrId > 0, x => x.ObjectCr.ProgramCr.Id == programCrId)
                .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                .Select(x => new
                {
                    x.Id,
                    InspectorName = x.Inspector.Fio,
                    BuilderName = x.Builder.Contragent.Name,
                    BuilderInn = x.Builder.Contragent.Inn,
                    RoMunicipality = x.ObjectCr.RealityObject.Municipality.Name,
                    RoSettlement = x.ObjectCr.RealityObject.MoSettlement.Name,
                    RoAddress = x.ObjectCr.RealityObject.Address,
                    x.TypeContractBuild,
                    x.DateStartWork,
                    x.DateEndWork,
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
                    x.IsLawProvided,
                    x.WebSite,
                    x.BuildContractState,
                    Text = $"№ {x.DocumentNum} от {(x.DocumentDateFrom.HasValue ? x.DocumentDateFrom.Value.ToShortDateString() : "")}"
                })
                .ToListDataResult(loadParams);
        }

        public override IDataResult Get(IDomainService<SpecialBuildContract> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return value != null
                ? new BaseDataResult(new
                {
                    value.Id,
                    value.ObjectCr,
                    value.Inspector,
                    TypeWork = value.TypeWork?.Id ?? 0,
                    Builder =
                        value.Builder != null
                            ? new {value.Builder.Id, ContragentName = value.Builder.Contragent.Name}
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
                    value.TerminationDictReason,
                    value.IsLawProvided,
                    value.WebSite,
                    value.BuildContractState
                })
                : new BaseDataResult();
        }
    }
}