using Bars.B4;
using Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction;
using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    public class PreventiveActionTaskViewModel : BaseViewModel<PreventiveActionTask>
    {
        private readonly IPreventiveActionTaskService service;

        public PreventiveActionTaskViewModel(IPreventiveActionTaskService service)
        {
            this.service = service;
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<PreventiveActionTask> domainService, BaseParams baseParams)
            => service.List(baseParams);
    }
}