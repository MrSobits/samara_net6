namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.RapidResponseSystem
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// Interceptor для <see cref="RapidResponseSystemAppealDetails"/>
    /// </summary>
    public class RapidResponseSystemAppealDetailsInterceptor : EmptyDomainInterceptor<RapidResponseSystemAppealDetails>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<RapidResponseSystemAppealDetails> service, RapidResponseSystemAppealDetails entity)
        {
            var rapidResponseSystemAppealService = this.Container.Resolve<IRapidResponseSystemAppealService>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateDomain = this.Container.ResolveDomain<State>();
            var dayDomain = this.Container.ResolveDomain<Day>();
            var config = this.Container.GetGkhConfig<AppealConfig>();

            using (this.Container.Using(stateDomain, dayDomain, rapidResponseSystemAppealService))
            {
                var currentDate = DateTime.Today;

                var controlPeriodMaxWorkDay = rapidResponseSystemAppealService
                    .GetControlPeriodMaxDay(config.RapidResponseSystemConfig.ControlPeriodParameter, currentDate);
                        
                var typeId = stateProvider.GetStatefulEntityInfo(typeof(RapidResponseSystemAppealDetails)).TypeId;
                var state = stateDomain.GetAll().First(x => x.TypeId == typeId && x.StartState);

                entity.ReceiptDate = currentDate;
                entity.ControlPeriod = controlPeriodMaxWorkDay;
                entity.State = state;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<RapidResponseSystemAppealDetails> service, RapidResponseSystemAppealDetails entity)
        {
            var rapidResponseSystemAppealResponseDomain = this.Container.ResolveDomain<RapidResponseSystemAppealResponse>();

            using (this.Container.Using(rapidResponseSystemAppealResponseDomain))
            {
                rapidResponseSystemAppealResponseDomain.GetAll()
                    .Where(x => x.RapidResponseSystemAppealDetails.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => rapidResponseSystemAppealResponseDomain.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}