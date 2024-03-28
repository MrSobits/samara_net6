namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class DecisionNotificationHelper : BindingBase
    {
        /// <summary>
        /// текущее уведомление
        /// </summary>
        public static DecisionNotification Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentDecisionNotification"))
                {
                    throw new SpecFlowException("Отсутствует текущее уведомление");
                }

                var decisionNotification = ScenarioContext.Current.Get<DecisionNotification>("CurrentDecisionNotification");

                return decisionNotification;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentDecisionNotification"))
                {
                    ScenarioContext.Current.Remove("CurrentDecisionNotification");
                }

                ScenarioContext.Current.Add("CurrentDecisionNotification", value);
            }
        }
    }
}
