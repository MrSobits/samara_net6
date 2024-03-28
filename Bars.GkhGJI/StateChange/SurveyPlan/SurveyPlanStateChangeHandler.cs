namespace Bars.GkhGji.StateChange.SurveyPlan
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.DomainService.SurveyPlan;
    using Bars.GkhGji.Entities.SurveyPlan;

    using Castle.Windsor;

    public class SurveyPlanStateChangeHandler : IStateChangeHandler
    {
        public IWindsorContainer Container { get; set; }

        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            if (entity.State.TypeId != "gji_survey_plan" || !newState.FinalState)
            {
                return;
            }

            var plan = entity as SurveyPlan;
            if (plan == null)
            {
                return;
            }

            var service = Container.Resolve<ISurveyPlanService>();
            try
            {
                service.CreateSurveys(plan);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}