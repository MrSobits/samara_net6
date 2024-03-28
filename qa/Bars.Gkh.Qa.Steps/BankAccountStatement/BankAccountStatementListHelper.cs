namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;
    using Bars.Gkh.RegOperator.Entities;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    internal static class BankAccountStatementListHelper
    {
        /// <summary>
        /// Список банковских операций
        /// </summary>
        public static List<BankAccountStatement> Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("BankAccountStatementListHelper"))
                {
                    throw new SpecFlowException("Отсутствует список банковских операций");
                }

                var current = ScenarioContext.Current.Get<List<BankAccountStatement>>("BankAccountStatementListHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("BankAccountStatementListHelper"))
                {
                    ScenarioContext.Current.Remove("BankAccountStatementListHelper");
                }

                ScenarioContext.Current.Add("BankAccountStatementListHelper", value);
            }
        }
    }
}
