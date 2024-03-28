namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class GroupTypeHelper : BindingBase
    {
        static public Entities.CommonEstateObject.GroupType Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("GroupTypeHelper"))
                {
                    throw new SpecFlowException("Нет текущего типа группы ООИ");
                }

                var current = ScenarioContext.Current.Get<Entities.CommonEstateObject.GroupType>("GroupTypeHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("GroupTypeHelper"))
                {
                    ScenarioContext.Current.Remove("GroupTypeHelper");
                }

                ScenarioContext.Current.Add("GroupTypeHelper", value);
            }
        }
    }
}