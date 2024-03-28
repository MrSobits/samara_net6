namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl;

    using TechTalk.SpecFlow;

    internal class PersonalAccountOperationLogEntryHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return typeof(PersonalAccountOperationLogEntry);
            }
        }

        public static IDomainService<PersonalAccountOperationLogEntry> DomainService
        {
            get
            {
                return Container.Resolve<IDomainService<PersonalAccountOperationLogEntry>>();
            }
        }

        /// <summary>
        /// Запись в истории изменений ЛС
        /// </summary>
        public static PersonalAccountOperationLogEntry Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentPersonalAccountOperationLogEntry"))
                {
                    throw new SpecFlowException("Отсутствует текущая запись в истории изменений ЛС");
                }

                var current = ScenarioContext.Current.Get<PersonalAccountOperationLogEntry>("CurrentPersonalAccountOperationLogEntry");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentPersonalAccountOperationLogEntry"))
                {
                    ScenarioContext.Current.Remove("CurrentPersonalAccountOperationLogEntry");
                }

                ScenarioContext.Current.Add("CurrentPersonalAccountOperationLogEntry", value);
            }
        }
    }
}
