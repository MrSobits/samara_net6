namespace Bars.GkhGji.Regions.Voronezh.Controllers
{

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.GkhGji.Regions.Voronezh.Tasks.EGRIPSendInformationRequest; //нужен
   // using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRULSendInformationRequest;
    using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
    using Bars.GkhGji.Regions.Voronezh.Tasks.SendSMEVPropertyType;
    using Entities;
    using Enums;
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    //// костыль 
    //using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
    //using System.Xml;
    //using System.Xml.Linq;
    //// костыль - end


    public class SMEVEGRIPExecuteController : BaseController
    {
        private readonly ITaskManager _taskManager;
        private readonly ISmevPrintPdfHelper helper;

        //// костыль 
        //static XNamespace ns1Namespace = @"urn://x-artefacts-fns-vipip-tosmv-ru/311-15/4.0.5";
        //private ISMEVEGRIPService _SMEVEGRIPService;
        //public SMEVEGRIPExecuteController(IFileManager fileManager, IDomainService<FileInfo> fileDomain, ITaskManager taskManager, ISMEVEGRIPService SMEVEGRIPService)
        //{
        //    _fileManager = fileManager;
        //    _fileDomain = fileDomain;
        //    _taskManager = taskManager;
        //    _SMEVEGRIPService = SMEVEGRIPService;
        //}
        //// костыль - end

        public SMEVEGRIPExecuteController(ITaskManager taskManager, ISmevPrintPdfHelper helper)
        {
            _taskManager = taskManager;
            this.helper = helper;
        }

        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }
        public IDomainService<SMEVPropertyType> SMEVPropertyTypeDomain { get; set; }

        public IDomainService<SMEVValidPassport> SMEVValidPassportDomain { get; set; }
        public IDomainService<SMEVStayingPlace> SMEVStayingPlaceDomain { get; set; }
        public IDomainService<SMEVLivingPlace> SMEVLivingPlaceDomain { get; set; }
        public IDomainService<SMEVEGRIPFile> SMEVEGRIPFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVEGRIPDomain.Get(taskId);
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
                _taskManager.CreateTasks(new SendEGRIPRequestTaskProvider(Container), baseParams);
                return GetResponce(baseParams, taskId);
            }
            catch (Exception e)
            {
                return JsFailure("Создание задачи на запрос данных из ЕГРИП не удалось: " + e.Message);
            }
        }

        public ActionResult PropTypeExecute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVPropertyTypeDomain.Get(taskId);
            //if (smevRequestData == null)
            //    return JsFailure("Запрос не сохранен");

            //if (smevRequestData.RequestState == RequestState.Queued)
            //    return JsFailure("Запрос уже отправлен");

            var propertyTypeService = Container.Resolve<ISMEVPropertyTypeService>();
            propertyTypeService.SendInformationRequest(smevRequestData, null);
            return null;
            //try
            //{
            //    _taskManager.CreateTasks(new SendSMEVPropertyTypeTaskProvider(Container), baseParams);
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    return JsFailure("Создание задачи на запрос данных из ГосИмущества не удалось: " + e.Message);
            //}
        }

        public ActionResult StayingPlaceExecute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVStayingPlaceDomain.Get(taskId);
            //if (smevRequestData == null)
            //    return JsFailure("Запрос не сохранен");

            //if (smevRequestData.RequestState == RequestState.Queued)
            //    return JsFailure("Запрос уже отправлен");

            var stayingPlaceService = Container.Resolve<ISMEVStayingPlaceService>();
            stayingPlaceService.SendInformationRequest(smevRequestData, null);
            return null;
            //try
            //{
            //    _taskManager.CreateTasks(new SendSMEVStayingPlaceTaskProvider(Container), baseParams);
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    return JsFailure("Создание задачи на запрос данных из ГосИмущества не удалось: " + e.Message);
            //}
        }

        public ActionResult LivingPlaceExecute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVLivingPlaceDomain.Get(taskId);
            //if (smevRequestData == null)
            //    return JsFailure("Запрос не сохранен");

            //if (smevRequestData.RequestState == RequestState.Queued)
            //    return JsFailure("Запрос уже отправлен");

            var livingPlaceService = Container.Resolve<ISMEVLivingPlaceService>();
            livingPlaceService.SendInformationRequest(smevRequestData, null);
            return null;
            //try
            //{
            //    _taskManager.CreateTasks(new SendSMEVLivingPlaceTaskProvider(Container), baseParams);
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    return JsFailure("Создание задачи на запрос данных из ГосИмущества не удалось: " + e.Message);
            //}
        }

        public ActionResult ValidPassportExecute(BaseParams baseParams, Int64 taskId)
        {
            var smevRequestData = SMEVValidPassportDomain.Get(taskId);
            //if (smevRequestData == null)
            //    return JsFailure("Запрос не сохранен");

            //if (smevRequestData.RequestState == RequestState.Queued)
            //    return JsFailure("Запрос уже отправлен");

            var propertyTypeService = Container.Resolve<ISMEVValidPassportService>();
            propertyTypeService.SendInformationRequest(smevRequestData, null);
            return null;
            //try
            //{
            //    _taskManager.CreateTasks(new SendSMEVPropertyTypeTaskProvider(Container), baseParams);
            //    return null;
            //}
            //catch (Exception e)
            //{
            //    return JsFailure("Создание задачи на запрос данных из ГосИмущества не удалось: " + e.Message);
            //}
        }

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            //Из-за нехватки времени все проверки ответа запускают таску на проверку всех ответоп
            var smevRequestData = SMEVEGRIPDomain.Get(taskId);
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
        public ActionResult PrintExtract(long id)
        {
            var extractDomain = this.Container.ResolveDomain<SMEVEGRIP>();
            var egrip = extractDomain.GetAll().FirstOrDefault(x => x.Id == id);
            
            Stream ms = new MemoryStream(this.helper.GetPdfExtract(egrip.XmlFile, "~/Resources/xslt/egrip.xsl" ));
            this.Response.Headers.Add("Content-Disposition", $@"attachment; filename={egrip.FIO}.pdf");
            return new FileStreamResult(ms, "application/pdf");
        }

        #region старый метод, переводим на таски
        /*
        public ActionResult ExecuteOld(BaseParams baseParams, Int64 taskId)
        {

            var smevRequestData = SMEVEGRIPDomain.Get(taskId);
            var files = SMEVEGRIPFileDomain.GetAll()
                .Where(x => x.SMEVEGRIP.Id == taskId).ToList();
            foreach (SMEVEGRIPFile fileRec in files)
            {
                SMEVEGRIPFileDomain.Delete(fileRec.Id);
            }

            MemoryStream streamReq = new MemoryStream();
            StreamWriter xwReq = new StreamWriter(streamReq, new UTF8Encoding(false));

            MemoryStream streamResp = new MemoryStream();
            StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));

            MemoryStream streamAck = new MemoryStream();
            StreamWriter xwAck = new StreamWriter(streamAck, new UTF8Encoding(false));

            if (smevRequestData.RequestState == Enums.RequestState.NotFormed)
            {
                //Формируем класс с данными
                StringBuilder sb = new StringBuilder();
                DateTime dateTime = DateTime.Now;
                String messageId = GuidGenerator.GenerateTimeBasedGuid(dateTime).ToString(); ;
                String inn = smevRequestData.INNReq;
                sb.Append($"<ns1:FNSVipIPRequest xmlns:ns1=\"urn://x-artefacts-fns-vipip-tosmv-ru/311-15/4.0.5\" ИдДок=\"{messageId}\" НомерДела=\"БН\">");
                sb.Append("<ns1:ЗапросИП>");
                if (smevRequestData.InnOgrn == InnOgrn.INN)
                    sb.Append($"<ns1:ИНН>{inn}</ns1:ИНН>");
                else
                    sb.Append($"<ns1:ОГРНИП>{inn}</ns1:ОГРНИП>");
                sb.Append("</ns1:ЗапросИП>");
                sb.Append("</ns1:FNSVipIPRequest>");

                //Получаем сертификат
                X509Certificate2 cert = SmevSign.GetCertificateFromStore();
                Stream streamElementSend = new MemoryStream();

                ////Сериализуем класс с данными в XML
                //XmlSerializer serializerMvdSendRequest = new XmlSerializer(typeof(MvdSendRequest));
                //XmlTextWriter xmltw = new XmlTextWriter(streamElementSend, new UTF8Encoding(false));
                //serializerMvdSendRequest.Serialize(xmltw, mvdSendRequest);
                //streamElementSend.Position = 0;

                XmlDocument egrulDocument = new XmlDocument();
                egrulDocument.LoadXml(sb.ToString());
                XmlElement egrulElement = egrulDocument.GetElementsByTagName("ns1:FNSVipIPRequest").Cast<XmlElement>().FirstOrDefault();

                //Формируем xml для подписи

                XmlDocument requestForSign = XmlOperations.CreateXmlDocReqForSign(egrulElement, messageId);
                smevRequestData.MessageId = messageId;

                //Получаем элемент с подписью
                var sign = SmevSign.Sign(requestForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                //Формируем Xml для отправки
                XmlDocument egrulRequestRequest = XmlOperations.CreateSendRequest(requestForSign, sign);

                //Пишем xml в MemoryStream
                xwReq.WriteLine(egrulRequestRequest.InnerXml);
                xwReq.Flush();
                streamReq.Position = 0;
                //SMEVMVDFileDomain
                SMEVEGRIPFile newRec = new SMEVEGRIPFile();
                newRec.ObjectCreateDate = DateTime.Now;
                newRec.ObjectEditDate = DateTime.Now;
                newRec.ObjectVersion = 1;
                newRec.SMEVEGRIP = smevRequestData;
                newRec.SMEVFileType = SMEVFileType.SendRequestSig;
                newRec.FileInfo = _fileManager.SaveFile(streamReq, "SendRequestRequest.xml");
                SMEVEGRIPFileDomain.Save(newRec);
                smevRequestData.RequestState = RequestState.Formed;
                SMEVEGRIPDomain.Update(smevRequestData);

                //Отправляем xml  в сМЭВ   
                Boolean isError;
                HttpWebResponse response = SmevWebRequest.SendRequest("urn:SendRequest", egrulRequestRequest, out isError);
                if (isError)
                {
                    //Распарсиваем ответ
                    smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                    //   добавить  smevRequestData.RequestState = RequestState.ResponseReceived;
                    SMEVEGRIPDomain.Update(smevRequestData);
                }
                else
                {
                    MemoryStream streamSendRequestResponse = new MemoryStream();
                    StreamWriter xwSendRequestResponse = new StreamWriter(streamSendRequestResponse, new UTF8Encoding(false));

                    //Распарсиваем ответ
                    XmlDocument sendRequestResponseXml = SmevWebRequest.GetResponseXML(response);
                    //Пишем xml в MemoryStream
                    xwSendRequestResponse.WriteLine(sendRequestResponseXml.InnerXml);
                    xwSendRequestResponse.Flush();
                    streamSendRequestResponse.Position = 0;

                    newRec = new SMEVEGRIPFile();
                    newRec.ObjectCreateDate = DateTime.Now;
                    newRec.ObjectEditDate = DateTime.Now;
                    newRec.ObjectVersion = 1;
                    newRec.SMEVEGRIP = smevRequestData;
                    newRec.SMEVFileType = SMEVFileType.SendRequestSigAnswer;
                    newRec.FileInfo = _fileManager.SaveFile(streamSendRequestResponse, "SendRequestResponse.xml");
                    SMEVEGRIPFileDomain.Save(newRec);


                    var errorList = sendRequestResponseXml.GetElementsByTagName("faultstring");
                    if (errorList.Count > 0)
                    {
                        //Распарсиваем ответ
                        smevRequestData.Answer = errorList[0].InnerText;
                        SMEVEGRIPDomain.Update(smevRequestData);
                    }
                    else
                    {
                        var statusList = sendRequestResponseXml.GetElementsByTagName("ns2:Status");
                        if (statusList.Count > 0)
                        {


                            string statusReq = statusList[0].InnerText;
                            if (statusReq == "requestIsQueued")
                            {
                                smevRequestData.RequestState = RequestState.Queued;
                                SMEVEGRIPDomain.Update(smevRequestData);
                                //RESPONSE
                                //Создаем элемент для подписания
                                XmlDocument responseForSign = XmlOperations.CreateXmlDocRespForSign("FNSVipIPRequest", "urn://x-artefacts-fns-vipip-tosmv-ru/311-15/4.0.5");

                                //Получаем подпись
                                sign = SmevSign.Sign(responseForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                //Создаем документ для отправки
                                XmlDocument mvdResponse = XmlOperations.CreateGetResponse(responseForSign, sign);
                                xwResp.WriteLine(mvdResponse.InnerXml);
                                xwResp.Flush();
                                streamResp.Position = 0;

                                //SMEVMVDFileDomain
                                newRec = new SMEVEGRIPFile();
                                newRec.ObjectCreateDate = DateTime.Now;
                                newRec.ObjectEditDate = DateTime.Now;
                                newRec.ObjectVersion = 1;
                                newRec.SMEVEGRIP = smevRequestData;
                                newRec.SMEVFileType = SMEVFileType.SendResponceSig;
                                newRec.FileInfo = _fileManager.SaveFile(streamResp, "GetResponse.xml");


                                SMEVEGRIPFileDomain.Save(newRec);


                                string respMessageId = String.Empty;
                                Thread.Sleep(10000);
                                while (respMessageId != messageId)
                                {
                                    //Отправляем xml  в сМЭВ  


                                    response = SmevWebRequest.SendRequest("urn:GetResponse", mvdResponse, out isError);
                                    if (isError)
                                    {
                                        //Распарсиваем ответ
                                        smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                                        smevRequestData.MessageId = "";
                                        smevRequestData.RequestState = RequestState.ResponseReceived;
                                        SMEVEGRIPDomain.Update(smevRequestData);
                                    }
                                    else
                                    {
                                        MemoryStream streamGetRequestResponse = new MemoryStream();
                                        StreamWriter xwGetRequestResponse = new StreamWriter(streamGetRequestResponse, new UTF8Encoding(false));

                                        //Распарсиваем ответ
                                        XmlDocument getRequestResponseXml = SmevWebRequest.GetResponseXML(response);

                                        //Пишем xml в MemoryStream
                                        xwGetRequestResponse.WriteLine(getRequestResponseXml.InnerXml);
                                        xwGetRequestResponse.Flush();
                                        streamGetRequestResponse.Position = 0;



                                        var originalMessageIdList = getRequestResponseXml.GetElementsByTagName("ns2:OriginalMessageId");
                                        if (originalMessageIdList.Count > 0)
                                        {
                                            respMessageId = originalMessageIdList[0].InnerText;
                                            if (respMessageId == messageId)
                                            {
                                                newRec = new SMEVEGRIPFile();
                                                newRec.ObjectCreateDate = DateTime.Now;
                                                newRec.ObjectEditDate = DateTime.Now;
                                                newRec.ObjectVersion = 1;
                                                newRec.SMEVEGRIP = smevRequestData;
                                                newRec.SMEVFileType = SMEVFileType.SendResponceSigAnswer;
                                                newRec.FileInfo = _fileManager.SaveFile(streamGetRequestResponse, "GetRequestResponse.xml");
                                                SMEVEGRIPFileDomain.Save(newRec);



                                                SMEVEGRIPDomain.Update(smevRequestData);
                                                var senderProvidedResponseDataList = getRequestResponseXml.GetElementsByTagName("ns2:SenderProvidedResponseData");
                                                if (senderProvidedResponseDataList.Count > 0)
                                                {
                                                    var messageIdList = getRequestResponseXml.GetElementsByTagName("ns2:MessageID");
                                                    if (messageIdList.Count > 0)
                                                    {
                                                        string ackMessageId = messageIdList[0].InnerText;

                                                        var responseList = getRequestResponseXml.GetElementsByTagName("ns1:FNSVipIPResponse");
                                                        if (responseList.Count > 0)
                                                        {
                                                            string responseRecords = responseList[0].InnerXml;
                                                            if (responseRecords.Contains("КодОбр"))
                                                            {
                                                                string resultCode = getRequestResponseXml.GetElementsByTagName("ns1:КодОбр")[0].InnerText;
                                                                if (resultCode == "53")
                                                                    smevRequestData.Answer = "сведения в отношении индивидуального предпринимателя не могут быть предоставлены в электронном виде";
                                                                else
                                                                    smevRequestData.Answer = "запрашиваемые сведения не найдены";

                                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                SMEVEGRIPDomain.Update(smevRequestData);
                                                            }
                                                            else
                                                            {
                                                                /////////////////////////////////////////////////////////////
                                                                XmlDocument parseDoc = getRequestResponseXml;


                                                                var elementNode = parseDoc.GetElementsByTagName("ns1:СвИП").Count > 0
                                                                                    ? parseDoc.GetElementsByTagName("ns1:СвИП")[0]
                                                                                        : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.IPType = elementNode.Attributes["НаимВидИП"].Value;
                                                                    smevRequestData.OGRN = elementNode.Attributes["ОГРНИП"].Value;
                                                                    smevRequestData.OGRNDate = null;
                                                                    DateTime dt;
                                                                    if (DateTime.TryParse(elementNode.Attributes["ДатаОГРНИП"].Value, out dt))
                                                                    {
                                                                        smevRequestData.OGRNDate = dt;
                                                                    }

                                                                    smevRequestData.ResponceDate = null;
                                                                    if (DateTime.TryParse(elementNode.Attributes["ДатаВып"].Value, out dt))
                                                                    {
                                                                        smevRequestData.ResponceDate = dt;
                                                                    }
                                                                }

                                                                var elementFIO = parseDoc.GetElementsByTagName("ns1:ФИОРус").Count > 0
                                                                               ? parseDoc.GetElementsByTagName("ns1:ФИОРус")[0]
                                                                                   : null;
                                                                if (elementFIO != null)
                                                                {
                                                                    smevRequestData.FIO = elementFIO.Attributes["Фамилия"].Value + " " +
                                                                                elementFIO.Attributes["Имя"].Value + " " +
                                                                                    elementFIO.Attributes["Отчество"].Value;
                                                                }


                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвГражд").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СвГражд")[0]
                                                                                    : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.Citizenship = elementNode.Attributes["НаимСтран"].Value;
                                                                }


                                                                var elementAdress = parseDoc.GetElementsByTagName("fnst:Регион").Count > 0
                                                                                        ? parseDoc.GetElementsByTagName("fnst:Регион")[0]
                                                                                            : null;
                                                                if (elementAdress != null)
                                                                {
                                                                    smevRequestData.RegionName = elementAdress.Attributes["НаимРегион"].Value;
                                                                    smevRequestData.RegionType = elementAdress.Attributes["ТипРегион"].Value;
                                                                }

                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвРегОрг").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СвРегОрг")[0]
                                                                                    : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.RegOrgName = elementNode.Attributes["НаимНО"].Value;
                                                                    smevRequestData.AddressRegOrg = elementNode.Attributes["АдрРО"].Value;
                                                                    smevRequestData.CodeRegOrg = elementNode.Attributes["КодНО"].Value;
                                                                }

                                                                elementNode = parseDoc.GetElementsByTagName("ns1:ВидЗап").Count > 0
                                                                               ? parseDoc.GetElementsByTagName("ns1:ВидЗап")[0]
                                                                                   : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.CreateWayName = elementNode.Attributes["НаимВидЗап"].Value;
                                                                }

                                                                var elementOKVED = parseDoc.GetElementsByTagName("ns1:СвОКВЭД").Count > 0
                                                                                        ? parseDoc.GetElementsByTagName("ns1:СвОКВЭД")[0]
                                                                                            : null;

                                                                if (elementOKVED != null)
                                                                {
                                                                    foreach (XmlNode childNode in elementOKVED.ChildNodes)
                                                                    {
                                                                        smevRequestData.OKVEDNames += childNode.Attributes["НаимОКВЭД"].Value + "; ";
                                                                        smevRequestData.OKVEDCodes += childNode.Attributes["КодОКВЭД"].Value + "; ";
                                                                    }
                                                                }


                                                                ////////////////////////////////////////////////////////////////


                                                                smevRequestData.Answer = "Данные о индивидуальном предпринимателе лице получены";
                                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                try
                                                                {
                                                                    SMEVEGRIPDomain.Update(smevRequestData);
                                                                }
                                                                catch (Exception ex)
                                                                {


                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            smevRequestData.Answer = "В базе ЕГРЮЛ отсутствуют записи";
                                                            smevRequestData.RequestState = RequestState.ResponseReceived;

                                                            SMEVEGRIPDomain.Update(smevRequestData);
                                                        }
                                                        //ACK
                                                        //Создаем элемент для подписания
                                                        XmlDocument ackForSign = XmlOperations.CreateXmlDocAckForSign(ackMessageId);

                                                        //Получаем подпись
                                                        sign = SmevSign.Sign(ackForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                                        //Создаем документ для отправки
                                                        XmlDocument mvdAck = XmlOperations.CreateAck(ackForSign, sign);
                                                        xwAck.WriteLine(mvdAck.InnerXml);
                                                        xwAck.Flush();
                                                        streamAck.Position = 0;

                                                        //SMEVMVDFileDomain
                                                        newRec = new SMEVEGRIPFile();
                                                        newRec.ObjectCreateDate = DateTime.Now;
                                                        newRec.ObjectEditDate = DateTime.Now;
                                                        newRec.ObjectVersion = 1;
                                                        newRec.SMEVEGRIP = smevRequestData;
                                                        newRec.SMEVFileType = SMEVFileType.AckRequestSig;
                                                        newRec.FileInfo = _fileManager.SaveFile(streamAck, "Ack.xml");

                                                        SMEVEGRIPFileDomain.Save(newRec);

                                                        response = SmevWebRequest.SendRequest("urn:Ack", mvdAck, out isError);
                                                        if (isError)
                                                        {

                                                            smevRequestData.Answer += " Внимание! Запрос не удален из списка запросов СМЭВ, уведомите техподдержку";
                                                            smevRequestData.MessageId = "";
                                                            SMEVEGRIPDomain.Update(smevRequestData);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Если нет мессаджИД, не знаю такое возможно или нет
                                                    }
                                                }
                                                else
                                                {
                                                    smevRequestData.Answer = "В полученном ответе отсутствуют сведения, либо ответ не соответствует требуемой схеме";
                                                    smevRequestData.MessageId = "";
                                                    smevRequestData.RequestState = RequestState.ResponseReceived;
                                                    SMEVEGRIPDomain.Update(smevRequestData);
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {
                                            smevRequestData.Answer = "Опрос очереди запросов СМЭВ закончен, ответ на данный запрос не найден.";
                                            smevRequestData.MessageId = "";
                                            smevRequestData.RequestState = RequestState.ResponseReceived;
                                            SMEVEGRIPDomain.Update(smevRequestData);
                                            break;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                smevRequestData.Answer = "Запрос отклонен";
                                smevRequestData.MessageId = "";
                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                SMEVEGRIPDomain.Update(smevRequestData);
                            }
                        }
                        else
                        {
                            smevRequestData.Answer = "Статус обращения неизвестен";
                            smevRequestData.MessageId = "";
                            smevRequestData.RequestState = RequestState.ResponseReceived;
                            SMEVEGRIPDomain.Update(smevRequestData);
                        }
                    }
                }
            }
            else
            {

            }



            return JsSuccess();
        }
        */
        #endregion
    }
}
