namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using TechTalk.SpecFlow;

    internal class PaymentPenaltiesHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return typeof(PaymentPenalties);
            }
        }

        public static IDomainService<PaymentPenalties> DomainService
        {
            get
            {
                return Container.Resolve<IDomainService<PaymentPenalties>>();
            }
        }

        /// <summary>
        /// Текущие параметры начисления пени
        /// </summary>
        public static PaymentPenalties Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPaymentPenalties"))
                {
                    throw new SpecFlowException("Отсутствуют текущие параметры начисления пени");
                }

                var current = ScenarioContext.Current.Get<PaymentPenalties>("CurrentPaymentPenalties");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPaymentPenalties"))
                {
                    ScenarioContext.Current.Remove("CurrentPaymentPenalties");
                }

                ScenarioContext.Current.Add("CurrentPaymentPenalties", value);
            }
        }
    }
}
