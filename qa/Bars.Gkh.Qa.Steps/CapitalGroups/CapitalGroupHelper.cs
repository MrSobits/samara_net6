namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    class CapitalGroupHelper : BindingBase
    {
        /// <summary>
        /// Текущяя группа капитальности
        /// </summary>
        public static CapitalGroup Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentCapitalGroup"))
                {
                    throw new SpecFlowException("Нет текущей группы капитальности");
                }

                var capitalGroup = ScenarioContext.Current.Get<CapitalGroup>("currentCapitalGroup");

                return capitalGroup;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentCapitalGroup"))
                {
                    ScenarioContext.Current.Remove("currentCapitalGroup");
                }

                ScenarioContext.Current.Add("currentCapitalGroup", value);
            }
        }
    }
}
