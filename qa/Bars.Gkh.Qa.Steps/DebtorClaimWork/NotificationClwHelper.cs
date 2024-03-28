namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class NotificationClwHelper : BindingBase
    {
        /// <summary>
        /// Текущее уведомление ПИР
        /// </summary>
        public static NotificationClw Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentNotificationClw"))
                {
                    throw new SpecFlowException("Отсутствует текущее уведомление ПИР");
                }

                var document = ScenarioContext.Current.Get<NotificationClw>("CurrentNotificationClw");

                return document;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentNotificationClw"))
                {
                    ScenarioContext.Current.Remove("CurrentNotificationClw");
                }

                ScenarioContext.Current.Add("CurrentNotificationClw", value);
            }
        }
    }
}
