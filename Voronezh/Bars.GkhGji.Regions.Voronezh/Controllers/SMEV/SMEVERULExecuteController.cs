namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using System.Xml;
    using System.Xml.XPath;
    using System.Xml.Xsl;
    using System.Security.Cryptography.Xml;
    using System.Security.Cryptography.X509Certificates;
    using System.Globalization;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.GkhGji.Regions.Voronezh.Tasks.ERULSendInformationRequest;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class SMEVERULExecuteController : BaseController
    {
        public IDomainService<SMEVERULReqNumber> SMEVERULReqNumberDomain { get; set; }
        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public IDomainService<SMEVERULReqNumberFile> SMEVERULReqNumberFileDomain { get; set; }

        private IFileManager _fileManager;

        private readonly ITaskManager _taskManager;

        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;


        public SMEVERULExecuteController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVERULReqNumberDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            try
            {
                _taskManager.CreateTasks(new SendERULRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос номера лицензии из ЕРУЛ не удалось: " + e.Message);
            }
        }

        /// <summary>
        /// Отправить запрос в ЕРУЛ
        /// </summary>
        /// <param name="baseParams">ids 
        public ActionResult SendErulRequest(BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return JsFailure("Не найдена лицензия с идентификатором 0");
            }
           
            var license = ManOrgLicenseDomain.Get(docId);
            var existsRequest = SMEVERULReqNumberDomain.GetAll().FirstOrDefault(x => x.ManOrgLicense.Id == docId && x.RequestState != RequestState.Error);
            if (existsRequest != null)
            {
                return JsFailure("По данной лицензии запрос уже направлен.");
            }
            var newRequest = new SMEVERULReqNumber
            {
                CalcDate = DateTime.Now,
                ERULRequestType = BaseChelyabinsk.Enums.ERULRequestType.GetLicNumber,
                ManOrgLicense = license,
                RequestState = RequestState.NotFormed
            };
            SMEVERULReqNumberDomain.Save(newRequest);
            baseParams.Params.Add("taskId", newRequest.Id);
            try
            {
                _taskManager.CreateTasks(new SendERULRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, newRequest.Id);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос номера лицензии из ЕРУЛ не удалось: " + e.Message);
            }



        }

        /// <summary>
        /// Отправить запрос в ЕРУЛ
        /// </summary>
        /// <param name="baseParams">ids 
        public ActionResult SendErulUpdateRequest(BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return JsFailure("Не найдена лицензия с идентификатором 0");
            }

            var license = ManOrgLicenseDomain.Get(docId);
   
            var newRequest = new SMEVERULReqNumber
            {
                CalcDate = DateTime.Now,
                ERULRequestType = BaseChelyabinsk.Enums.ERULRequestType.AdditionalInfo,
                ManOrgLicense = license,
                RequestState = RequestState.NotFormed
            };
            SMEVERULReqNumberDomain.Save(newRequest);
            baseParams.Params.Add("taskId", newRequest.Id);
            try
            {
                _taskManager.CreateTasks(new SendERULRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, newRequest.Id);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос номера лицензии из ЕРУЛ не удалось: " + e.Message);
            }



        }

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVERULReqNumberDomain.Get(taskId);
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
    }
}
