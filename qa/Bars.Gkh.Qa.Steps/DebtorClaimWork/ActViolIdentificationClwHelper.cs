namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class ActViolIdentificationClwHelper : BindingBase
    {
        /// <summary>
        /// Текущий Акт выявления нарушений уведомление ПИР
        /// </summary>
        public static ActViolIdentificationClw Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentActViolIdentificationClw"))
                {
                    throw new SpecFlowException("Отсутствует текущий Акт выявления нарушений уведомление ПИР");
                }

                var document = ScenarioContext.Current.Get<ActViolIdentificationClw>("CurrentActViolIdentificationClw");

                return document;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentActViolIdentificationClw"))
                {
                    ScenarioContext.Current.Remove("CurrentActViolIdentificationClw");
                }

                ScenarioContext.Current.Add("CurrentActViolIdentificationClw", value);
            }
        }
    }
}
