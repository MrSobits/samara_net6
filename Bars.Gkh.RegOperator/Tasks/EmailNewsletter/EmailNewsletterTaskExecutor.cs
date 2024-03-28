namespace Bars.Gkh.RegOperator.Tasks.EmailNewsletter
{
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Bars.Gkh.RegOperator.DomainService.EmailNewsletter;
    using Castle.Windsor;
    using System;
    using System.Reflection;
    using System.Threading;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Executor для отправки рассылки по эл. почте
    /// </summary>
    public class EmailNewsletterTaskExecutor : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис для работы с платежными документами
        /// </summary>
        public IEmailNewsletterService EmailNewsletterService { get; set; }

        /// <summary>
        /// Код исполнителя задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        /// <summary>Метод для выполнения задачи</summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Признак отмены</param>
        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                return EmailNewsletterService.SendEmails(@params, indicator);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error("message: {0} {1}\r\nstacktrace: {2}".FormatUsing(exception.Message, exception.InnerException, exception.StackTrace));
            }
        }
    }
}
