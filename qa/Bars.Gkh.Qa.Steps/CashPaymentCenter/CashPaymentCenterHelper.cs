namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class CashPaymentCenterHelper : BindingBase
    {
        
        /// <summary>
        /// Расчетно-кассовые центры
        /// </summary>
        public static CashPaymentCenter Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CashPaymentCenterCurrent"))
                {
                    throw new SpecFlowException("Отсутствует текущий Расчетно-кассовый центр");
                }

                var current = ScenarioContext.Current.Get<CashPaymentCenter>("CashPaymentCenterCurrent");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CashPaymentCenterCurrent"))
                {
                    ScenarioContext.Current.Remove("CashPaymentCenterCurrent");
                }

                ScenarioContext.Current.Add("CashPaymentCenterCurrent", value);
            }
        }
    }
}
