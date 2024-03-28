using Bars.Gkh.Entities;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class PositionHelper : BindingBase
    {
        static public Position Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("Position"))
                {
                    throw new SpecFlowException("Нет текущей должности");
                }

                var position = ScenarioContext.Current.Get<Position>("Position");

                return position;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("Position"))
                {
                    ScenarioContext.Current.Remove("Position");
                }

                ScenarioContext.Current.Add("Position", value);
            }
        }
    }
}