namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    public class PeriodHelper : BindingBase
    {
        /// <summary>
        /// Виды санкций
        /// </summary>

        static public Period Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("Period"))
                {
                    throw new SpecFlowException("Нет текущего периода");
                }

                var current = ScenarioContext.Current.Get<Period>("Period");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("Period"))
                {
                    ScenarioContext.Current.Remove("Period");
                }

                ScenarioContext.Current.Add("Period", value);
            }
        }
    }
}