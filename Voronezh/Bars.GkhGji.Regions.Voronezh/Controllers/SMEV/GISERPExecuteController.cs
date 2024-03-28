namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    ////// костыль 
    //using System.Xml;
    //using System.Xml.Linq;
    ////// костыль - end

    /// <summary>
    /// Контроллер для запросов в ГИС ГМП для получения платежей
    /// </summary>
    public class GISERPExecuteController : BaseController
    {
        #region Fields
        
        private IGISERPService _GISERPServicee;

        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        public IDomainService<GISERP> GISERPDomain { get; set; }

        public IDomainService<GISERPFile> GISERPFileDomain { get; set; }



        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        #endregion

        #region Constructors

        public GISERPExecuteController(IWindsorContainer container, IFileManager fileManager, IDomainService<FileInfo> fileDomain, IGISERPService GISERPService, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _GISERPServicee = GISERPService;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public ActionResult Export(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("GISERPDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }

        public ActionResult GetListDisposal(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IGISERPService>();
            try
            {
                return resolutionService.GetListDisposal(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }


        /// <summary>
        /// Отправить запрос на получение справочника прокуратур
        /// </summary>
        public ActionResult SendAskProsecOfficesRequest(BaseParams baseParams)
        {          

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendAskProsecOfficesRequestProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на запрос отделов прокурауры поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при запросе оплат: " + e.Message);
            }
        }

        /// <summary>
        /// Отправить начисление
        /// </summary>
        public ActionResult SendInitiateRequest(BaseParams baseParams, Int64 taskId)
        {
            GISERP smevRequestData = GISERPDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            if (smevRequestData.ERPInspectionType == ERPInspectionType.NotSet)
                return JsFailure("Не указан тип проверки (плановая/внеплановая)");

            if (smevRequestData.ProsecutorOffice == null)
                return JsFailure("Не указан отдел прокуратуры");

            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", taskId);

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendInitiateRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на отправку проверки поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при отправке проверки: " + e.Message);
            }
        }

        public ActionResult GetInspectionInfo(BaseParams baseParams)
        {
            var erpService = Container.Resolve<IGISERPService>();
            try
            {
                var data =  erpService.GetInspectionInfo(baseParams);
                return JsSuccess(data);
            }
            finally
            {

            }
        }


        #endregion

    }
}

