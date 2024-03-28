namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class TableLockHelper : BindingBase
    {
        /// <summary>
        /// Текущая блокировка
        /// </summary>
        public static TableLock Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentTableLock"))
                {
                    throw new SpecFlowException("Отсутствует текущая блокировка");
                }

                var currentTaskEntry = ScenarioContext.Current.Get<TableLock>("CurrentTableLock");

                return currentTaskEntry;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentTableLock"))
                {
                    ScenarioContext.Current.Remove("CurrentTableLock");
                }

                ScenarioContext.Current.Add("CurrentTableLock", value);
            }
        }
    }
}
