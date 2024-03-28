namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionCarriedOutEventViewModel : BaseViewModel<ActCheckActionCarriedOutEvent>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ActCheckActionCarriedOutEvent> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var actCheckActionId = baseParams.Params.GetAsId("actCheckActionId");

            return domainService
                .GetAll()
                .WhereIf(actCheckActionId > 0, x => x.ActCheckAction.Id == actCheckActionId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}