namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Habarovsk.Tasks;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVChangePremisesStateExecuteController : BaseController
    {
        public IDomainService<SMEVChangePremisesState> SMEVChangePremisesStateDomain { get; set; }

        public IDomainService<SMEVChangePremisesStateFile> SMEVChangePremisesStateFileDomain { get; set; }

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<FileInfo> _fileDomain;

        public SMEVChangePremisesStateExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVChangePremisesStateDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new SendChangePremisesStateRequestTaskProvider(Container), baseParams);
                return GetResponse(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponse(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVChangePremisesStateDomain.Get(taskId);
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
