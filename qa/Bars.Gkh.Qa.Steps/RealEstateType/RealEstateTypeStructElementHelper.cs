namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    public class RealEstateTypeStructElementHelper : BindingBase
    {
        static public RealEstateTypeStructElement Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("RealEstateTypeStructElement"))
                {
                    throw new SpecFlowException("Нет текущего Конструктивного элемента типа дома");
                }

                var current = ScenarioContext.Current.Get<RealEstateTypeStructElement>("RealEstateTypeStructElement");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("RealEstateTypeStructElement"))
                {
                    ScenarioContext.Current.Remove("RealEstateTypeStructElement");
                }

                ScenarioContext.Current.Add("RealEstateTypeStructElement", value);
            }
        }
    }
}