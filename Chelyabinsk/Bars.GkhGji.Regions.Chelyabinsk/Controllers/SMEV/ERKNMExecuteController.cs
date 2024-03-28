namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.ERKNMDictRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.SendPaymentRequest;
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
    public class ERKNMExecuteController : BaseController
    {
        #region Fields
        
        private IERKNMService _ERKNMServicee;

        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        public IDomainService<ERKNM> ERKNMDomain { get; set; }
        public IDomainService<Decision> DecisionDomain { get; set; }
        public IDomainService<AppealCitsAdmonition> AppealCitsAdmonitionDomain { get; set; }
        public IDomainService<ERKNMFile> ERKNMFileDomain { get; set; }



        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        #endregion

        #region Constructors

        public ERKNMExecuteController(IWindsorContainer container, IFileManager fileManager, IDomainService<FileInfo> fileDomain, IERKNMService GISERPService, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _ERKNMServicee = GISERPService;
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

        /// <summary>
        /// Отправить запрос в ЕРУЛ
        /// </summary>
        /// <param name="baseParams">ids 
        public ActionResult SendERKNMRequest(BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAs<long>("docId");
            var docType = baseParams.Params.GetAs<ERKNMDocumentType>("typeDoc");
            if (docId == 0)
            {
                return JsFailure("Не найден документ с идентификатором 0");
            }
            if (docType == 0)
            {
                return JsFailure("Не найден документ с типом 0");
            }
            if (docType == ERKNMDocumentType.Decision)
            {
                return SendDecision(baseParams, docId);
            }
            if (docType == ERKNMDocumentType.Admonition)
            {
                return SendAdmonition(baseParams, docId);
            }
            return JsFailure("Не удалось отправить КНМ");
        }

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = ERKNMDomain.Get(taskId);
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


        public ActionResult GetListDecision(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IERKNMService>();
            try
            {
                return resolutionService.GetListDecision(baseParams).ToJsonResult();
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
        /// Отправить запрос на получение справочника
        /// </summary>
        public ActionResult SendCompareDict(BaseParams baseParams)
        {
            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendERKNMDictRequestProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на запрос справочника поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при запросе справочника: " + e.Message);
            }
        }

        /// <summary>
        /// Отправить начисление
        /// </summary>
        public ActionResult SendInitiateRequest(BaseParams baseParams, Int64 taskId)
        {
            ERKNM smevRequestData = ERKNMDomain.Get(taskId);
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
                var taskInfo = _taskManager.CreateTasks(new ERKNMSendInitiateRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
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
            var erpService = Container.Resolve<IERKNMService>();
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
        private ActionResult SendDecision(BaseParams baseParams, long docId)
        {
            var decision = DecisionDomain.Get(docId);
            var existsRequest = ERKNMDomain.GetAll().FirstOrDefault(x => x.Disposal != null && x.Disposal.Id == docId && x.RequestState != RequestState.Error);
            if (existsRequest != null)
            {
                return JsFailure("По данному решению запрос уже направлен.");
            }
            baseParams.Params.Add("protocolData", docId);

            var erknmData = _ERKNMServicee.GetERKNMInfo(baseParams);
            erknmData.ERKNMDocumentType = ERKNMDocumentType.Decision;
            ERKNMDomain.Save(erknmData);
            baseParams.Params.Add("taskId", erknmData.Id.ToString());
            try
            {
                _taskManager.CreateTasks(new ERKNMSendInitiateRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, erknmData.Id);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос номера лицензии из ЕРУЛ не удалось: " + e.Message);
            }
        }
        private ActionResult SendAdmonition(BaseParams baseParams, long docId)
        {
            var admonition = AppealCitsAdmonitionDomain.Get(docId);
            var existsRequest = ERKNMDomain.GetAll().FirstOrDefault(x => x.AppealCitsAdmonition != null && x.AppealCitsAdmonition.Id == docId && x.RequestState != RequestState.Error);
            if (existsRequest != null)
            {
                return JsFailure("По данному предостережению запрос уже сформирован.");
            }
            baseParams.Params.Add("protocolData", docId);

            var erknmData = _ERKNMServicee.GetERKNMInfo(baseParams);
            ERKNMDomain.Save(erknmData);
            baseParams.Params.Add("taskId", erknmData.Id.ToString());
            try
            {
                _taskManager.CreateTasks(new ERKNMSendInitiateRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, erknmData.Id);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос номера лицензии из ЕРУЛ не удалось: " + e.Message);
            }
        }


    }
}

