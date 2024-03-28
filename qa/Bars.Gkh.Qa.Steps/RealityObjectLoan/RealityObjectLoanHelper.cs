namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class RealityObjectLoanHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return typeof(RealityObjectLoan);
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
        /// Текущий Займ
        /// </summary>
        public static dynamic Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentRealityObjectLoan"))
                {
                    throw new SpecFlowException("Отсутствует текущий займ");
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
