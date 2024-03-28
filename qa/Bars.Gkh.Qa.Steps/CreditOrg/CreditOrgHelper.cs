using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;

    internal static class CreditOrgHelper
    {
        /// <summary>
        /// Кредитная организация
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CreditOrgHelper"))
                {
                    throw new SpecFlowException("Отсутствует текущая кредитная организация");
                }

                var current = ScenarioContext.Current.Get<dynamic>("CreditOrgHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CreditOrgHelper"))
                {
                    ScenarioContext.Current.Remove("CreditOrgHelper");
                }

                ScenarioContext.Current.Add("CreditOrgHelper", value);
            }
        }
    }
}
