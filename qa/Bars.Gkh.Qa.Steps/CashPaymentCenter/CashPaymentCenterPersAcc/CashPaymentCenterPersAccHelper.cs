namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    class CashPaymentCenterPersAccHelper : BindingBase
    {
        /// <summary>
        /// Объект Расчетно-кассового центра
        /// </summary>
        public static CashPaymentCenterPersAcc Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CashPaymentCenterPersAccCurrent"))
                {
                    throw new SpecFlowException("Отсутствует текущий объект Расчетно-кассового центра");
                }

                var current = ScenarioContext.Current.Get<CashPaymentCenterPersAcc>("CashPaymentCenterPersAccCurrent");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CashPaymentCenterPersAccCurrent"))
                {
                    ScenarioContext.Current.Remove("CashPaymentCenterPersAccCurrent");
                }

                ScenarioContext.Current.Add("CashPaymentCenterPersAccCurrent", value);
            }
        }
    }
}
