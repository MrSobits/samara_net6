namespace Bars.Gkh.Reforma.Impl.Logger
{
    // ReSharper disable once RedundantUsingDirective
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Reforma.Entities.Log;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface.Logger;
    using Bars.Gkh.Reforma.Interface.Session;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    using Microsoft.Extensions.Logging;

    // ReSharper disable once RedundantUsingDirective
    using Newtonsoft.Json;

    /// <summary>
    ///     Логгер запросов к сервису Реформы
    /// </summary>
    public class SyncLogger : ISyncLogger
    {
        private static readonly int ConcurrencyLevel = Environment.ProcessorCount * 2;

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="session">Сессия синхронизации</param>
        public SyncLogger(IWindsorContainer container, ISyncSession session)
        {
            this.container = container;
            StoreSession(session);
            session.SessionClosed += OnSessionClosed;
            items = new List<LogItem>(ConcurrencyLevel * 2 + 1);
        }

        #endregion

        private class LogItem
        {
            #region Public Properties

            public string Action { get; set; }

            public string Parameters { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }

            public string Details { get; set; }

            public SyncActionResult Result { get; set; }

            public Exception Exception { get; set; }

            public List<RequestItem> RequestItems { get; set; }

            public List<string> Messages { get; set; }

            #endregion
        }

        private class RequestItem
        {
            public string Request { get; set; }

            public DateTime RequestTime { get; set; }

            public string Response { get; set; }

            public DateTime ResponseTime { get; set; }
        }

        #region Fields

        private readonly IWindsorContainer container;

        private readonly List<LogItem> items;

        private readonly object lockObject = new object();

        [ThreadStatic]
        private static LogItem currentItem;

        private long sessionLogItemId;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Окончание вызова метода
        /// </summary>
        /// <param name="result">Результат действия</param>
        public void EndActionInvocation(SyncActionResult result)
        {
            currentItem.EndTime = DateTime.UtcNow;
            currentItem.Result = result;
        }

        /// <summary>
        ///     Исключение
        /// </summary>
        /// <param name="e">Исключение</param>
        public void SetException(Exception e)
        {
            if (currentItem == null)
            {
                StartActionInvocation("Исключительная ситуация", null);
            }

            // ReSharper disable once PossibleNullReferenceException
            currentItem.Exception = e;
        }

        public void AddMessage(string message)
        {
            currentItem.Messages.Add(message);
        }

        /// <summary>
        ///     Указывает детали действия
        /// </summary>
        /// <param name="details">Детали</param>
        public void SetActionDetails(string details)
        {
            currentItem.Details = details;
        }

        /// <summary>
        ///     Исходящий запрос
        /// </summary>
        /// <param name="content">Содержимое запроса</param>
        public void LogRequest(string content)
        {
            currentItem.RequestItems.Add(new RequestItem { RequestTime = DateTime.UtcNow, Request = content });
        }

        /// <summary>
        ///     Ответ от сервера
        /// </summary>
        /// <param name="content">Содержимое ответа</param>
        public void LogResponse(string content)
        {
            var request = currentItem.RequestItems.LastOrDefault();
            if (request == null)
            {
                return;
            }
            request.ResponseTime = DateTime.UtcNow;
            request.Response = content;
        }

        /// <summary>
        ///     Начало вызова метода
        /// </summary>
        /// <param name="actionId">Имя метода</param>
        /// <param name="parameters">Параметры вызова</param>
        public void StartActionInvocation(string actionId, string parameters)
        {
            if (items.Count >= ConcurrencyLevel)
            {
                FlushItemsScoped();
            }

            lock (this.lockObject)
            {
                currentItem = new LogItem { Action = actionId, Parameters = parameters, StartTime = DateTime.UtcNow, RequestItems = new List<RequestItem>(), Messages = new List<string>() };
                items.Add(currentItem);
            }
        }

        #endregion

        #region Methods

        private void StoreSession(ISyncSession session)
        {
            var sessionLogService = container.ResolveDomain<SessionLogItem>();
            try
            {
                var sessionId = session.SessionId.ToString();
                var sessionLogItem = sessionLogService.GetAll().FirstOrDefault(x => x.SessionId == sessionId) ?? new SessionLogItem
                {
                    SessionId = sessionId,
                    StartTime = session.StartTime,
                    TypeIntegration = session.TypeIntegration
                };

                if (sessionLogItem.Id == 0)
                {
                    sessionLogService.Save(sessionLogItem);
                }
                else
                {
                    sessionLogService.Update(sessionLogItem);
                }

                sessionLogItemId = sessionLogItem.Id;
            }
            finally
            {
                container.Release(sessionLogService);
            }
        }

        private void OnSessionClosed(ISyncSession session)
        {
            session.SessionClosed -= OnSessionClosed;

            FlushItemsScoped(true);

            if (!session.Resurrected)
            {
                var sessionLogService = container.ResolveDomain<SessionLogItem>();
                try
                {
                    var sessionLogItem = sessionLogService.Get(sessionLogItemId);
                    sessionLogItem.EndTime = DateTime.UtcNow;
                    sessionLogService.Update(sessionLogItem);
                }
                finally
                {
                    container.Release(sessionLogService);
                }
            }
        }

        private void FlushItemsScoped(bool force = false)
        {
            using (this.container.BeginScope())
            {
                this.FlushItems(force);
            }
        }

        private void FlushItems(bool force)
        {
            if (items.Count == 0)
            {
                return;
            }

            lock (this.lockObject)
            {
                if (items.Count == 0)
                {
                    return;
                }

                if (!force)
                {
                    while (true)
                    {
                        if (items.All(x => x.EndTime != default(DateTime)))
                        {
                            break;
                        }

                        Thread.Sleep(0);
                    }
                }

                var actionLogService = container.ResolveDomain<ActionLogItem>();
                var fileManager = container.Resolve<IFileManager>();
                try
                {
                    container.InTransaction(
                        () =>
                            {
                                foreach (var item in items)
                                {
                                    var actionLogItem = new ActionLogItem
                                                            {
                                                                Session = new SessionLogItem { Id = sessionLogItemId },
                                                                Parameters = Encoding.UTF8.GetBytes(item.Parameters),
                                                                RequestTime = item.StartTime,
                                                                ResponseTime = item.EndTime,
                                                                Success = item.Result.Success,
                                                                Name = item.Action,
                                                                Details = item.Details
                                                            };

                                    if (!item.Result.Success)
                                    {
                                        actionLogItem.ErrorCode = item.Result.ErrorDetails.Code;
                                        actionLogItem.ErrorName = item.Result.ErrorDetails.Name;
                                        actionLogItem.ErrorDescription = item.Result.ErrorDetails.Description;
                                    }
                                    else if (item.Exception != null)
                                    {
                                        actionLogItem.Success = false;
                                        actionLogItem.ErrorName = item.Exception.GetType().Name;
                                        actionLogItem.ErrorDescription = item.Exception.Message;
                                    }

                                    using (var ms = new MemoryStream())
                                    {
                                        using (var file = new ZipFile(Encoding.UTF8) { AlternateEncoding = Encoding.UTF8, CompressionLevel = CompressionLevel.BestCompression })
                                        {
                                            var i = 0;
                                            foreach (var request in item.RequestItems)
                                            {
                                                i++;
                                                file.AddEntry(string.Format("request_{0}_{1}.xml", i, request.RequestTime.ToString("yyyyMMdd_HHmmss")), request.Request, Encoding.UTF8);
                                                if (request.ResponseTime > DateTime.MinValue)
                                                {
                                                    file.AddEntry(string.Format("response_{0}_{1}.xml", i, request.ResponseTime.ToString("yyyyMMdd_HHmmss")), request.Response, Encoding.UTF8);
                                                }
                                            }

                                            if (item.Messages.Count > 0)
                                            {
                                                file.AddEntry("messages.txt", string.Join("\r\n", item.Messages));
                                            }

                                            if (item.Exception != null)
                                            {
                                                file.AddEntry("exception.txt", item.Exception.ToString(), Encoding.UTF8);
                                            }

#if DEBUG
                                            try
                                            {
                                                file.AddEntry("details.json", JsonNetConvert.SerializeObject(container, item, Formatting.Indented), Encoding.UTF8);
                                            }
                                            catch
                                            {
                                                // ignored
                                            }
#endif
                                            file.Save(ms);
                                        }

                                        try
                                        {
                                            actionLogItem.Packets = fileManager.SaveFile(ms, string.Format("{0}.zip", actionLogItem.Name));
                                        }
                                        catch(Exception ex)
                                        {
                                            var logManager = this.container.Resolve<ILogger>();
                                            using (this.container.Using(logManager))
                                            {
                                                logManager.LogError(ex, "Ошибка сохранения файлов пакетов");
                                            }
                                        }
                                    }

                                    actionLogService.Save(actionLogItem);
                                }
                            });
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    items.Clear();
                    container.Release(actionLogService);
                    container.Release(fileManager);
                }
            }
        }

        #endregion
    }
}