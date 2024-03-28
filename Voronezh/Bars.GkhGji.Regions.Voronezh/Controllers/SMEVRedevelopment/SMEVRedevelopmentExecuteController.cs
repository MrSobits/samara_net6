namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
    using Bars.GkhGji.Regions.Voronezh.Tasks;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Microsoft.AspNetCore.Mvc;

    public class SMEVRedevelopmentExecuteController : BaseController
    {
        public IDomainService<SMEVRedevelopment> SMEVRedevelopmentDomain { get; set; }

        public IDomainService<SMEVRedevelopmentFile> SMEVRedevelopmentFileDomain { get; set; }

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<FileInfo> _fileDomain;

        public SMEVRedevelopmentExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVRedevelopmentDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new RedevelopmentTaskProvider(Container), baseParams);
                return GetResponse(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponse(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVRedevelopmentDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            try
            {
                _taskManager.CreateTasks(new GetRPGUAnswersTaskProvider(Container), baseParams);
                return JsSuccess("Задача поставлена в очередь задач");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }
    }
}
