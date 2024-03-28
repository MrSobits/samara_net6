using Bars.Gkh.Entities;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class TypeServicesHelper : BindingBase
    {
        static public TypeService Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("TypeServices"))
                {
                    throw new SpecFlowException("Нет текущего типа обслуживания");
                }

                var current = ScenarioContext.Current.Get<TypeService>("TypeServices");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("TypeServices"))
                {
                    ScenarioContext.Current.Remove("TypeServices");
                }

                ScenarioContext.Current.Add("TypeServices", value);
            }
        }
    }
}