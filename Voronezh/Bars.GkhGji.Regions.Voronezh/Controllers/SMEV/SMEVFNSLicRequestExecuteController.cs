namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Castle.Windsor;
    using Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для запросов в ГИС ГМП
    /// </summary>
    public class SMEVFNSLicRequestExecuteController : BaseController
    {
        #region Fields

        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        public IDomainService<SMEVFNSLicRequest> SMEVFNSLicRequestDomain { get; set; }

        public IDomainService<SMEVFNSLicRequestFile> SMEVFNSLicRequestFileDomain { get; set; }

        #endregion

        #region Constructors

        public SMEVFNSLicRequestExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Проверить ответ на запрос
        /// </summary>
        public ActionResult GetResponse(BaseParams baseParams, Int64 taskId)
        {
            // все проверки ответа запускают таску на проверку всех ответов
            SMEVFNSLicRequest smevRequestData = SMEVFNSLicRequestDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.ResponseReceived)
                return JsFailure("Ответ уже получен");

            try
            {
                var taskInfo = _taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на проверку ответов в СМЭВ поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на проверку ответов не удалось: " + e.Message);
            }
        }

        /// <summary>
        /// Отправить начисление
        /// </summary>
        public ActionResult SendRequest(BaseParams baseParams, Int64 taskId)
        {
            SMEVFNSLicRequest smevRequestData = SMEVFNSLicRequestDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", taskId);

            try
            {
                var taskInfo = _taskManager.CreateTasks(new FNSSendRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на отправку начисления поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при отправке начисления: " + e.Message);
            }
        }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        #endregion

        public ActionResult GetLicenseInfo(BaseParams baseParams, Int64 licenseData)
        {
            var data = ManOrgLicenseDomain.GetAll()
                .Where(x => x.Id == licenseData)
                .Select(x => new
                {
                    INN = x.Contragent.Inn,
                    OGRN = x.Contragent.Ogrn,
                    NameUL = x.Contragent.Name,
                    Address = x.Contragent.JuridicalAddress,
                    x.DateRegister,
                    x.DateIssued,
                    x.DateTermination,
                    x.LicNumber,
                    NameLO = x.HousingInspection.Contragent.Name,
                    ShortNameLO = x.HousingInspection.Contragent.ShortName,
                    INNLO = x.HousingInspection.Contragent.Inn,
                    OGRNLO = x.HousingInspection.Contragent.Ogrn,
                    OKOGULO = x.HousingInspection.Contragent.Okogu,
                    PrAction = "Действует"
                }).FirstOrDefault();

            return JsSuccess(data);
        }
    }
}

