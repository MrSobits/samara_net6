using Bars.Gkh.Entities;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class PublicServicesHelper : BindingBase
    {
        static public dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("PublicServices"))
                {
                    throw new SpecFlowException("Нет текущей коммунтальной услуги");
                }

                var current = ScenarioContext.Current.Get<dynamic>("PublicServices");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("PublicServices"))
                {
                    ScenarioContext.Current.Remove("PublicServices");
                }

                ScenarioContext.Current.Add("PublicServices", value);
            }
        }
    }
}