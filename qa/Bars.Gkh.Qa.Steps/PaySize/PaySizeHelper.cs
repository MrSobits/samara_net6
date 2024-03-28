namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Overhaul.Entities;

    public class PaySizeHelper : BindingBase
    {
        /// <summary>
        /// Размер взносов на КР
        /// </summary>

        static public Paysize Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PaySizeHelper"))
                {
                    throw new SpecFlowException("Нет текущего размера взносов на КР");
                }

                var current = ScenarioContext.Current.Get<Paysize>("PaySizeHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PaySizeHelper"))
                {
                    ScenarioContext.Current.Remove("PaySizeHelper");
                }

                ScenarioContext.Current.Add("PaySizeHelper", value);
            }
        }
    }
}