namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    internal static class BaseJurPersonHelper
    {
        /// <summary>
        /// Плановая проверка юр лица
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("BaseJurPersonHelper"))
                {
                    throw new SpecFlowException("Отсутствует текущая плановая проверка юр лица");
                }

                var current = ScenarioContext.Current.Get<dynamic>("BaseJurPersonHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("BaseJurPersonHelper"))
                {
                    ScenarioContext.Current.Remove("BaseJurPersonHelper");
                }

                ScenarioContext.Current.Add("BaseJurPersonHelper", value);
            }
        }
    }
}
