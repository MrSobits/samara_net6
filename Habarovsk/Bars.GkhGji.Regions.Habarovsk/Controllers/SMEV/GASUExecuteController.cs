namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Habarovsk.Tasks;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers;
    using Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;
    using System;
    using Microsoft.AspNetCore.Mvc;

    public class GASUExecuteController : BaseController
    {
        public IDomainService<GASU> GASUDomain { get; set; }

        public IDomainService<GASUFile> GASUFileDomain { get; set; }

        public IDomainService<GASUData> GASUDataDomain { get; set; }

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<FileInfo> _fileDomain;

        public GASUExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = GASUDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new GASUTaskProvider(Container), baseParams);
                return GetResponse(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponse(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = GASUDomain.Get(taskId);
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
