namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhCr.Entities;
    using Bars.GkhCR.Tasks;
    using Castle.Windsor;
    using System;
    using System.Linq;
    
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер 
    /// </summary>
    public class CrFileRegisterController : B4.Alt.DataController<CrFileRegister>
    {
        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        public IDomainService<CrFileRegister> FileRegisterDomain { get; set; }

        public CrFileRegisterController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
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
            CrFileRegister fileRegisterData = FileRegisterDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);
            if (fileRegisterData == null)
                return JsFailure("Перед формированием, требуется сохранить запись.");
            if (fileRegisterData.File != null)
                return JsFailure("Архив файлов для этого адреса уже сформирован.");

            try
            {
                var taskInfo = _taskManager.CreateTasks(new CrFileRegisterTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
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

