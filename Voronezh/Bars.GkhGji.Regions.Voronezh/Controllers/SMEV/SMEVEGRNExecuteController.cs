namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.EGRNSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Entities;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.GkhGji.Entities;
    using System.Linq;

    public class SMEVEGRNExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        private readonly ITaskManager _taskManager;
        private ISMEVEGRNService _SMEVEGRNService;

        public SMEVEGRNExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager, ISMEVEGRNService EGRNService)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
            _taskManager = taskManager;
            _SMEVEGRNService = EGRNService;
        }

        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }
        public IDomainService<SMEVEGRNFile> SMEVEGRNFileDomain { get; set; }
        public IDomainService<ProtocolOSPRequest> ProtocolDomain { get; set; }

        /// <summary>
        /// Отправить запрос в ЕРУЛ
        /// </summary>
        /// <param name="baseParams">ids 
        public ActionResult SendEGRNRequest(BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return JsFailure("Не найдена заявка с идентификатором 0");
            }

            var existsRequest = SMEVEGRNDomain.GetAll().FirstOrDefault(x => x.ProtocolOSPRequestID == docId.ToString() && x.RequestState != RequestState.Error);
            if (existsRequest != null)
            {
                return JsFailure("По данной заявке запрос уже направлен.");
            }

            var CadastralNumber = ProtocolDomain.Get(docId).CadastralNumber;
            var email = ProtocolDomain.Get(docId).Email;
            if (CadastralNumber == null || CadastralNumber == "")
            {
                return JsFailure("Не указан кадастровый номер, направьте межведомственный запрос в Росреестр для получения выписки из Единого государственного реестра недвижимости вручную.");
            }

            baseParams.Params.Add("cadastr", CadastralNumber);
            var egrnData = _SMEVEGRNService.GetEGRNInfo(baseParams);

            SMEVEGRNDomain.Save(egrnData);
            baseParams.Params.Add("taskId", egrnData.Id.ToString());
            try
            {
                _taskManager.CreateTasks(new SendEGRNRequestTaskProvider(Container), baseParams);
                try
                {
                    EmailSender emailSender = EmailSender.Instance;
                    emailSender.Send(email, "Рассмотрение заявления", MakeMessageBody(), null);
                }
                catch { }

                return GetResponce(baseParams, egrnData.Id);
            }
            catch (Exception e)
            {
                return JsFailure("Создать задачу на запрос доступа к протоколам ОСС не удалось: " + e.Message);
            }
        }

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

        private string MakeMessageBody()
        {
            string body = $"Уважаемый(ая) заявитель!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что Ваше заявление находится на проверке\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }
    }
}
