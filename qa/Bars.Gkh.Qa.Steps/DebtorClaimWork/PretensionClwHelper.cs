namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class PretensionClwHelper : BindingBase
    {
        /// <summary>
        /// Текущяя претензия
        /// </summary>
        public static PretensionClw Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPretensionClw"))
                {
                    throw new SpecFlowException("Отсутствует текущяя претензия");
                }

                var document = ScenarioContext.Current.Get<PretensionClw>("CurrentPretensionClw");

                return document;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPretensionClw"))
                {
                    ScenarioContext.Current.Remove("CurrentPretensionClw");
                }

                ScenarioContext.Current.Add("CurrentPretensionClw", value);
            }
        }
    }
}
