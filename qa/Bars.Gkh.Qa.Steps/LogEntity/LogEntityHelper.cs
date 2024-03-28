namespace Bars.Gkh.Qa.Steps
{
    using System.Linq;

    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    internal class LogEntityHelper : BindingBase
    {
        /// <summary>
        /// Текущая Запись Журнала действия пользователя
        /// </summary>
        public static LogEntity Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentLogEntity"))
                {
                    throw new SpecFlowException("отсутствует текущая запись Журнала действия пользователя");
                }

                return ScenarioContext.Current.Get<LogEntity>("CurrentLogEntity");
            }
            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentLogEntity"))
                {
                    ScenarioContext.Current.Remove("CurrentLogEntity");
                }

                ScenarioContext.Current.Add("CurrentLogEntity", value);
            }
        }

        public static LoggedEntityInfo GetLoggedEntitiesbyEntityName(string entityName)
        {
            var objectToSelect = Container.Resolve<IChangeLogInfoProvider>()
                .GetLoggedEntities()
                .AsQueryable()
                .FirstOrDefault(x => x.EntityName == entityName);

            if (objectToSelect == null)
            {
                throw new SpecFlowException(string.Format("отсутсвует объект с наименованием {0}", entityName));
            }

            return objectToSelect;
        }
    }
}
