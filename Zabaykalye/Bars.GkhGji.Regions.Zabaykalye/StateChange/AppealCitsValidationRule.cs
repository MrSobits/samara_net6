namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    using System.Linq;
    using B4;
    using B4.Modules.States;

    using Bars.GkhGji.Entities;

    public class AppealCitsValidationRule : Bars.GkhGji.StateChange.AppealCitsValidationRule
    {
        public override ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is AppealCits)
            {
                var appCitsRoDomain = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
                var appCitsAnswerDomain = Container.Resolve<IDomainService<AppealCitsAnswer>>();

                var appealCits = statefulEntity as AppealCits;
                
                try
                {
                    if (!appCitsRoDomain.GetAll().Any(x => x.AppealCits.Id == appealCits.Id))
                    {
                        return ValidateResult.No("Обязательно наличие хотя бы одного дома во вкладке \"Место возникновения проблемы\"");
                    }

                    if (newState.FinalState)
                    {
                        if (!appCitsAnswerDomain.GetAll().Any(x => x.AppealCits.Id == appealCits.Id))
                        {
                            return ValidateResult.No("Необходимо заполнить вкладку \"Ответы\"");
                        }
                    }
                }
                finally
                {
                    Container.Release(appCitsRoDomain);
                    Container.Release(appCitsAnswerDomain);
                }
            }

            return ValidateResult.Yes();
        }
    }
}