namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class CommonEstateObjectHelper : BindingBase
    {
        static public Entities.CommonEstateObject.CommonEstateObject Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ComonEstateObjectHelper"))
                {
                    throw new SpecFlowException("Нет текущего объекта общего имущества");
                }

                var current = ScenarioContext.Current.Get<Entities.CommonEstateObject.CommonEstateObject>("ComonEstateObjectHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ComonEstateObjectHelper"))
                {
                    ScenarioContext.Current.Remove("ComonEstateObjectHelper");
                }

                ScenarioContext.Current.Add("ComonEstateObjectHelper", value);
            }
        }
    }
}