namespace Bars.Gkh.RegOperator.Controllers.PersonalAccount.PayDoc
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Tasks.PaymentDocuments;

    internal class PaymentDocumentSnapshotController : BaseController
    {
        private readonly IDomainService<PaymentDocumentSnapshot> paymentDocumentSnapshotDomain;
        private readonly IViewModel<PaymentDocumentSnapshot> snapshotViewModel;
        private readonly IPaymentDocumentService paymentDocumentService;

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        public PaymentDocumentSnapshotController(
            IDomainService<PaymentDocumentSnapshot> paymentDocumentSnapshotDomain,
            IViewModel<PaymentDocumentSnapshot> snapshotViewModel,
            IPaymentDocumentService paymentDocumentService)
        {
            this.paymentDocumentSnapshotDomain = paymentDocumentSnapshotDomain;
            this.snapshotViewModel = snapshotViewModel;
            this.paymentDocumentService = paymentDocumentService;
        }

        public ActionResult List(BaseParams baseParams)
        {
            return this.Js(this.snapshotViewModel.List(this.paymentDocumentSnapshotDomain, baseParams));
        }

        public ActionResult Get(BaseParams baseParams)
        {
            return this.Js(this.snapshotViewModel.Get(this.paymentDocumentSnapshotDomain, baseParams));
        }

        public ActionResult CreateDocumentFromSnapshot(BaseParams baseParams)
        {
            var snapshotId = baseParams.Params.GetAsId("snapshotId");

            var stream = this.paymentDocumentService.CreateDocumentFromSnapshot(snapshotId);
            stream.Position = 0;

            this.Response.Headers.Add("Content-Disposition", "inline; filename=report.pdf");
            this.Response.Headers.Add("Content-Length", stream.Length.ToString());
            return this.File(stream, "application/pdf");
        }

        public ActionResult CreateDocsFromSnapshots(BaseParams prms)
        {
            return this.Js(this.paymentDocumentService.CreateDocumentsFromSnapshots(prms));
        }

        /// <summary>
        /// Отправить платежные документы по эл. почте
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public ActionResult SendEmails(BaseParams @params)
        {
            var result = this.TaskManager.CreateTasks(new PaymentDocumentEmailSendTaskProvider(this.Container), @params);
            return result.Success
                ? new JsonNetResult(new {sucess = true, message = "Задачи успешно поставлены в очередь на выполнение" })
                : this.JsFailure(result.Message);
        }

        public ActionResult Delete(BaseParams prms)
        {
            return this.Js(this.paymentDocumentService.DeleteSnapshots(prms));
        }

        public ActionResult SetEmails(BaseParams @params)
        {
            var result = this.TaskManager.CreateTasks(new PaymentDocumentEmailSetTaskProvider(this.Container), @params);
            return result.Success
                ? new JsonNetResult(new { sucess = true, message = "Задача успешно поставлена в очередь на выполнение" })
                : this.JsFailure(result.Message);
        }
    }
}