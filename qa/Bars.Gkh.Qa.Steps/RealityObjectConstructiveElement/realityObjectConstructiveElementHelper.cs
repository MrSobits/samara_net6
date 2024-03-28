namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using TechTalk.SpecFlow;

    class RealityObjectConstructiveElementHelper
    {
        public static RealityObjectConstructiveElement Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("RealityObjectConstructiveElement"))
                {
                    throw new SpecFlowException("Нет текущего конструктивного элемента жилищного дома");
                }

                var realityObjectConstructiveElement = ScenarioContext.Current.Get<RealityObjectConstructiveElement>("RealityObjectConstructiveElement");

                return realityObjectConstructiveElement;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("RealityObjectConstructiveElement"))
                {
                    ScenarioContext.Current.Remove("RealityObjectConstructiveElement");
                }

                ScenarioContext.Current.Add("RealityObjectConstructiveElement", value);
            }
        }
    }
}
