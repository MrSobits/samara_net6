namespace Bars.GkhGji.Regions.Tyumen.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tyumen.DomainService;
    using Bars.GkhGji.Regions.Tyumen.Tasks.EGRNSendInformationRequest;
    using Bars.GkhGji.Regions.Tyumen.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using System;
    using Microsoft.AspNetCore.Mvc;

    public class SMEVEGRNExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        private readonly ITaskManager _taskManager;

        public SMEVEGRNExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
        }

        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }
        public IDomainService<SMEVEGRNFile> SMEVEGRNFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVEGRNDomain.Get(taskId);
            if (smevRequestData == null)
                return JsFailure("Запрос не сохранен");

            if (smevRequestData.RequestState == RequestState.Queued)
                return JsFailure("Запрос уже отправлен");

            //// !!!!!!!!!!!!!!!! костыль - вместо создания таска подкидывем нужный файл

            //XmlDocument doc = new XmlDocument();
            //doc.Load("C://Temp//ip.xml");
            //string xmlcontents = doc.InnerXml;
            //XmlNode node = doc.GetElementsByTagName("MessagePrimaryContent")[0];  //оснговной нод
            //XElement xmlOut = XElement.Parse(node.InnerXml.ToString());             //распарсили основной нод и поместили в XElement

            //_SMEVEGRIPService.ProcessResponseXML(smevRequestData, xmlOut.Element(ns1Namespace + "СвИП"));
            //// костыль - end

            try
            {
                _taskManager.CreateTasks(new SendEGRNRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из ЕГРН не удалось: " + e.Message);
            }
        }

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответов
            var smevRequestData = SMEVEGRNDomain.Get(taskId);
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

        public ActionResult GetListRoom(BaseParams baseParams)
        {
            var _SMEVEGRNService = Container.Resolve<ISMEVEGRNService>();
            try
            {
                return _SMEVEGRNService.GetListRoom(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }
    }
}
