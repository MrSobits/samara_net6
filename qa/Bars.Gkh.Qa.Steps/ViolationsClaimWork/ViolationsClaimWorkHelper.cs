namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Modules.ClaimWork.Entities;

    using TechTalk.SpecFlow;

    public class ViolationsClaimWorkHelper
    {
        static public ViolClaimWork Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ViolationsClaimWork"))
                {
                    throw new SpecFlowException("Нет текущего вида нарушений договора подряда");
                }

                var current  = ScenarioContext.Current.Get<ViolClaimWork>("ViolationsClaimWork");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ViolationsClaimWork"))
                {
                    ScenarioContext.Current.Remove("ViolationsClaimWork");
                }

                ScenarioContext.Current.Add("ViolationsClaimWork", value);
            }
        }
    }
}
