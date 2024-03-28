namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class PaymentDocInfoHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return _type
                       ?? (_type =
                           Type.GetType(
                               "Bars.Gkh.RegOperator.Entities.Dict.PaymentDocInfo, Bars.Gkh.RegOperator"));
            }
        }

        public static IDomainService DomainService
        {
            get
            {
                var t = typeof(IDomainService<>).MakeGenericType(Type);
                return (IDomainService)Container.Resolve(t);
            }
        }

        /// <summary>
        /// Текущая информация для физ. лиз
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPaymentDocInfo"))
                {
                    throw new SpecFlowException("Отсутствует Текущая информация для физ. лиз");
                }

                var current = ScenarioContext.Current.Get<dynamic>("CurrentPaymentDocInfo");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPaymentDocInfo"))
                {
                    ScenarioContext.Current.Remove("CurrentPaymentDocInfo");
                }

                ScenarioContext.Current.Add("CurrentPaymentDocInfo", value);
            }
        }
    }
}
