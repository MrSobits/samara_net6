using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;

    internal static class BuilderHelper
    {
        /// <summary>
        /// Текущая подрядная организация
        /// </summary>
        public static Builder Current 
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentBuilder"))
                {
                    throw new SpecFlowException("Отсутствует текущая подрядная организация");
                }

                var current = ScenarioContext.Current.Get<Builder>("CurrentBuilder");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentBuilder"))
                {
                    ScenarioContext.Current.Remove("CurrentBuilder");
                }

                ScenarioContext.Current.Add("CurrentBuilder", value);
            }
        }
    }
}
