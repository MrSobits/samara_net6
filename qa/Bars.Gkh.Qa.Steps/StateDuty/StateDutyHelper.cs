namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class StateDutyHelper : BindingBase
    {
        /// <summary>
        /// Госпошлина
        /// </summary>
        static public Modules.ClaimWork.Entities.StateDuty Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("StateDutyHelper"))
                {
                    throw new SpecFlowException("Нет текущей госпошлины");
                }

                var current = ScenarioContext.Current.Get<Modules.ClaimWork.Entities.StateDuty>("StateDutyHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("StateDutyHelper"))
                {
                    ScenarioContext.Current.Remove("StateDutyHelper");
                }

                ScenarioContext.Current.Add("StateDutyHelper", value);
            }
        }
    }
}