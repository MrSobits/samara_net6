namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments
{
    using System;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;

    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;

    using Castle.Windsor;

    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Executor для отправки платежных документов по эл. почте
    /// </summary>
    public class PaymentDocumentEmailSendTaskExecutor : ITaskExecutor
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
        public IPaymentDocumentService PaymentDocumentService { get; set; }

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
                return this.PaymentDocumentService.SendEmails(@params, indicator);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error("message: {0} {1}\r\nstacktrace: {2}".FormatUsing(exception.Message, exception.InnerException, exception.StackTrace));
            }
        }
    }
}
