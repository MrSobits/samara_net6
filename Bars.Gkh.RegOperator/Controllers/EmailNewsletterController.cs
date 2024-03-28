namespace Bars.Gkh.RegOperator.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Tasks.EmailNewsletter;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;
    using Microsoft.AspNetCore.Mvc;

    internal class EmailNewsletterController : FileStorageDataController<EmailNewsletter>
    {
        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Отправить платежные документы по эл. почте
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public ActionResult SendEmails(BaseParams @params)
        {
            var result = TaskManager.CreateTasks(new EmailNewsletterTaskProvider(this.Container), @params);
            return result.Success
                ? new JsonNetResult(new { sucess = true, message = "Задачи успешно поставлены в очередь на выполнение" })
                : this.JsFailure(result.Message);
        }
    }
}