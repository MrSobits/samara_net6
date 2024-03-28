namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class LawsuitClwDocumentHelper : BindingBase
    {
        /// <summary>
        /// Текущий документ искового заявления
        /// </summary>
        public static LawsuitClwDocument Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentLawsuitClwDocument"))
                {
                    throw new SpecFlowException("Отсутствует текущий документ искового заявления");
                }

                var document = ScenarioContext.Current.Get<LawsuitClwDocument>("CurrentLawsuitClwDocument");

                return document;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentLawsuitClwDocument"))
                {
                    ScenarioContext.Current.Remove("CurrentLawsuitClwDocument");
                }

                ScenarioContext.Current.Add("CurrentLawsuitClwDocument", value);
            }
        }
    }
}
