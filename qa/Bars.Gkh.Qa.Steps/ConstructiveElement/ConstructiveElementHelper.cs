namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using TechTalk.SpecFlow;

    class ConstructiveElementHelper
    {
        public static ConstructiveElement Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("constructiveElement"))
                {
                    throw new SpecFlowException("Нет текущей группы конструктивных элементов");
                }

                var constructiveElement = ScenarioContext.Current.Get<ConstructiveElement>("constructiveElement");

                return constructiveElement;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("constructiveElement"))
                {
                    ScenarioContext.Current.Remove("constructiveElement");
                }

                ScenarioContext.Current.Add("constructiveElement", value);
            }
        }
    }
}
