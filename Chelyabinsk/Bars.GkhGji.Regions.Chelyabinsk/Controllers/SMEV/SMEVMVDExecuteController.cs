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
    using System.Net;
    using System.Xml;
    using System.Xml.Serialization;
    using Entities.SMEV;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;

    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVMVDExecuteController : BaseController
    {
        private IFileManager _fileManager;
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;

        public SMEVMVDExecuteController(IFileManager fileManager, IDomainService<B4.Modules.FileStorage.FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }

        public IDomainService<SMEVMVDFile> SMEVMVDFileDomain { get; set; }

        public ActionResult Execute(BaseParams baseParams, Int64 taskId)
        {

            var smevRequestData = SMEVMVDDomain.Get(taskId);
            var files = SMEVMVDFileDomain.GetAll()
                .Where(x => x.SMEVMVD.Id == taskId).ToList();
            foreach (SMEVMVDFile fileRec in files)
            {
                SMEVMVDFileDomain.Delete(fileRec.Id);
            }

            MemoryStream streamReq = new MemoryStream();
            StreamWriter xwReq = new StreamWriter(streamReq, new UTF8Encoding(false));

            MemoryStream streamResp = new MemoryStream();
            StreamWriter xwResp = new StreamWriter(streamResp, new UTF8Encoding(false));

            MemoryStream streamAck = new MemoryStream();
            StreamWriter xwAck = new StreamWriter(streamAck, new UTF8Encoding(false));

            Dictionary<MVDTypeAddress, string> typeAddrCodeDict = new Dictionary<MVDTypeAddress, string>();
            typeAddrCodeDict.Add(MVDTypeAddress.BirthPlace, "000");
            typeAddrCodeDict.Add(MVDTypeAddress.LivingPlace, "200");
            typeAddrCodeDict.Add(MVDTypeAddress.FactPlace, "201");
            typeAddrCodeDict.Add(MVDTypeAddress.Other, "202");
            if (smevRequestData.RequestState == RequestState.NotFormed)
            {
                //Формируем класс с данными
                MvdSendRequest mvdSendRequest = new MvdSendRequest();
                mvdSendRequest.Id = "Id" + smevRequestData.Id;
                mvdSendRequest.BirthDate = new BirthDate()
                {
                    Year = smevRequestData.BirthDate.Year,
                    Month = smevRequestData.BirthDate.Month,
                    Day = smevRequestData.BirthDate.Day
                };
                mvdSendRequest.SNILS = smevRequestData.SNILS;
                mvdSendRequest.Surname = smevRequestData.Surname;
                mvdSendRequest.Name = smevRequestData.Name;
                mvdSendRequest.PatronymicName = smevRequestData.PatronymicName;

                List<RegistrationPlace> registrationPlace = new List<RegistrationPlace>();
                registrationPlace.Add(new RegistrationPlace()
                {
                    Place = smevRequestData.AddressPrimary,
                    Type = typeAddrCodeDict[smevRequestData.MVDTypeAddressPrimary],
                    RegionCode = smevRequestData.RegionCodePrimary.Code
                });

                if (smevRequestData.AddressAdditional != "" && smevRequestData.RegionCodeAdditional != null && smevRequestData.MVDTypeAddressAdditional != MVDTypeAddress.NotSet)
                {
                    registrationPlace.Add(new RegistrationPlace()
                    {
                        Place = smevRequestData.AddressAdditional,
                        Type = typeAddrCodeDict[smevRequestData.MVDTypeAddressAdditional],
                        RegionCode = smevRequestData.RegionCodeAdditional.Code
                    });
                }
                mvdSendRequest.RegistrationPlaces = registrationPlace;

                //Получаем сертификат
                X509Certificate2 cert = SmevSign.GetCertificateFromStore();
                Stream streamElementSend = new MemoryStream();

                //Сериализуем класс с данными в XML
                XmlSerializer serializerMvdSendRequest = new XmlSerializer(typeof(MvdSendRequest));
                XmlTextWriter xmltw = new XmlTextWriter(streamElementSend, new UTF8Encoding(false));
                serializerMvdSendRequest.Serialize(xmltw, mvdSendRequest);
                streamElementSend.Position = 0;

                XmlDocument mvdDocument = new XmlDocument();
                mvdDocument.Load(streamElementSend);
                XmlElement mvdElement = mvdDocument.GetElementsByTagName("request").Cast<XmlElement>().FirstOrDefault();

                //Формируем xml для подписи
                String messageId = String.Empty;
                XmlDocument requestForSign = XmlOperations.CreateXmlDocReqForSign(mvdElement, out messageId);
                smevRequestData.MessageId = messageId;

                //Получаем элемент с подписью
                var sign = SmevSign.Sign(requestForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                //Формируем Xml для отправки
                XmlDocument mvdRequestRequest = XmlOperations.CreateSendRequest(requestForSign, sign);

                //Пишем xml в MemoryStream
                xwReq.WriteLine(mvdRequestRequest.InnerXml);
                xwReq.Flush();
                streamReq.Position = 0;
                //SMEVMVDFileDomain
                SMEVMVDFile newRec = new SMEVMVDFile();
                newRec.ObjectCreateDate = DateTime.Now;
                newRec.ObjectEditDate = DateTime.Now;
                newRec.ObjectVersion = 1;
                newRec.SMEVMVD = smevRequestData;
                newRec.SMEVFileType = SMEVFileType.SendRequestSig;
                newRec.FileInfo = _fileManager.SaveFile(streamReq, "SendRequestRequest.xml");
                SMEVMVDFileDomain.Save(newRec);
                smevRequestData.RequestState = RequestState.Formed;
                SMEVMVDDomain.Update(smevRequestData);

                //Отправляем xml  в сМЭВ   
                Boolean isError;
                HttpWebResponse response = SmevWebRequest.SendRequest("urn:SendRequest", mvdRequestRequest, out isError);
                if (isError)
                {
                    //Распарсиваем ответ
                    smevRequestData.Answer = SmevWebRequest.GetResponseError(response);
                    //   добавить  smevRequestData.RequestState = RequestState.ResponseReceived;
                    SMEVMVDDomain.Update(smevRequestData);
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

                    newRec = new SMEVMVDFile();
                    newRec.ObjectCreateDate = DateTime.Now;
                    newRec.ObjectEditDate = DateTime.Now;
                    newRec.ObjectVersion = 1;
                    newRec.SMEVMVD = smevRequestData;
                    newRec.SMEVFileType = SMEVFileType.SendRequestSigAnswer;
                    newRec.FileInfo = _fileManager.SaveFile(streamSendRequestResponse, "SendRequestResponse.xml");
                    SMEVMVDFileDomain.Save(newRec);


                    var errorList = sendRequestResponseXml.GetElementsByTagName("faultstring");
                    if (errorList.Count > 0)
                    {
                        //Распарсиваем ответ
                        smevRequestData.Answer = errorList[0].InnerText;
                        SMEVMVDDomain.Update(smevRequestData);
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
                                SMEVMVDDomain.Update(smevRequestData);
                                //RESPONSE
                                //Создаем элемент для подписания
                                XmlDocument responseForSign = XmlOperations.CreateXmlDocRespForSign("request", "urn://ru/mvd/ibd-m/convictions/search/1.0.2");

                                //Получаем подпись
                                sign = SmevSign.Sign(responseForSign, cert, new string[] { "SIGNED_BY_CONSUMER" });

                                //Создаем документ для отправки
                                XmlDocument mvdResponse = XmlOperations.CreateGetResponse(responseForSign, sign);
                                xwResp.WriteLine(mvdResponse.InnerXml);
                                xwResp.Flush();
                                streamResp.Position = 0;

                                //SMEVMVDFileDomain
                                newRec = new SMEVMVDFile();
                                newRec.ObjectCreateDate = DateTime.Now;
                                newRec.ObjectEditDate = DateTime.Now;
                                newRec.ObjectVersion = 1;
                                newRec.SMEVMVD = smevRequestData;
                                newRec.SMEVFileType = SMEVFileType.SendResponceSig;
                                newRec.FileInfo = _fileManager.SaveFile(streamResp, "GetResponse.xml");


                                SMEVMVDFileDomain.Save(newRec);


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
                                        SMEVMVDDomain.Update(smevRequestData);
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
                                                newRec = new SMEVMVDFile();
                                                newRec.ObjectCreateDate = DateTime.Now;
                                                newRec.ObjectEditDate = DateTime.Now;
                                                newRec.ObjectVersion = 1;
                                                newRec.SMEVMVD = smevRequestData;
                                                newRec.SMEVFileType = SMEVFileType.SendResponceSigAnswer;
                                                newRec.FileInfo = _fileManager.SaveFile(streamGetRequestResponse, "GetRequestResponse.xml");
                                                SMEVMVDFileDomain.Save(newRec);

                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                SMEVMVDDomain.Update(smevRequestData);
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
                                                                SMEVMVDDomain.Update(smevRequestData);
                                                            }
                                                            else
                                                            {
                                                                smevRequestData.Answer = responseRecords;
                                                                smevRequestData.RequestState = RequestState.ResponseReceived;
                                                                SMEVMVDDomain.Update(smevRequestData);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            smevRequestData.Answer = "В базе МВД отсутствуют записи";
                                                            smevRequestData.RequestState = RequestState.ResponseReceived;
                                                            SMEVMVDDomain.Update(smevRequestData);
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
                                                        newRec = new SMEVMVDFile();
                                                        newRec.ObjectCreateDate = DateTime.Now;
                                                        newRec.ObjectEditDate = DateTime.Now;
                                                        newRec.ObjectVersion = 1;
                                                        newRec.SMEVMVD = smevRequestData;
                                                        newRec.SMEVFileType = SMEVFileType.AckRequestSig;
                                                        newRec.FileInfo = _fileManager.SaveFile(streamAck, "Ack.xml");

                                                        SMEVMVDFileDomain.Save(newRec);

                                                        response = SmevWebRequest.SendRequest("urn:Ack", mvdAck, out isError);
                                                        if (isError)
                                                        {

                                                            smevRequestData.Answer += " Внимание! Запрос не удален из списка запросов СМЭВ, уведомите техподдержку";
                                                            smevRequestData.RequestState = RequestState.ResponseReceived;
                                                            smevRequestData.MessageId = "";
                                                            SMEVMVDDomain.Update(smevRequestData);

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
                                                    SMEVMVDDomain.Update(smevRequestData);
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
                                            SMEVMVDDomain.Update(smevRequestData);
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
                                SMEVMVDDomain.Update(smevRequestData);
                            }
                        }
                        else
                        {
                            smevRequestData.Answer = "Статус обращения неизвестен";
                            smevRequestData.MessageId = "";
                            smevRequestData.RequestState = RequestState.ResponseReceived;
                            SMEVMVDDomain.Update(smevRequestData);
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
