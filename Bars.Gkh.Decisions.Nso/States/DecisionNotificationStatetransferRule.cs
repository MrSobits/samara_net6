namespace Bars.Gkh.Decisions.Nso.States
{
    using System.Linq;
    using B4.Modules.States;
    using B4.Utils;

    public class DecisionNotificationStatetransferRule : IRuleChangeStatus
    {
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var properties = statefulEntity.GetType().GetProperties();

            if (properties.Any(property => property.GetValue(statefulEntity, new object[0]).IsNull()))
            {
                return ValidateResult.No("Не все поля заполнены. Перевод статуса запрещен!");
            }

            return new ValidateResult();
        }

        public string Id { get { return "gkh_decision_notification_rule"; } }

        public string Name { get { return "Проверка заполненности полей уведомления о принятии решения"; } }

        public string TypeId { get { return "gkh_decision_notification"; } }

        public string Description
        {
            get
            {
                return "Правило осуществляет проверку заполненности полей формы уведомления о принятии решения";
            }
        }
    }
}