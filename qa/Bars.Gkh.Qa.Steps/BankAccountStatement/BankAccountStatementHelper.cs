namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class BankAccountStatementHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return typeof(BankAccountStatement);
            }
        }

        public static IDomainService<BankAccountStatement> DomainService
        {
            get
            {
                return Container.Resolve<IDomainService<BankAccountStatement>>();
            }
        }

        /// <summary>
        /// Банковская операция
        /// </summary>
        public static BankAccountStatement Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentBankAccountStatement"))
                {
                    throw new SpecFlowException("Отсутствует текущяя Банковская операция");
                }

                var current = ScenarioContext.Current.Get<BankAccountStatement>("CurrentBankAccountStatement");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentBankAccountStatement"))
                {
                    ScenarioContext.Current.Remove("CurrentBankAccountStatement");
                }

                ScenarioContext.Current.Add("CurrentBankAccountStatement", value);
            }
        }
    }
}
