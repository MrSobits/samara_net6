namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVNDFLExecuteController : BaseController
    {
        public IDomainService<SMEVNDFL> SMEVNDFLDomain { get; set; }

        public IDomainService<SMEVNDFLFile> SMEVNDFLFileDomain { get; set; }

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<FileInfo> _fileDomain;

        private ISMEVNDFLService _SMEVNDFLService;

        public SMEVNDFLExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager, ISMEVNDFLService sMEVNDFLService)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
            _SMEVNDFLService = sMEVNDFLService;
        }

        public ActionResult GetXML(BaseParams baseParams, long reqId)
        {
            try
            {
                string xml = _SMEVNDFLService.GetXML(reqId);
                return new JsonGetResult(new BaseDataResult(xml).Data);
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.Message);
            }
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVNDFLDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new SendNDFLRequestTaskProvider(Container), baseParams);
                return GetResponse(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponse(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVNDFLDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

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
    }
}
