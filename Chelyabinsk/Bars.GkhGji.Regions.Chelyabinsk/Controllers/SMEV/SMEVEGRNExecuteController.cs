namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Entities;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Xml;
    using Entities.SMEV;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVEGRNExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public SMEVEGRNExecuteController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }

        public IDomainService<SMEVEGRNFile> SMEVEGRNFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {

            var smevRequestData = SMEVEGRNDomain.Get(taskId);
            var files = SMEVEGRNFileDomain.GetAll()
                .Where(x => x.SMEVEGRN.Id == taskId).ToList();
            foreach (SMEVEGRNFile fileRec in files)
            {
                SMEVEGRNFileDomain.Delete(fileRec.Id);
            }

            MemoryStream streamReq = new MemoryStream();
            StreamWriter xwReq = new StreamWriter(streamReq, new UTF8Encoding(false));

            MemoryStream streamResp = new MemoryStream();
            StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));

            MemoryStream streamAck = new MemoryStream();
            StreamWriter xwAck = new StreamWriter(streamAck, new UTF8Encoding(false));

            //Dictionary<MVDTypeAddress, string> typeAddrCodeDict = new Dictionary<MVDTypeAddress, string>();
            //typeAddrCodeDict.Add(MVDTypeAddress.BirthPlace, "000");
            //typeAddrCodeDict.Add(MVDTypeAddress.LivingPlace, "200");
            //typeAddrCodeDict.Add(MVDTypeAddress.FactPlace, "201");
            //typeAddrCodeDict.Add(MVDTypeAddress.Other, "202");
            if (smevRequestData.RequestState == RequestState.NotFormed)
            {
                //Создаем хмл
                Guid fileName = Guid.NewGuid();
                DateTime dateTime = DateTime.Now;
                String messageId = GuidGenerator.GenerateTimeBasedGuid(dateTime).ToString();
                //XmlDocument elementXML = new XmlDocument();
                StringBuilder sb = new StringBuilder();
                sb.Append("<req:Request xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:req=\"urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/1.1.1\" xmlns:das=\"urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/dRegionsRF/1.0.0\">");
                sb.Append($"<req:region>{smevRequestData.RegionCode.Code.Substring(1)}</req:region>");
                sb.Append($"<req:externalNumber>{smevRequestData.Id}</req:externalNumber>");
                sb.Append("<req:senderType>Vedomstvo</req:senderType>");
                sb.Append("<req:actionCode>659511111112</req:actionCode>");
                sb.Append("<req:Attachment>");
                sb.Append("<req:IsMTOMAttachmentContent>true</req:IsMTOMAttachmentContent>");
                sb.Append("<req:RequestDescription>");
                sb.Append("<req:IsUnstructuredFormat>false</req:IsUnstructuredFormat>");
                sb.Append("<req:IsZippedPacket>true</req:IsZippedPacket>");
                sb.Append("<req:fileName>request.xml</req:fileName>");
                sb.Append("</req:RequestDescription>");
                sb.Append("<req:Statement>");
                sb.Append("<req:IsUnstructuredFormat>false</req:IsUnstructuredFormat>");
                sb.Append("<req:IsZippedPacket>true</req:IsZippedPacket>");
                sb.Append($"<req:fileName>{fileName.ToString()}.xml</req:fileName>");
                //sb1.Append($"<req:fileName>40e5d060-e9e9-4d8d-b5be-bce49d6dfbdb.xml</req:fileName>");
                sb.Append("</req:Statement>");
                sb.Append("</req:Attachment>");
                sb.Append("</req:Request>");
                //elementXML.LoadXml(sb.ToString());


                //Получаем сертификат
                X509Certificate2 cert = SmevSign.GetCertificateFromStore();
                Stream streamElementSend = new MemoryStream();

                ////Сериализуем класс с данными в XML
                //XmlSerializer serializerMvdSendRequest = new XmlSerializer(typeof(MvdSendRequest));
                //XmlTextWriter xmltw = new XmlTextWriter(streamElementSend, new UTF8Encoding(false));
                //serializerMvdSendRequest.Serialize(xmltw, mvdSendRequest);
                //streamElementSend.Position = 0;

                XmlDocument egrnDocument = new XmlDocument();
                egrnDocument.LoadXml(sb.ToString());
                XmlElement egrnElement = egrnDocument.GetElementsByTagName("req:Request").Cast<XmlElement>().FirstOrDefault();

                //Формируем xml для подписи
                XmlDocument requestForSign = XmlOperations.CreateXmlDocReqForSign(egrnElement, messageId, true);
                smevRequestData.MessageId = messageId;

                //Получаем элемент с подписью
                var sign = SmevSign.Sign(requestForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });


                XmlOperations.CreateAttachments(smevRequestData, fileName.ToString());

                //Формируем Xml для отправки
                XmlDocument egrnRequestRequest = XmlOperations.CreateSendRequest(requestForSign, sign, true);



                //Пишем xml в MemoryStream
                xwReq.WriteLine(egrnRequestRequest.InnerXml);
                xwReq.Flush();
                streamReq.Position = 0;
                //SMEVMVDFileDomain
                SMEVEGRNFile newRec = new SMEVEGRNFile();
                newRec.ObjectCreateDate = DateTime.Now;
                newRec.ObjectEditDate = DateTime.Now;
                newRec.ObjectVersion = 1;
                newRec.SMEVEGRN = smevRequestData;
                newRec.SMEVFileType = SMEVFileType.SendRequestSig;
                newRec.FileInfo = _fileManager.SaveFile(streamReq, "SendRequestRequest.xml");
                SMEVEGRNFileDomain.Save(newRec);
                smevRequestData.RequestState = RequestState.Formed;
                SMEVEGRNDomain.Update(smevRequestData);

                //Отправляем xml  в сМЭВ   
                Boolean isError;
                HttpWebResponse response = SmevWebRequest.SendRequest("urn:SendRequest", egrnRequestRequest, out isError);
                if (isError)
                {
                    //Распарсиваем ответ
                    smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                    //   добавить  smevRequestData.RequestState = RequestState.ResponseReceived;
                    SMEVEGRNDomain.Update(smevRequestData);
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

                    newRec = new SMEVEGRNFile();
                    newRec.ObjectCreateDate = DateTime.Now;
                    newRec.ObjectEditDate = DateTime.Now;
                    newRec.ObjectVersion = 1;
                    newRec.SMEVEGRN = smevRequestData;
                    newRec.SMEVFileType = SMEVFileType.SendRequestSigAnswer;
                    newRec.FileInfo = _fileManager.SaveFile(streamSendRequestResponse, "SendRequestResponse.xml");
                    SMEVEGRNFileDomain.Save(newRec);


                    var errorList = sendRequestResponseXml.GetElementsByTagName("faultstring");
                    if (errorList.Count > 0)
                    {
                        //Распарсиваем ответ
                        smevRequestData.Answer = errorList[0].InnerText;
                        SMEVEGRNDomain.Update(smevRequestData);
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
                                SMEVEGRNDomain.Update(smevRequestData);
                                //RESPONSE
                                //Создаем элемент для подписания
                                XmlDocument responseForSign = XmlOperations.CreateXmlDocRespForSign("Request", "urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/1.1.1");

                                //Получаем подпись
                                sign = SmevSign.Sign(responseForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                //Создаем документ для отправки
                                XmlDocument egrnResponse = XmlOperations.CreateGetResponse(responseForSign, sign);
                                xwResp.WriteLine(egrnResponse.InnerXml);
                                xwResp.Flush();
                                streamResp.Position = 0;

                                //SMEVMVDFileDomain
                                newRec = new SMEVEGRNFile();
                                newRec.ObjectCreateDate = DateTime.Now;
                                newRec.ObjectEditDate = DateTime.Now;
                                newRec.ObjectVersion = 1;
                                newRec.SMEVEGRN = smevRequestData;
                                newRec.SMEVFileType = SMEVFileType.SendResponceSig;
                                newRec.FileInfo = _fileManager.SaveFile(streamResp, "GetResponse.xml");


                                SMEVEGRNFileDomain.Save(newRec);


                                string respMessageId = String.Empty;
                                Thread.Sleep(10000);
                                while (respMessageId != messageId)
                                {
                                    //Отправляем xml  в сМЭВ  


                                    response = SmevWebRequest.SendRequest("urn:GetResponse", egrnResponse, out isError);
                                    if (isError)
                                    {
                                        //Распарсиваем ответ
                                        smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                                        smevRequestData.RequestState = RequestState.ResponseReceived;
                                        SMEVEGRNDomain.Update(smevRequestData);
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
                                                newRec = new SMEVEGRNFile();
                                                newRec.ObjectCreateDate = DateTime.Now;
                                                newRec.ObjectEditDate = DateTime.Now;
                                                newRec.ObjectVersion = 1;
                                                newRec.SMEVEGRN = smevRequestData;
                                                newRec.SMEVFileType = SMEVFileType.SendResponceSigAnswer;
                                                newRec.FileInfo = _fileManager.SaveFile(streamGetRequestResponse, "GetRequestResponse.xml");
                                                SMEVEGRNFileDomain.Save(newRec);

                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                SMEVEGRNDomain.Update(smevRequestData);
                                                var senderProvidedResponseDataList = getRequestResponseXml.GetElementsByTagName("ns2:SenderProvidedResponseData");
                                                if (senderProvidedResponseDataList.Count > 0)
                                                {
                                                    var messageIdList = getRequestResponseXml.GetElementsByTagName("ns2:MessageID");
                                                    if (messageIdList.Count > 0)
                                                    {
                                                        string ackMessageId = messageIdList[0].InnerText;
                                                        var responseList = getRequestResponseXml.GetElementsByTagName("ns:response");
                                                        if (responseList.Count > 0)
                                                        {
                                                            string responseRecords = responseList[0].InnerXml;
                                                            if (responseRecords.Contains("noRecords"))
                                                            {
                                                                smevRequestData.Answer = "В базе МВД отсутствуют записи по запрашиваемому гражданину";
                                                                //   добавить  smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                SMEVEGRNDomain.Update(smevRequestData);
                                                            }
                                                            else
                                                            {
                                                                smevRequestData.Answer = responseRecords;
                                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                SMEVEGRNDomain.Update(smevRequestData);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            smevRequestData.Answer = "В базе МВД отсутствуют записи";
                                                            smevRequestData.RequestState = RequestState.ResponseReceived;
                                                            SMEVEGRNDomain.Update(smevRequestData);
                                                        }
                                                        //ACK
                                                        //Создаем элемент для подписания
                                                        XmlDocument ackForSign = XmlOperations.CreateXmlDocAckForSign(ackMessageId);

                                                        //Получаем подпись
                                                        sign = SmevSign.Sign(ackForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                                        //Создаем документ для отправки
                                                        XmlDocument egrnAck = XmlOperations.CreateAck(ackForSign, sign);
                                                        xwAck.WriteLine(egrnAck.InnerXml);
                                                        xwAck.Flush();
                                                        streamAck.Position = 0;

                                                        //SMEVMVDFileDomain
                                                        newRec = new SMEVEGRNFile();
                                                        newRec.ObjectCreateDate = DateTime.Now;
                                                        newRec.ObjectEditDate = DateTime.Now;
                                                        newRec.ObjectVersion = 1;
                                                        newRec.SMEVEGRN = smevRequestData;
                                                        newRec.SMEVFileType = SMEVFileType.AckRequestSig;
                                                        newRec.FileInfo = _fileManager.SaveFile(streamAck, "Ack.xml");

                                                        SMEVEGRNFileDomain.Save(newRec);

                                                        response = SmevWebRequest.SendRequest("urn:Ack", egrnAck, out isError);
                                                        if (isError)
                                                        {

                                                            smevRequestData.Answer += " Внимание! Запрос не удален из списка запросов СМЭВ, уведомите техподдержку";
                                                            smevRequestData.RequestState = RequestState.ResponseReceived;
                                                            smevRequestData.MessageId = "";
                                                            SMEVEGRNDomain.Update(smevRequestData);

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
                                                    SMEVEGRNDomain.Update(smevRequestData);
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
                                            SMEVEGRNDomain.Update(smevRequestData);
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
                                SMEVEGRNDomain.Update(smevRequestData);
                            }
                        }
                        else
                        {
                            smevRequestData.Answer = "Статус обращения неизвестен";
                            smevRequestData.MessageId = "";
                            smevRequestData.RequestState = RequestState.ResponseReceived;
                            SMEVEGRNDomain.Update(smevRequestData);
                        }
                    }
                }
            }
            else
            {

            }



            return JsSuccess();
        }

    }
}
