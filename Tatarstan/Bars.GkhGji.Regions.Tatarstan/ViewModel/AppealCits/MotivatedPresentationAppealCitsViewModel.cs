namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.AppealCits
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;

    /// <summary>
    /// View-модель для <see cref="MotivatedPresentationAppealCits"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsViewModel : BaseViewModel<MotivatedPresentationAppealCits>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<MotivatedPresentationAppealCits> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IMotivatedPresentationAppealCitsService>();
            using (this.Container.Using(service))
            {
                return service.Get(baseParams);
            }
        }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<MotivatedPresentationAppealCits> domainService, BaseParams baseParams)
        {
            var service = this.Container.Resolve<IMotivatedPresentationAppealCitsService>();
            using (this.Container.Using(service))
            {
                return service.List(baseParams);
            }
        }
    }
}