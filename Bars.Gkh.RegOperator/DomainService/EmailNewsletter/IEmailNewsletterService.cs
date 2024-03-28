namespace Bars.Gkh.RegOperator.DomainService.EmailNewsletter
{
    using B4;

    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Интерфейс рассылки на email
    /// </summary>
    public interface IEmailNewsletterService
    {
        /// <summary>
        /// Отправить рассылку по эл. почте
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="indicator">Индикатор выполнения задачи</param>
        IDataResult SendEmails(BaseParams @params, IProgressIndicator indicator);
    }
}