namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class DebtorClaimWorkHelper : BindingBase
    {
        /// <summary>
        /// претензионно исковая работа
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentDebtorClaimWork"))
                {
                    throw new SpecFlowException("Нет текущей претензионно исковой работы");
                }

                var current = ScenarioContext.Current.Get<dynamic>("CurrentDebtorClaimWork");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentDebtorClaimWork"))
                {
                    ScenarioContext.Current.Remove("CurrentDebtorClaimWork");
                }

                ScenarioContext.Current.Add("CurrentDebtorClaimWork", value);
            }
        }
    }
}
