namespace Bars.Gkh.Qa.Steps
{
    using TechTalk.SpecFlow;

    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    class ContragentHelper : BindingBase
    {
        /// <summary>
        /// Текущий контрагент
        /// </summary>
        public static Contragent CurrentContragent
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentContragent"))
                {
                    throw new SpecFlowException("Нет текущего контрагента");
                }

                var contragent = ScenarioContext.Current.Get<Contragent>("currentContragent");

                return contragent;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentContragent"))
                {
                    ScenarioContext.Current.Remove("currentContragent");
                }

                ScenarioContext.Current.Add("currentContragent", value);
            }
        }
    }
}
