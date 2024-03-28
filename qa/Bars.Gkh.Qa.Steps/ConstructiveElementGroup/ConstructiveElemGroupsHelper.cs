namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using TechTalk.SpecFlow;

    class ConstructiveElementGroupHelper
    {
        public static ConstructiveElementGroup Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ConstructiveElementGroup"))
                {
                    throw new SpecFlowException("Нет текущей группы конструктивных элементов");
                }

                var ConstructiveElementGroup = ScenarioContext.Current.Get<ConstructiveElementGroup>("ConstructiveElementGroup");

                return ConstructiveElementGroup;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ConstructiveElementGroup"))
                {
                    ScenarioContext.Current.Remove("ConstructiveElementGroup");
                }

                ScenarioContext.Current.Add("ConstructiveElementGroup", value);
            }
        }
    }
}
