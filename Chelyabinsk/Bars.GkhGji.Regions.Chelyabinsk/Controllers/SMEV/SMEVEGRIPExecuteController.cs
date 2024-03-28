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

    public class SMEVEGRIPExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public SMEVEGRIPExecuteController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }

        public IDomainService<SMEVEGRIPFile> SMEVEGRIPFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
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

            if (smevRequestData.RequestState == RequestState.NotFormed)
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

                XmlDocument requestForSign = XmlOperations.CreateXmlDocReqForSign(egrulElement, messageId, false);
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
                                                                SMEVEGRIPDomain.Update(smevRequestData);
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

            return JsSuccess();
        }

    }
}
