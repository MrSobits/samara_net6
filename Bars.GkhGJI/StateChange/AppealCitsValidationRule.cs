namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Linq;
    using B4;
    using B4.Modules.States;

    using Bars.GkhGji.Contracts.Reminder;

    using Castle.Windsor;
    using Entities;

    public class AppealCitsValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_appeal_citizens_validation_rule"; }
        }

        public string Name
        {
            get { return "Проверка заполненности карточки обращения граждан"; }
        }

        public string TypeId
        {
            get { return "gji_appeal_citizens"; }
        }

        public string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей обращения граждан"; }
        }

        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is AppealCits)
            {
                var appCitsRodomain = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
                
                try
                {
                    var appealCits = statefulEntity as AppealCits;

                    if (!appCitsRodomain.GetAll().Any(x => x.AppealCits.Id == appealCits.Id))
                    {
                        return ValidateResult.No("Обязательно наличие хотя бы одного дома во вкладке \"Место возникновения проблемы\"");
                    }
                    
                }
                finally
                {
                    Container.Release(appCitsRodomain);
                }
            }

            return ValidateResult.Yes();
        }
    }
}