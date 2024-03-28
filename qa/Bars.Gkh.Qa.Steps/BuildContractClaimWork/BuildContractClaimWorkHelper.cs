namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    class BuildContractClaimWorkHelper : BindingBase
    {
        /// <summary>
        /// Подрядчики, нарушившие условия договора
        /// </summary>
        public static object Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentBuildContractClaimWork"))
                {
                    throw new SpecFlowException("Нет текущего подрядчика, нарушевшего договор");
                }

                var current = ScenarioContext.Current.Get<object>("CurrentBuildContractClaimWork");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentBuildContractClaimWork"))
                {
                    ScenarioContext.Current.Remove("CurrentBuildContractClaimWork");
                }

                ScenarioContext.Current.Add("CurrentBuildContractClaimWork", value);
            }
        }
    }
}
