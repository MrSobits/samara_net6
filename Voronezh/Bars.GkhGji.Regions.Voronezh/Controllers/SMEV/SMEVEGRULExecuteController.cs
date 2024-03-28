namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.Tasks.EGRULSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVEGRULExecuteController : BaseController
    {
        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }
        private readonly ITaskManager _taskManager;
        private readonly ISmevPrintPdfHelper helper;

        public SMEVEGRULExecuteController(ITaskManager taskManager, ISmevPrintPdfHelper helper)
        {
            this._taskManager = taskManager;
            this.helper = helper;
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVEGRULDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new SendInformationRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из ЕГРЮЛ не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVEGRULDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            //if (!baseParams.Params.ContainsKey("taskId"))
            //    baseParams.Params.Add("taskId", taskId);

            try
            {
                _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                return JsSuccess("Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }

        public ActionResult PrintExtract(long id)
        {
            var extractDomain = this.Container.ResolveDomain<SMEVEGRUL>();
            
            var egrul = extractDomain.GetAll().FirstOrDefault(x => x.Id == id);
          
            Stream ms = new MemoryStream(this.helper.GetPdfExtract(egrul.XmlFile, "~/Resources/xslt/egrul.xsl" ));
            this.Response.Headers.Add("Content-Disposition", $@"attachment; filename={egrul.Name}.pdf");
            return new FileStreamResult(ms, "application/pdf");
        }
    }
}