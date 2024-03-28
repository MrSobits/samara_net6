namespace Bars.GkhCr.ViewModel.SpecialObjectCr
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class SpecialObjectCrViewModel : BaseViewModel<SpecialObjectCr>
    {
        public override IDataResult List(IDomainService<SpecialObjectCr> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var programId = baseParams.Params.GetAs("programId", string.Empty);
            var municipalityId = baseParams.Params.GetAs("municipalityId", string.Empty);
            var deleted = baseParams.Params.GetAs("deleted", false);
            var ids = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var programIds = !string.IsNullOrEmpty(programId) ? programId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var municipalityIds = !string.IsNullOrEmpty(municipalityId) ? municipalityId.Split(',').Select(x => x.ToLong()).ToArray() : new long[0];
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

            var stateId = baseParams.Params.GetAsId("stateId");

            var data = this.Container.Resolve<ISpecialObjectCrService>().GetFilteredByOperator()
                .WhereIf(stateId > 0, x => x.State.Id == stateId)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectId == realityObjectId)
                .WhereIf(deleted, x => !x.ProgramCrId.HasValue && x.BeforeDeleteProgramCrId.HasValue)
                .WhereIf(!deleted, x => x.ProgramCrId.HasValue && !x.BeforeDeleteProgramCrId.HasValue)
                .WhereIf(programIds.Length > 0 && deleted, x => programIds.Contains(x.BeforeDeleteProgramCrId.Value))
                .WhereIf(programIds.Length > 0 && !deleted, x => programIds.Contains(x.ProgramCrId.Value))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.MunicipalityId) || municipalityIds.Contains(x.SettlementId))
                .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                .Select(x => new
                    {
                        x.Id,
                        x.ProgramNum,
                        x.DateAcceptCrGji,
                        RealityObjName = x.Address,
                        x.Municipality,
                        x.Settlement,
                        x.State,
                        ProgramCrName = deleted ? x.BeforeDeleteProgramCrName : x.ProgramCrName,
                        AllowReneg = x.DateAcceptCrGji,
                        MonitoringSmrState = x.SmrState,
                        x.MonitoringSmrId,
                        RealityObject = x.RealityObjectId,
                        x.PeriodName,
                        x.MethodFormFund
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.RealityObjName)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = loadParams.Order.Length == 0 ? data.Paging(loadParams) : data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}