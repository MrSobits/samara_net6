namespace Bars.GkhGji.Interceptors.SurveyPlan
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities.SurveyPlan;

    public class SurveyPlanInterceptor : EmptyDomainInterceptor<SurveyPlan>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SurveyPlan> service, SurveyPlan entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            try
            {
                stateProvider.SetDefaultState(entity);
                if (entity.State == null)
                {
                    throw new ValidationException("Для плана проверки не создан начальный статус");
                }
            }
            finally
            {
                Container.Release(stateProvider);
            }
            return base.BeforeCreateAction(service, entity);
        }
    }
}