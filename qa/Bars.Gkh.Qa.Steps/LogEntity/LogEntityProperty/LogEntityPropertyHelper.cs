namespace Bars.Gkh.Qa.Steps
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class LogEntityPropertyHelper : BindingBase
    {
        /// <summary>
        /// Текущая детальная информация записи Журнала действия пользователя
        /// </summary>
        /// Специально написал Current с одной r иначе вызываеться во всех тестах по не понятной причине
        public static LogEntityProperty Curent
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentLogEntityProperty"))
                {
                    throw new SpecFlowException("отсутствует текущая детальная информация записи Журнала действия пользователя");
                }

                return ScenarioContext.Current.Get<LogEntityProperty>("CurrentLogEntityProperty");
            }
            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentLogEntityProperty"))
                {
                    ScenarioContext.Current.Remove("CurrentLogEntityProperty");
                }

                ScenarioContext.Current.Add("CurrentLogEntityProperty", value);
            }
        }
    }
}
