namespace Bars.Gkh.Reforma.Domain.Impl
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Entities.Log;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.Performer;

    using Castle.MicroKernel;
    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с логами интеграции с Реформой ЖКХ
    /// </summary>
    public class SyncLogService : ISyncLogService
    {
        #region Constructors and Destructors
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public SyncLogService(IWindsorContainer container)
        {
            this.Container = container;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Контейнер
        /// </summary>
        protected IWindsorContainer Container { get; set; }

        #endregion

        #region Public Methods and Operators
        /// <summary>
        /// Вернуть лог-список по методу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список логов</returns>
        public IDataResult GetActionDetails(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var sessionId = baseParams.Params.Get("sessionId").ToLong();
            var actionName = baseParams.Params.Get("action").ToStr();
            if (sessionId == 0)
            {
                return new BaseDataResult(false, "Не указан идентификатор сессии");
            }

            if (string.IsNullOrEmpty(actionName))
            {
                return new BaseDataResult(false, "Не указано имя действия");
            }

            var service = this.Container.ResolveDomain<ActionLogItem>();
            try
            {
                var query =
                    service.GetAll()
                           .Where(x => x.Session.Id == sessionId && x.Name == actionName)
                           .Select(
                               x =>
                               new
                               {
                                   x.Id,
                                   x.Name,
                                   x.RequestTime,
                                   x.ResponseTime,
                                   x.Success,
                                   x.ErrorCode,
                                   x.ErrorName,
                                   x.ErrorDescription,
                                   x.Details,
                                   FileId = x.Packets != null ? x.Packets.Id : 0
                               })
                           .Filter(loadParams, this.Container);
                return new ListDataResult(query.Order(loadParams).Paging(loadParams), query.Count());
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Вернуть список выполненных действий по сессии
        /// </summary>
        /// <param name="baseParams">Базовые параметыр запроса</param>
        /// <returns>Список действий</returns>
        public IDataResult ListActions(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var sessionId = baseParams.Params.Get("sessionId").ToLong();
            if (sessionId == 0)
            {
                return new BaseDataResult(false, "Не указан идентификатор сессии");
            }

            var service = this.Container.ResolveDomain<ActionLogItem>();
            try
            {
                var query = service.GetAll().Where(x => x.Session.Id == sessionId);
                var actions =
                    query.AsEnumerable()
                        .GroupBy(x => x.Name)
                        .Select(
                            x =>
                            new
                                {
                                    SessionId = sessionId,
                                    Name = x.Key,
                                    Total = x.Count(),
                                    Failed = x.Count(y => !y.Success),
                                    RequestTime = x.Select(y => y.RequestTime).OrderBy(y => y).FirstOrDefault()
                                })
                        .OrderBy(x => x.RequestTime)
                        .AsQueryable()
                        .Filter(loadParams, this.Container);

                return new ListDataResult(actions.Order(loadParams).Paging(loadParams), actions.Count());
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Вернуть список сессий интеграций
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список сессий</returns>
        public IDataResult ListSessions(BaseParams baseParams)
        {
            var service = this.Container.ResolveDomain<SessionLogItem>();
            var loadParams = baseParams.GetLoadParam();
            try
            {
                var query = service.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.StartTime,
                        x.EndTime,
                        x.SessionId,
                        x.TypeIntegration
                    })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(query.Order(loadParams).Paging(loadParams), query.Count());
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Повторить действие
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат повтора</returns>
        public IDataResult ReplayAction(BaseParams baseParams)
        {
            var actionId = baseParams.Params.Get("id").ToLong();
            if (actionId == 0)
            {
                return new BaseDataResult(false, "Не указан идентификатор действия");
            }

            var service = this.Container.ResolveDomain<ActionLogItem>();
            try
            {
                var action = service.Get(actionId);
                if (action == null)
                {
                    return new BaseDataResult(false, "Действие с указанным идентификатором не найдено");
                }

                SyncActionResult result = null;

                using (this.Container.BeginScope())
                {
                    result = this.ReplayActionInternal(action.Name, Encoding.UTF8.GetString(action.Parameters), action.Session.SessionId);
                }
                if (result.Success)
                {
                    return new BaseDataResult();
                }
                else
                {
                    return new BaseDataResult(false, result.ErrorDetails.Description) { Data = result.ErrorDetails };
                }
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        private SyncActionResult ReplayActionInternal(string actionName, string parameters, string sessionId)
        {
            var argument = new Arguments { { "sessionId", Guid.Parse(sessionId) } };
            var provider = this.Container.Resolve<ISyncProvider>(argument);
            var args = new Arguments { { typeof(ISyncProvider), provider } };

            var action = this.Container.Resolve<ISyncAction>(actionName, args);
            try
            {
                action.SerializedParameters = parameters;
                return action.Perform();
            }
            finally
            {
                provider.Close();
                this.Container.Release(action);
                this.Container.Release(provider);
            }
        }

        #endregion
    }
}