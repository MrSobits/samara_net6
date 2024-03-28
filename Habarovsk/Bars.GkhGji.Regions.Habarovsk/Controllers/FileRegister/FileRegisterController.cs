namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Habarovsk.Entities;
    using Bars.GkhGji.Regions.Habarovsk.Tasks;
    using Castle.Windsor;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер 
    /// </summary>
    public class FileRegisterController : B4.Alt.DataController<FileRegister>
    {
        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        public IDomainService<FileRegister> FileRegisterDomain { get; set; }

        public FileRegisterController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        /// <summary>
        /// Проверить ответ на запрос
        /// </summary>
        public ActionResult CreateTask(BaseParams baseParams, Int64 roId)
        {
            FileRegister fileRegisterData = FileRegisterDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);
            if (fileRegisterData == null)
                return JsFailure("Перед формированием, требуется сохранить запись.");
            if (fileRegisterData.File != null)
                return JsFailure("Архив файлов для этого адреса уже сформирован.");

            try
            {
                var taskInfo = _taskManager.CreateTasks(new FileRegisterTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи не удалось: " + e.Message);
            }
        }
    }
}

