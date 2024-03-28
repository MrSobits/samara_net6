namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    public class FinanceSourceHelper : BindingBase
    {
        static public FinanceSource Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("FinanceSource"))
                {
                    throw new SpecFlowException("Нет текущего разреза финансирования");
                }

                var current = ScenarioContext.Current.Get<FinanceSource>("FinanceSource");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("FinanceSource"))
                {
                    ScenarioContext.Current.Remove("FinanceSource");
                }

                ScenarioContext.Current.Add("FinanceSource", value);
            }
        }
    }
}