namespace Bars.Gkh.Qa.Steps
{

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    public class ProblemPlaceHelper : BindingBase
    {
        /// <summary>
        /// Место проблемы
        /// </summary>

        static public Entities.Suggestion.ProblemPlace Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ProblemPlaceHelper"))
                {
                    throw new SpecFlowException("Нет текущего Места проблемы");
                }

                var current = ScenarioContext.Current.Get<Entities.Suggestion.ProblemPlace>("ProblemPlaceHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ProblemPlaceHelper"))
                {
                    ScenarioContext.Current.Remove("ProblemPlaceHelper");
                }

                ScenarioContext.Current.Add("ProblemPlaceHelper", value);
            }
        }
    }
}