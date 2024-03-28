using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Bars.GkhGji.Tasks;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks;

namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    public class SSTUExportTaskExecuteController : BaseController
    {
        /// <summary>
        /// Менеджер задач
        /// </summary>
        private ITaskManager _taskManager;
        private IWindsorContainer _container;
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public SSTUExportTaskExecuteController(IWindsorContainer container, IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _container = container;
            _taskManager = taskManager;
        }

        //   public IDomainService<SSTUExportTask> SSTUExportTaskDomain { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<SSTUExportTaskAppeal> SSTUExportTaskAppealDomain { get; set; }

        public IDomainService<SSTUExportTask> SSTUExportTaskDomain { get; set; }

        public IDomainService<SSTUExportedAppeal> SSTUExportedAppealDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            SSTUExportTask SSTURequestData = SSTUExportTaskDomain.Get(taskId);
            if (SSTURequestData == null)
                return JsFailure("Запрос не сохранен");

            if (SSTURequestData.SSTUExportState == SSTUExportState.Exported)
                return JsFailure("Запрос уже отправлен");

            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", taskId);

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SSTUExportTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на отправку  поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при отправке: " + e.Message);
            }

        }
    }
}
