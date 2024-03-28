namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    public class RealEstateTypeHelper : BindingBase
    {
        static public RealEstateType Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("RealEstateType"))
                {
                    throw new SpecFlowException("Нет текущего типа дома");
                }

                var realEstateType = ScenarioContext.Current.Get<RealEstateType>("RealEstateType");

                return realEstateType;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("RealEstateType"))
                {
                    ScenarioContext.Current.Remove("RealEstateType");
                }

                ScenarioContext.Current.Add("RealEstateType", value);
            }
        }
    }
}