namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;

    public class InstrExamActionNormativeDocViewModel : BaseViewModel<InstrExamActionNormativeDoc>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<InstrExamActionNormativeDoc> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var actCheckActionId = baseParams.Params.GetAsId("actCheckActionId");

            return domainService
                .GetAll()
                .WhereIf(actCheckActionId > 0, x => x.InstrExamAction.Id == actCheckActionId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}