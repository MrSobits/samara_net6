namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using System;
    using System.IO;
    using System.Linq;
    using Enums;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4.Modules.States;
    using System.Net;
    using System.Xml;
    using System.Xml.Serialization;
    using SmevRef;
    using Entities.SMEV;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVEGRULExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public SMEVEGRULExecuteController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }

        public IDomainService<SMEVEGRULFile> SMEVEGRULFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {
            StreamWriter swErr = new StreamWriter($"C:\\FileStore\\log{taskId}.txt");
            try
            {
                swErr.WriteLine("go");
                swErr.WriteLine(taskId.ToString());
          
            var smevRequestData = SMEVEGRULDomain.Get(taskId);
                if (smevRequestData.RequestState != RequestState.NotFormed)
                {
                    var files = SMEVEGRULFileDomain.GetAll()
                        .Where(x => x.SMEVEGRUL.Id == taskId).ToList();
                    foreach (SMEVEGRULFile fileRec in files)
                    {
                        SMEVEGRULFileDomain.Delete(fileRec.Id);
                    }
                }

            MemoryStream streamReq = new MemoryStream();
            StreamWriter xwReq = new StreamWriter(streamReq, new UTF8Encoding(false));

            MemoryStream streamResp = new MemoryStream();
            StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));

            MemoryStream streamAck = new MemoryStream();
            StreamWriter xwAck = new StreamWriter(streamAck, new UTF8Encoding(false));

            if (smevRequestData.RequestState == RequestState.NotFormed)
            {
                    swErr.WriteLine("not formed");
                    //Формируем класс с данными
                    StringBuilder sb = new StringBuilder();
                DateTime dateTime = DateTime.Now;
                String messageId = GuidGenerator.GenerateTimeBasedGuid(dateTime).ToString(); ;
                String inn = smevRequestData.INNReq;
                sb.Append($"<ns1:FNSVipULRequest xmlns:ns1=\"urn://x-artefacts-fns-vipul-tosmv-ru/311-14/4.0.5\" ИдДок=\"{messageId}\" НомерДела=\"БН\">");
                sb.Append("<ns1:ЗапросЮЛ>");
                if (smevRequestData.InnOgrn == InnOgrn.INN)
                    sb.Append($"<ns1:ИННЮЛ>{inn}</ns1:ИННЮЛ>");
                else
                    sb.Append($"<ns1:ОГРН>{inn}</ns1:ОГРН>");
                sb.Append("</ns1:ЗапросЮЛ>");
                sb.Append("</ns1:FNSVipULRequest>");
                    swErr.WriteLine("msg " + messageId + ", INN " + inn);
                    //Получаем сертификат
                    //Главное управление "ГЖИ Челябинской области" X509Certificate2 cert = SmevSign.GetCertificateFromStore("Государственная жилищная инспекция Самарской области");
                    X509Certificate2 cert = SmevSign.GetCertificateFromStore();
                Stream streamElementSend = new MemoryStream();
                    swErr.WriteLine("cert " + cert);
                    ////Сериализуем класс с данными в XML
                    //XmlSerializer serializerMvdSendRequest = new XmlSerializer(typeof(MvdSendRequest));
                    //XmlTextWriter xmltw = new XmlTextWriter(streamElementSend, new UTF8Encoding(false));
                    //serializerMvdSendRequest.Serialize(xmltw, mvdSendRequest);
                    //streamElementSend.Position = 0;

                    XmlDocument egrulDocument = new XmlDocument();
                egrulDocument.LoadXml(sb.ToString());
                XmlElement egrulElement = egrulDocument.GetElementsByTagName("ns1:FNSVipULRequest").Cast<XmlElement>().FirstOrDefault();

                //Формируем xml для подписи

                XmlDocument requestForSign = XmlOperations.CreateXmlDocReqForSign(egrulElement, messageId, false);
                smevRequestData.MessageId = messageId;

                //Получаем элемент с подписью
                var sign = SmevSign.Sign(requestForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                //Формируем Xml для отправки
                XmlDocument egrulRequestRequest = XmlOperations.CreateSendRequest(requestForSign, sign);
                    swErr.WriteLine("egrulRequestRequest " + egrulRequestRequest.InnerXml);
                    //Пишем xml в MemoryStream
                    xwReq.WriteLine(egrulRequestRequest.InnerXml);
                xwReq.Flush();
                streamReq.Position = 0;
                //SMEVMVDFileDomain
                SMEVEGRULFile newRec = new SMEVEGRULFile();
                newRec.ObjectCreateDate = DateTime.Now;
                newRec.ObjectEditDate = DateTime.Now;
                newRec.ObjectVersion = 1;
                newRec.SMEVEGRUL = smevRequestData;
                newRec.SMEVFileType = SMEVFileType.SendRequestSig;
                newRec.FileInfo = _fileManager.SaveFile(streamReq, "SendRequestRequest.xml");
                SMEVEGRULFileDomain.Save(newRec);
                smevRequestData.RequestState = RequestState.Formed;
                SMEVEGRULDomain.Update(smevRequestData);

                //Отправляем xml  в сМЭВ   
                Boolean isError;
                HttpWebResponse response = SmevWebRequest.SendRequest("urn:SendRequest", egrulRequestRequest, out isError);
                if (isError)
                {
                    //Распарсиваем ответ
                    smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                    //   добавить  smevRequestData.RequestState = RequestState.ResponseReceived;
                    SMEVEGRULDomain.Update(smevRequestData);
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

                    newRec = new SMEVEGRULFile();
                    newRec.ObjectCreateDate = DateTime.Now;
                    newRec.ObjectEditDate = DateTime.Now;
                    newRec.ObjectVersion = 1;
                    newRec.SMEVEGRUL = smevRequestData;
                    newRec.SMEVFileType = SMEVFileType.SendRequestSigAnswer;
                    newRec.FileInfo = _fileManager.SaveFile(streamSendRequestResponse, "SendRequestResponse.xml");
                    SMEVEGRULFileDomain.Save(newRec);


                    var errorList = sendRequestResponseXml.GetElementsByTagName("faultstring");
                    if (errorList.Count > 0)
                    {
                        //Распарсиваем ответ
                        smevRequestData.Answer = errorList[0].InnerText;
                        SMEVEGRULDomain.Update(smevRequestData);
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
                                SMEVEGRULDomain.Update(smevRequestData);
                                //RESPONSE
                                //Создаем элемент для подписания
                                XmlDocument responseForSign = XmlOperations.CreateXmlDocRespForSign("FNSVipULRequest", "urn://x-artefacts-fns-vipul-tosmv-ru/311-14/4.0.5");

                                //Получаем подпись
                                sign = SmevSign.Sign(responseForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                //Создаем документ для отправки
                                XmlDocument mvdResponse = XmlOperations.CreateGetResponse(responseForSign, sign);
                                xwResp.WriteLine(mvdResponse.InnerXml);
                                xwResp.Flush();
                                streamResp.Position = 0;

                                //SMEVMVDFileDomain
                                newRec = new SMEVEGRULFile();
                                newRec.ObjectCreateDate = DateTime.Now;
                                newRec.ObjectEditDate = DateTime.Now;
                                newRec.ObjectVersion = 1;
                                newRec.SMEVEGRUL = smevRequestData;
                                newRec.SMEVFileType = SMEVFileType.SendResponceSig;
                                newRec.FileInfo = _fileManager.SaveFile(streamResp, "GetResponse.xml");


                                SMEVEGRULFileDomain.Save(newRec);


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
                                     
                                        smevRequestData.RequestState = RequestState.ResponseReceived;
                                        SMEVEGRULDomain.Update(smevRequestData);
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
                                                newRec = new SMEVEGRULFile();
                                                newRec.ObjectCreateDate = DateTime.Now;
                                                newRec.ObjectEditDate = DateTime.Now;
                                                newRec.ObjectVersion = 1;
                                                newRec.SMEVEGRUL = smevRequestData;
                                                newRec.SMEVFileType = SMEVFileType.SendResponceSigAnswer;
                                                newRec.FileInfo = _fileManager.SaveFile(streamGetRequestResponse, "GetRequestResponse.xml");
                                                SMEVEGRULFileDomain.Save(newRec);



                                                SMEVEGRULDomain.Update(smevRequestData);
                                                var senderProvidedResponseDataList = getRequestResponseXml.GetElementsByTagName("ns2:SenderProvidedResponseData");
                                                if (senderProvidedResponseDataList.Count > 0)
                                                {
                                                    var messageIdList = getRequestResponseXml.GetElementsByTagName("ns2:MessageID");
                                                    if (messageIdList.Count > 0)
                                                    {
                                                        string ackMessageId = messageIdList[0].InnerText;

                                                        var responseList = getRequestResponseXml.GetElementsByTagName("ns1:FNSVipULResponse");
                                                        if (responseList.Count > 0)
                                                        {
                                                            string responseRecords = responseList[0].InnerXml;
                                                            if (responseRecords.Contains("КодОбр"))
                                                            {
                                                                string resultCode = getRequestResponseXml.GetElementsByTagName("ns1:КодОбр")[0].InnerText;
                                                                if (resultCode == "53")
                                                                    smevRequestData.Answer = "сведения в отношении юридического лица не могут быть предоставлены в электронном виде";
                                                                else
                                                                    smevRequestData.Answer = "запрашиваемые сведения не найдены";

                                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                SMEVEGRULDomain.Update(smevRequestData);
                                                            }
                                                            else
                                                            {
                                                                /////////////////////////////////////////////////////////////
                                                                XmlDocument parseDoc = getRequestResponseXml;


                                                                var elementNode = parseDoc.GetElementsByTagName("ns1:СвЮЛ").Count > 0
                                                                                    ? parseDoc.GetElementsByTagName("ns1:СвЮЛ")[0]
                                                                                        : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.OPFName = elementNode.Attributes["ПолнНаимОПФ"].Value;
                                                                    smevRequestData.INN = elementNode.Attributes["ИНН"].Value;
                                                                    smevRequestData.KPP = elementNode.Attributes["КПП"].Value;
                                                                    smevRequestData.OGRN = elementNode.Attributes["ОГРН"].Value;
                                                                    smevRequestData.OGRNDate = null;
                                                                    DateTime dt;
                                                                    if (DateTime.TryParse(elementNode.Attributes["ДатаОГРН"].Value, out dt))
                                                                    {
                                                                        smevRequestData.OGRNDate = dt;
                                                                    }

                                                                    smevRequestData.ResponceDate = null;
                                                                    if (DateTime.TryParse(elementNode.Attributes["ДатаВып"].Value, out dt))
                                                                    {
                                                                        smevRequestData.ResponceDate = dt;
                                                                    }
                                                                }


                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвНаимЮЛ").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СвНаимЮЛ")[0]
                                                                                    : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.Name = elementNode.Attributes["НаимЮЛПолн"].Value;
                                                                    smevRequestData.ShortName = elementNode.Attributes["НаимЮЛСокр"].Value;
                                                                }


                                                                var elementAdress = parseDoc.GetElementsByTagName("ns1:АдресРФ").Count > 0
                                                                                        ? parseDoc.GetElementsByTagName("ns1:АдресРФ")[0]
                                                                                            : null;
                                                                if (elementAdress != null)
                                                                {
                                                                    foreach (XmlNode childNode in elementAdress.ChildNodes)
                                                                    {
                                                                        foreach (XmlAttribute attr in childNode.Attributes)
                                                                        {
                                                                            smevRequestData.AddressUL += attr.Value + " ";
                                                                        }
                                                                    }
                                                                    smevRequestData.AddressUL += ", д." + elementAdress.Attributes["Дом"].Value;
                                                                    smevRequestData.AddressUL += ", корп." + elementAdress.Attributes["Корпус"].Value;
                                                                }

                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СпОбрЮЛ").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СпОбрЮЛ")[0]
                                                                                    : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.CreateWayName = elementNode.Attributes["НаимСпОбрЮЛ"].Value;
                                                                }


                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвРегОрг").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СвРегОрг")[0]
                                                                                    : null;
                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.RegOrgName = elementNode.Attributes["НаимНО"].Value;
                                                                    smevRequestData.AddressRegOrg = elementNode.Attributes["АдрРО"].Value;
                                                                    smevRequestData.CodeRegOrg = elementNode.Attributes["КодНО"].Value;
                                                                    smevRequestData.StateNameUL = "Действует";
                                                                }



                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвУстКап").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СвУстКап")[0]
                                                                                    : null;
                                                                if (elementNode != null)
                                                                {
                                                                    Decimal d = 0;
                                                                    string str546465 = elementNode.Attributes["СумКап"].Value;
                                                                    Decimal.TryParse(elementNode.Attributes["СумКап"].Value, out d);
                                                                    Decimal.TryParse(elementNode.Attributes["СумКап"].Value.Replace('.', ','), out d);
                                                                    smevRequestData.AuthorizedCapitalAmmount = d;
                                                                    smevRequestData.AuthorizedCapitalType = elementNode.Attributes["НаимВидКап"].Value;
                                                                }


                                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвДолжн").Count > 0
                                                                                ? parseDoc.GetElementsByTagName("ns1:СвДолжн")[0]
                                                                                    : null;

                                                                if (elementNode != null)
                                                                {
                                                                    smevRequestData.TypePozitionName = elementNode.Attributes["НаимВидДолжн"].Value;
                                                                    smevRequestData.Pozition = elementNode.Attributes["НаимДолжн"].Value;
                                                                }


                                                                var elementFIO = parseDoc.GetElementsByTagName("ns1:СвФЛ").Count > 0
                                                                                    ? parseDoc.GetElementsByTagName("ns1:СвФЛ")[0]
                                                                                        : null;
                                                                if (elementFIO != null)
                                                                {
                                                                    smevRequestData.FIO = elementFIO.Attributes["Фамилия"].Value + " " +
                                                                                elementFIO.Attributes["Имя"].Value + " " +
                                                                                    elementFIO.Attributes["Отчество"].Value;
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


                                                                smevRequestData.Answer = "Данные о юридическом лице получены";
                                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                SMEVEGRULDomain.Update(smevRequestData);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            smevRequestData.Answer = "В базе МВД отсутствуют записи";
                                                            smevRequestData.RequestState = RequestState.ResponseReceived;

                                                            SMEVEGRULDomain.Update(smevRequestData);
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
                                                        newRec = new SMEVEGRULFile();
                                                        newRec.ObjectCreateDate = DateTime.Now;
                                                        newRec.ObjectEditDate = DateTime.Now;
                                                        newRec.ObjectVersion = 1;
                                                        newRec.SMEVEGRUL = smevRequestData;
                                                        newRec.SMEVFileType = SMEVFileType.AckRequestSig;
                                                        newRec.FileInfo = _fileManager.SaveFile(streamAck, "Ack.xml");

                                                        SMEVEGRULFileDomain.Save(newRec);

                                                        response = SmevWebRequest.SendRequest("urn:Ack", mvdAck, out isError);
                                                        if (isError)
                                                        {

                                                            smevRequestData.Answer += " Внимание! Запрос не удален из списка запросов СМЭВ, уведомите техподдержку";
                                                           
                                                            SMEVEGRULDomain.Update(smevRequestData);

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
                                                  
                                                    smevRequestData.RequestState = RequestState.ResponseReceived;
                                                    SMEVEGRULDomain.Update(smevRequestData);
                                                }
                                            }
                                            else
                                            {

                                            }
                                        }
                                        else
                                        {
                                            smevRequestData.Answer = "Опрос очереди запросов СМЭВ закончен, ответ на данный запрос не найден.";
                                      
                                            smevRequestData.RequestState = RequestState.Queued;
                                            SMEVEGRULDomain.Update(smevRequestData);
                                            break;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                smevRequestData.Answer = "Запрос отклонен";
                              
                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                SMEVEGRULDomain.Update(smevRequestData);
                            }
                        }
                        else
                        {
                            smevRequestData.Answer = "Статус обращения неизвестен";
                           
                            smevRequestData.RequestState = RequestState.ResponseReceived;
                            SMEVEGRULDomain.Update(smevRequestData);
                        }
                    }
                }
            }
            else
            {

            }


                swErr.Close();
                return JsSuccess();
            }
            catch (Exception exc)
            {
                swErr.WriteLine(exc.Message);
                swErr.Close();
                return JsSuccess();
            }
        }

        public ActionResult GetResponce(BaseParams baseParams, Int64 taskId)
        {
            StreamWriter swErr = new StreamWriter($"C:\\FileStore\\logResponce{taskId}.txt");
            try
            {
                swErr.WriteLine("go");
                swErr.WriteLine(taskId.ToString());

                var files = SMEVEGRULFileDomain.GetAll()
                      .Where(x => x.SMEVEGRUL.Id == taskId && x.FileInfo.Name == "GetResponse").ToList();
                foreach (SMEVEGRULFile fileRec in files)
                {
                    SMEVEGRULFileDomain.Delete(fileRec.Id);
                }

                var smevRequestData = SMEVEGRULDomain.Get(taskId);
                //RESPONSE
                //Создаем элемент для подписания
                XmlDocument responseForSign = XmlOperations.CreateXmlDocRespForSign("FNSVipULRequest", "urn://x-artefacts-fns-vipul-tosmv-ru/311-14/4.0.5");

                //Получаем подпись
                X509Certificate2 cert = SmevSign.GetCertificateFromStore();
                var sign = SmevSign.Sign(responseForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                //Создаем документ для отправки
                MemoryStream streamResp = new MemoryStream();
                StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));
                XmlDocument mvdResponse = XmlOperations.CreateGetResponse(responseForSign, sign);
                xwResp.WriteLine(mvdResponse.InnerXml);
                xwResp.Flush();
                streamResp.Position = 0;

                //SMEVMVDFileDomain
                SMEVEGRULFile newRec = new SMEVEGRULFile();
                newRec.ObjectCreateDate = DateTime.Now;
                newRec.ObjectEditDate = DateTime.Now;
                newRec.ObjectVersion = 1;
                newRec.SMEVEGRUL = smevRequestData;
                newRec.SMEVFileType = SMEVFileType.SendResponceSig;
                newRec.FileInfo = _fileManager.SaveFile(streamResp, "GetResponse.xml");


                SMEVEGRULFileDomain.Save(newRec);


                string respMessageId = String.Empty;
              
                while (respMessageId != smevRequestData.MessageId)
                {
                    //Отправляем xml  в сМЭВ  
                    Boolean isError;

                    HttpWebResponse response = SmevWebRequest.SendRequest("urn:GetResponse", mvdResponse, out isError);
                    if (isError)
                    {
                        //Распарсиваем ответ
                        smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                       
                        smevRequestData.RequestState = RequestState.ResponseReceived;
                        SMEVEGRULDomain.Update(smevRequestData);
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
                            if (respMessageId == smevRequestData.MessageId)
                            {
                                newRec = new SMEVEGRULFile();
                                newRec.ObjectCreateDate = DateTime.Now;
                                newRec.ObjectEditDate = DateTime.Now;
                                newRec.ObjectVersion = 1;
                                newRec.SMEVEGRUL = smevRequestData;
                                newRec.SMEVFileType = SMEVFileType.SendResponceSigAnswer;
                                newRec.FileInfo = _fileManager.SaveFile(streamGetRequestResponse, "GetRequestResponse.xml");
                                SMEVEGRULFileDomain.Save(newRec);



                                SMEVEGRULDomain.Update(smevRequestData);
                                var senderProvidedResponseDataList = getRequestResponseXml.GetElementsByTagName("ns2:SenderProvidedResponseData");
                                if (senderProvidedResponseDataList.Count > 0)
                                {
                                    var messageIdList = getRequestResponseXml.GetElementsByTagName("ns2:MessageID");
                                    if (messageIdList.Count > 0)
                                    {
                                        string ackMessageId = messageIdList[0].InnerText;

                                        var responseList = getRequestResponseXml.GetElementsByTagName("ns1:FNSVipULResponse");
                                        if (responseList.Count > 0)
                                        {
                                            string responseRecords = responseList[0].InnerXml;
                                            if (responseRecords.Contains("КодОбр"))
                                            {
                                                string resultCode = getRequestResponseXml.GetElementsByTagName("ns1:КодОбр")[0].InnerText;
                                                if (resultCode == "53")
                                                    smevRequestData.Answer = "сведения в отношении юридического лица не могут быть предоставлены в электронном виде";
                                                else
                                                    smevRequestData.Answer = "запрашиваемые сведения не найдены";

                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                SMEVEGRULDomain.Update(smevRequestData);
                                            }
                                            else
                                            {
                                                /////////////////////////////////////////////////////////////
                                                XmlDocument parseDoc = getRequestResponseXml;


                                                var elementNode = parseDoc.GetElementsByTagName("ns1:СвЮЛ").Count > 0
                                                                    ? parseDoc.GetElementsByTagName("ns1:СвЮЛ")[0]
                                                                        : null;
                                                if (elementNode != null)
                                                {
                                                    smevRequestData.OPFName = elementNode.Attributes["ПолнНаимОПФ"].Value;
                                                    smevRequestData.INN = elementNode.Attributes["ИНН"].Value;
                                                    smevRequestData.KPP = elementNode.Attributes["КПП"].Value;
                                                    smevRequestData.OGRN = elementNode.Attributes["ОГРН"].Value;
                                                    smevRequestData.OGRNDate = null;
                                                    DateTime dt;
                                                    if (DateTime.TryParse(elementNode.Attributes["ДатаОГРН"].Value, out dt))
                                                    {
                                                        smevRequestData.OGRNDate = dt;
                                                    }

                                                    smevRequestData.ResponceDate = null;
                                                    if (DateTime.TryParse(elementNode.Attributes["ДатаВып"].Value, out dt))
                                                    {
                                                        smevRequestData.ResponceDate = dt;
                                                    }
                                                }


                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвНаимЮЛ").Count > 0
                                                                ? parseDoc.GetElementsByTagName("ns1:СвНаимЮЛ")[0]
                                                                    : null;
                                                if (elementNode != null)
                                                {
                                                    smevRequestData.Name = elementNode.Attributes["НаимЮЛПолн"].Value;
                                                    smevRequestData.ShortName = elementNode.Attributes["НаимЮЛСокр"].Value;
                                                }


                                                var elementAdress = parseDoc.GetElementsByTagName("ns1:АдресРФ").Count > 0
                                                                        ? parseDoc.GetElementsByTagName("ns1:АдресРФ")[0]
                                                                            : null;
                                                if (elementAdress != null)
                                                {
                                                    foreach (XmlNode childNode in elementAdress.ChildNodes)
                                                    {
                                                        foreach (XmlAttribute attr in childNode.Attributes)
                                                        {
                                                            smevRequestData.AddressUL += attr.Value + " ";
                                                        }
                                                    }
                                                    smevRequestData.AddressUL += ", д." + elementAdress.Attributes["Дом"].Value;
                                                    smevRequestData.AddressUL += ", корп." + elementAdress.Attributes["Корпус"].Value;
                                                }

                                                elementNode = parseDoc.GetElementsByTagName("ns1:СпОбрЮЛ").Count > 0
                                                                ? parseDoc.GetElementsByTagName("ns1:СпОбрЮЛ")[0]
                                                                    : null;
                                                if (elementNode != null)
                                                {
                                                    smevRequestData.CreateWayName = elementNode.Attributes["НаимСпОбрЮЛ"].Value;
                                                }


                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвРегОрг").Count > 0
                                                                ? parseDoc.GetElementsByTagName("ns1:СвРегОрг")[0]
                                                                    : null;
                                                if (elementNode != null)
                                                {
                                                    smevRequestData.RegOrgName = elementNode.Attributes["НаимНО"].Value;
                                                    smevRequestData.AddressRegOrg = elementNode.Attributes["АдрРО"].Value;
                                                    smevRequestData.CodeRegOrg = elementNode.Attributes["КодНО"].Value;
                                                    smevRequestData.StateNameUL = "Действует";
                                                }



                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвУстКап").Count > 0
                                                                ? parseDoc.GetElementsByTagName("ns1:СвУстКап")[0]
                                                                    : null;
                                                if (elementNode != null)
                                                {
                                                    Decimal d = 0;
                                                    string str546465 = elementNode.Attributes["СумКап"].Value;
                                                    Decimal.TryParse(elementNode.Attributes["СумКап"].Value, out d);
                                                    Decimal.TryParse(elementNode.Attributes["СумКап"].Value.Replace('.', ','), out d);
                                                    smevRequestData.AuthorizedCapitalAmmount = d;
                                                    smevRequestData.AuthorizedCapitalType = elementNode.Attributes["НаимВидКап"].Value;
                                                }


                                                elementNode = parseDoc.GetElementsByTagName("ns1:СвДолжн").Count > 0
                                                                ? parseDoc.GetElementsByTagName("ns1:СвДолжн")[0]
                                                                    : null;

                                                if (elementNode != null)
                                                {
                                                    smevRequestData.TypePozitionName = elementNode.Attributes["НаимВидДолжн"].Value;
                                                    smevRequestData.Pozition = elementNode.Attributes["НаимДолжн"].Value;
                                                }


                                                var elementFIO = parseDoc.GetElementsByTagName("ns1:СвФЛ").Count > 0
                                                                    ? parseDoc.GetElementsByTagName("ns1:СвФЛ")[0]
                                                                        : null;
                                                if (elementFIO != null)
                                                {
                                                    smevRequestData.FIO = elementFIO.Attributes["Фамилия"].Value + " " +
                                                                elementFIO.Attributes["Имя"].Value + " " +
                                                                    elementFIO.Attributes["Отчество"].Value;
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


                                                smevRequestData.Answer = "Данные о юридическом лице получены";
                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                SMEVEGRULDomain.Update(smevRequestData);
                                            }
                                        }
                                        else
                                        {
                                            smevRequestData.Answer = "В базе МВД отсутствуют записи";
                                            smevRequestData.RequestState = RequestState.ResponseReceived;

                                            SMEVEGRULDomain.Update(smevRequestData);
                                        }
                                        //ACK
                                        //Создаем элемент для подписания
                                        XmlDocument ackForSign = XmlOperations.CreateXmlDocAckForSign(ackMessageId);

                                        //Получаем подпись
                                        sign = SmevSign.Sign(ackForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                        //Создаем документ для отправки
                                        XmlDocument mvdAck = XmlOperations.CreateAck(ackForSign, sign);
                                        MemoryStream streamAck = new MemoryStream();
                                        StreamWriter xwAck = new StreamWriter(streamAck, new UTF8Encoding(false));
                                        xwAck.WriteLine(mvdAck.InnerXml);
                                        xwAck.Flush();
                                        streamAck.Position = 0;

                                        //SMEVMVDFileDomain
                                        newRec = new SMEVEGRULFile();
                                        newRec.ObjectCreateDate = DateTime.Now;
                                        newRec.ObjectEditDate = DateTime.Now;
                                        newRec.ObjectVersion = 1;
                                        newRec.SMEVEGRUL = smevRequestData;
                                        newRec.SMEVFileType = SMEVFileType.AckRequestSig;
                                        newRec.FileInfo = _fileManager.SaveFile(streamAck, "Ack.xml");

                                        SMEVEGRULFileDomain.Save(newRec);

                                        response = SmevWebRequest.SendRequest("urn:Ack", mvdAck, out isError);
                                        if (isError)
                                        {

                                            smevRequestData.Answer += " Внимание! Запрос не удален из списка запросов СМЭВ, уведомите техподдержку";
                                         
                                            SMEVEGRULDomain.Update(smevRequestData);

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
                                  
                                    smevRequestData.RequestState = RequestState.ResponseReceived;
                                    SMEVEGRULDomain.Update(smevRequestData);
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            smevRequestData.Answer = "Опрос очереди запросов СМЭВ закончен, ответ на данный запрос не найден.";
                            
                            smevRequestData.RequestState = RequestState.ResponseReceived;
                            SMEVEGRULDomain.Update(smevRequestData);
                            break;
                        }

                    }
                }
                swErr.Close();
                return JsSuccess();
            }
            catch (Exception exc)
            {
                swErr.WriteLine(exc.Message);
                swErr.Close();
                return JsSuccess();
            }
            
        }
       

    }
}
