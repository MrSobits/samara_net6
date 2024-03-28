using Bars.Gkh.Entities.CommonEstateObject;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    public class TypeGroupHelper : BindingBase
    {
        static public GroupType Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("GroupType"))
                {
                    throw new SpecFlowException("Нет текущей группы ООИ");
                }

                var current = ScenarioContext.Current.Get<GroupType>("GroupType");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("GroupType"))
                {
                    ScenarioContext.Current.Remove("GroupType");
                }

                ScenarioContext.Current.Add("GroupType", value);
            }
        }
    }
}