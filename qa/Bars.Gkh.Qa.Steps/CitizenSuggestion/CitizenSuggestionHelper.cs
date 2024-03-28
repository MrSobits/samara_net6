namespace Bars.Gkh.Qa.Steps
{

    using TechTalk.SpecFlow;

    internal static class CitizenSuggestionHelper
    {
        /// <summary>
        /// Обращения граждан 
        /// </summary>
        public static Entities.Suggestion.CitizenSuggestion Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CitizenSuggestionHelper"))
                {
                    throw new SpecFlowException("Отсутствует текущее обращение граждан");
                }

                var current =
                    ScenarioContext.Current.Get<Entities.Suggestion.CitizenSuggestion>("CitizenSuggestionHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CitizenSuggestionHelper"))
                {
                    ScenarioContext.Current.Remove("CitizenSuggestionHelper");
                }

                ScenarioContext.Current.Add("CitizenSuggestionHelper", value);
            }
        }
    }

}