namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    public class RealEstateTypeCommonParamHelper : BindingBase
    {
        static public RealEstateTypeCommonParam Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("RealEstateTypeCommonParamHelper"))
                {
                    throw new SpecFlowException("Нет текущего Общего параметра типа дома");
                }

                var current = ScenarioContext.Current.Get<RealEstateTypeCommonParam>("RealEstateTypeCommonParamHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("RealEstateTypeCommonParamHelper"))
                {
                    ScenarioContext.Current.Remove("RealEstateTypeCommonParamHelper");
                }

                ScenarioContext.Current.Add("RealEstateTypeCommonParamHelper", value);
            }
        }
    }
}