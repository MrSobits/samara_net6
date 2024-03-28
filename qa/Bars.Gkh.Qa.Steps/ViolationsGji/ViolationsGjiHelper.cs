namespace Bars.Gkh.Qa.Steps
{

    using TechTalk.SpecFlow;

    class ViolationsGjiHelper
    {

        /// <summary>
        /// Нарушение
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("ViolationsGjiHelper"))
                {
                    throw new SpecFlowException("Нет текущего нарушения");
                }

                var current = ScenarioContext.Current.Get<dynamic>("ViolationsGjiHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("ViolationsGjiHelper"))
                {
                    ScenarioContext.Current.Remove("ViolationsGjiHelper");
                }

                ScenarioContext.Current.Add("ViolationsGjiHelper", value);
            }
        }
    }
}
