namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class BuilderContractClaimWorkParamsHelper : BindingBase
    {
        /// <summary>
        /// насиройка реестра подрядчиков
        /// </summary>
        public static DynamicDictionary Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentBuilderContractClaimWorkParams"))
                {
                    throw new SpecFlowException("Нет текущей насиройки реестра подрядчиков");
                }

                var current = ScenarioContext.Current.Get<DynamicDictionary>("CurrentBuilderContractClaimWorkParams");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentBuilderContractClaimWorkParams"))
                {
                    ScenarioContext.Current.Remove("CurrentBuilderContractClaimWorkParams");
                }

                ScenarioContext.Current.Add("CurrentBuilderContractClaimWorkParams", value);
            }
        }
    }
}
