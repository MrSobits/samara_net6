namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Habarovsk.DomainService;
    using Bars.GkhGji.Regions.Habarovsk.Tasks;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.Habarovsk.Tasks.SendPaymentRequest;
    using Castle.Windsor;
    using Entities;
    using Enums;
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    ////// костыль 
    //using System.Xml;
    //using System.Xml.Linq;
    ////// костыль - end

    /// <summary>
    /// Контроллер для запросов в ГИС ГМП для получения платежей
    /// </summary>
    public class PAYREGExecuteController : BaseController
    {
        #region Fields
        
        private IPAYREGService _PAYREGService;

        private IFileManager _fileManager;

        private IDomainService<FileInfo> _fileDomain;

        ////// костыль 
        //static XNamespace ns17Namespace = @"urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.0.1";
        ////private IPAYREGService _PAYREGService;
        //public PAYREGExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager, IPAYREGService PAYREGService)
        //{
        //    _fileManager = fileManager;
        //    _fileDomain = fileDomain;
        //    _taskManager = taskManager;
        //    _PAYREGService = PAYREGService;
        //}
        ////// костыль - end

        /// <summary>
        /// Менеджер задач
        /// </summary>
        private readonly ITaskManager _taskManager;

        private readonly IWindsorContainer _container;

        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }

        public IDomainService<PayReg> PayRegDomain { get; set; }

        public IDomainService<PayRegFile> PayRegFileDomain { get; set; }

        #endregion

        #region Constructors

        public PAYREGExecuteController(IWindsorContainer container, IFileManager fileManager, IDomainService<FileInfo> fileDomain, IPAYREGService PAYREGService, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _PAYREGService = PAYREGService;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods
             

        /// <summary>
        /// Отправить запрос об оплатах
        /// </summary>
        public ActionResult SendPaymentRequest(BaseParams baseParams, DateTime paymentsStartDate, DateTime paymentsEndDate)
        {
            PayRegRequests smevRequestData = new PayRegRequests();
            smevRequestData.GetPaymentsStartDate = paymentsStartDate;
            smevRequestData.GetPaymentsEndDate = paymentsEndDate;
            smevRequestData.Answer = "";
            smevRequestData.MessageId = "";
            smevRequestData.PayRegPaymentsType = Enums.GisGmpPaymentsType.AllInTime;
            PayRegRequestsDomain.Save(smevRequestData);

            //if (!baseParams.Params.ContainsKey("paymentsStartDate"))
            //    baseParams.Params.Add("paymentsStartDate", paymentsStartDate);

            //if (!baseParams.Params.ContainsKey("paymentsEndDate"))
            //    baseParams.Params.Add("paymentsEndDate", paymentsEndDate);

            //// !!!!!!!!!!!!!!!! костыль - вместо создания таска подкидывем нужный файл

            //XmlDocument doc = new XmlDocument();
            //doc.Load("C://Temp//opl.xml");
            //string xmlcontents = doc.InnerXml;
            //XmlNode node = doc.GetElementsByTagName("MessagePrimaryContent")[0];  //оснговной нод
            //XElement xmlOut = XElement.Parse(node.InnerXml.ToString());             //распарсили основной нод и поместили в XElement
            //XElement eryer = xmlOut.Element(ns17Namespace + "ExportPaymentsResponse");
            //_PAYREGService.TryProcessResponse(null, xmlOut);
            //return JsFailure("Сбой создания задачи");
            //// костыль - end
            if (!baseParams.Params.ContainsKey("taskId"))
                baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

            try
            {
                var taskInfo = _taskManager.CreateTasks(new SendPaymentRequestTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                if (taskInfo == null)
                    return JsFailure("Сбой создания задачи");
                else
                    return JsSuccess($"Задача на запрос оплат поставлена в очередь с id {taskInfo.TaskId}");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при запросе оплат: " + e.Message);
            }
        }
              
        /// <summary>
        /// Проверить ответ на запрос
        /// </summary>
        public ActionResult CheckAnswer(BaseParams baseParams, long taskId)
        {
            // все проверки ответа запускают таску на проверку всех ответов
            PayRegRequests smevRequestData = PayRegRequestsDomain.Get(taskId);
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
               
        public ActionResult GetPaymentsListByGisGmpId(BaseParams baseParams)
        {
            var paymentsService = Container.Resolve<IPAYREGService>();
            try
            {
                return paymentsService.ListPayments(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }
               
        public ActionResult GetPaymentsListForPayFine(BaseParams baseParams)
        {
            var paymentsService = Container.Resolve<IPAYREGService>();
            try
            {
                return paymentsService.ListPaymentsForPayFine(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }
               
        public ActionResult AddPayFine(BaseParams baseParams, long resolutionId, long payRegId)
        {
             var paymentsService = Container.Resolve<IPAYREGService>();
            try
            {
                var result = paymentsService.AddPayFine(baseParams, resolutionId, payRegId);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult FindGisGmp(BaseParams baseParams, string id, string UIN, string purpose)
        {
            var paymentsService = Container.Resolve<IPAYREGService>();
            try
            {
                //JsonResult result = new JsonResult();
                var gisGmp = paymentsService.FindGisGmp(UIN, purpose);
                if (gisGmp != null)
                {
                    var payReg = PayRegDomain.Get(long.Parse(id));
                    payReg.GisGmp = gisGmp;
                    PayRegDomain.Update(payReg);
                    //result.Data = gisGmp.Id;
                    return JsSuccess("Начисление найдено");
                }
                else return JsSuccess("Начисление не найдено");
            }
            catch (Exception eeeeee)
            {
                return JsFailure("Ошибка поиска начисления");
            }
        }

        #endregion

    }
}

