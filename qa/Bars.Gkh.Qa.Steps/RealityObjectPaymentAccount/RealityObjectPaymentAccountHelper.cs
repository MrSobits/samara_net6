namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Runtime.InteropServices;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class RealityObjectPaymentAccountHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return _type
                       ?? (_type =
                           Type.GetType(
                               "Bars.Gkh.RegOperator.Entities.RealityObjectPaymentAccount, Bars.Gkh.RegOperator"));
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
        /// Текущий Счет оплат дома
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentRealityObjectLoan"))
                {
                    throw new SpecFlowException("Отсутствует текущий Счет оплат дома");
                }

                var current = ScenarioContext.Current.Get<dynamic>("CurrentRealityObjectLoan");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentRealityObjectLoan"))
                {
                    ScenarioContext.Current.Remove("CurrentRealityObjectLoan");
                }

                ScenarioContext.Current.Add("CurrentRealityObjectLoan", value);
            }
        }
    }
}
