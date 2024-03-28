namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class SuspenseAccountHelper : BindingBase
    {
        /// <summary>
        /// Текущий Счет НВС
        /// </summary>
        public static SuspenseAccount Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentSuspenseAccount"))
                {
                    throw new SpecFlowException("Отсутствует текущий Счет НВС");
                }

                var current = ScenarioContext.Current.Get<SuspenseAccount>("CurrentSuspenseAccount");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentSuspenseAccount"))
                {
                    ScenarioContext.Current.Remove("CurrentSuspenseAccount");
                }

                ScenarioContext.Current.Add("CurrentSuspenseAccount", value);
            }
        }
    }
}
