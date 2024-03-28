namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;
    using Bars.Gkh.Entities;

    internal static class FeatureViolGjiHelper
    {
        /// <summary>
        /// Группа нарушений
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("FeatureViolGjiHelper"))
                {
                    throw new SpecFlowException("Отсутствует текущая группа нарушений");
                }

                var current = ScenarioContext.Current.Get<dynamic>("FeatureViolGjiHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("FeatureViolGjiHelper"))
                {
                    ScenarioContext.Current.Remove("FeatureViolGjiHelper");
                }

                ScenarioContext.Current.Add("FeatureViolGjiHelper", value);
            }
        }
    }
}
