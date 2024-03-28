namespace Bars.Gkh.Qa.Steps
{
    
    
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    public class RubricHelper : BindingBase
    {
        /// <summary>
        /// Рубрика
        /// </summary>
        static public Entities.Suggestion.Rubric Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("RubricHelper"))
                {
                    throw new SpecFlowException("Нет текущей рубрики");
                }

                var current = ScenarioContext.Current.Get<Entities.Suggestion.Rubric>("RubricHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("RubricHelper"))
                {
                    ScenarioContext.Current.Remove("RubricHelper");
                }

                ScenarioContext.Current.Add("RubricHelper", value);
            }
        }
    }
}