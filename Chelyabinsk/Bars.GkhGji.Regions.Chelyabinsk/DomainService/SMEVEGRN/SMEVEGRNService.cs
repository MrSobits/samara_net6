using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums.Egrn;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;
using SMEV3Library.Entities;
using Bars.B4.Modules.Tasks.Common.Utils;
using Bars.B4.Modules.FIAS;
using System.Net;
using System.Text;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    public class SMEVEGRNService : ISMEVEGRNService
    {
        #region Constants

        static XNamespace req = @"urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/1.1.2";
        static XNamespace das = @"urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/dRegionsRF/1.0.0";
        static XNamespace tns = @"http://rosreestr.ru/services/v0.18/TStatementRequestEGRN";
        static XNamespace stCom = @"http://rosreestr.ru/services/v0.1/TStatementCommons";
        static XNamespace subj = @"http://rosreestr.ru/services/v0.1/commons/Subjects";
        static XNamespace obj = @"http://rosreestr.ru/services/v0.1/commons/TObject";
        static XNamespace adr = @"http://rosreestr.ru/services/v0.1/commons/Address";
        static XNamespace requestTech = @"http://rosreestr.ru/services/v0.12/TRequest";
        static XNamespace ns2 = @"urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2";
        static string senderType = "Vedomstvo";
        static string actionCode = "659411111112";
        static string declarantId = "03b288ac-e3fd-49ac-98e0-7d125f4cc1e2";

        #endregion

        #region Properties

        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }
        public IDomainService<SMEVEGRNFile> SMEVEGRNFileDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }
        public IDomainService<Fias> FiasDomain { get; set; }
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVEGRNService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса выписки ЕГРН
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVEGRN requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                SMEVEGRNFileDomain.GetAll().Where(x => x.SMEVEGRN == requestData).ToList().ForEach(x => SMEVEGRNFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var attachment = SMEVEGRNFileDomain.GetAll().Where(x => x.SMEVFileType == SMEVFileType.RequestAttachment).OrderByDescending(x => x.ObjectCreateDate).First().FileInfo;
                var requestResult = _SMEV3Service.SendRequestAsync(request, new List<FileAttachment> { new FileAttachment { FileData = _fileManager.GetFile(attachment).ReadAllBytes(), FileName = attachment.FullName } }, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVEGRNDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                //
                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVEGRNDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                //ошибки связи прокидываем в контроллер
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessResponse(SMEVEGRN requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //requestData = SMEVEGRNDomain.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
                response.Attachments.ForEach(x => SaveAttachmentFile(requestData, x.FileData, x.FileName));
                response.FSAttachmentsList.ForEach(x => SaveFSAttachmentFile(requestData, x));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в Росреестре, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в Росреестре, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    SaveFile(requestData, response.RequestRejected, "RequestRejected.xml");
                    SetErrorState(requestData, "Сервер отклонил запрос, подробности в приаттаченом файле RequestRejected.xml");
                }
                else if (response.RequestStatus != null)
                {
                    var status = response.RequestStatus.Element(ns2 + "StatusDescription")?.Value;
                    ChangeStateAnswer(requestData, RequestState.Queued, status);
                }
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                }
                else
                {
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");

                    var RosReestrResponse = response.MessagePrimaryContent.Element(req + "Response");
                    if (RosReestrResponse == null)
                    {
                        SetErrorState(requestData, "Пустой ответ от Росреестра");
                        return false;
                    }

                    //switch (FNSresponse.Element(ns1Namespace + "КодОбр")?.Value)
                    //{
                    //    case "53":
                    //        SetErrorState(requestData, "сведения в отношении индивидуального предпринимателя не могут быть предоставлены в электронном виде");
                    //        return false;
                    //    case "54":
                    //        requestData.RequestState = RequestState.ResponseReceived;
                    //        requestData.Answer = "Данные содержатся в приложенном ZIP-архиве";
                    //        SMEVEGRIPDomain.Update(requestData);
                    //        return true;
                    //}

                    //var SvULElement = FNSresponse.Element(ns1Namespace + "СвИП");
                    //if (SvULElement == null)
                    //{
                    //    SetErrorState(requestData, "Секция СвИП отсутствует");
                    //    return false;
                    //}

                    //ProcessResponseXML(requestData, SvULElement);

                    requestData.RequestState = RequestState.ResponseReceived;
                    if (response.Attachments != null && response.Attachments.Count() > 0)
                    {
                        requestData.Answer = "Успешно, имеются вложения в количестве " + response.Attachments.Count();
                    }
                    else
                    {
                        requestData.Answer = "Успешно, вложений не обнаружено";
                    }
                    SMEVEGRNDomain.Update(requestData);
                    return true;
                }
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        public IDataResult GetListRoom(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var roId = loadParams.Filter.GetAs("RO", 0L);
            if (roId <= 0)
            {
                return new ListDataResult();
            }

            return RoomDomain.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.RoomNum,
                    x.CadastralNumber
                })
                .ToListDataResult(loadParams);
        }
        #endregion

        #region Private methods

        private XElement GetInformationRequestXML(SMEVEGRN requestData)
        {
            if (requestData.EGRNApplicantType.Code == "357013000000")
            {
                string messageId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();
                var directory = Path.Combine(Path.GetTempPath(), $"_{messageId}");
                var requestEGRNFileName = $"{directory}\\requestEGRN.xml";
                var requestTechFileName = $"{directory}\\request.xml";
                Directory.CreateDirectory(directory);
                XElement requestEGRN = GetRequestEGRN(requestData);
                requestEGRN.Save(requestEGRNFileName);
                var requestEGRNFileSigName = requestEGRNFileName + ".sig";
                File.WriteAllBytes(requestEGRNFileSigName, _SMEV3Service.SignFileDetached2012256(requestEGRNFileName));
                XElement requestTech = GetRequestTech(requestEGRNFileName.Split('\\').Last(), new List<string> { requestEGRNFileSigName.Split('\\').Last() });
                requestTech.Save(requestTechFileName);
                var requestTechFileSigName = requestTechFileName + ".sig";
                File.WriteAllBytes(requestTechFileSigName, _SMEV3Service.SignFileDetached2012256(requestTechFileName));
                ZipFile.CreateFromDirectory(directory, directory + ".zip");
                SaveAttachment(requestData, directory + ".zip");
                Directory.Delete(directory, true);
                File.Delete(directory + ".zip");
                var result = new XElement(req + "Request",
                    new XElement(req + "region", requestData.RegionCode.Code.TrimStart('0')),
                    new XElement(req + "externalNumber", messageId),
                    new XElement(req + "senderType", senderType),
                    new XElement(req + "actionCode", actionCode),
                    new XElement(req + "Attachment",
                        new XElement(req + "IsMTOMAttachmentContent", "true"),
                        new XElement(req + "RequestDescription",
                            new XElement(req + "IsUnstructuredFormat", "false"),
                            new XElement(req + "IsZippedPacket", "true"),
                            new XElement(req + "fileName", requestTechFileName.Split('\\').Last())),
                        new XElement(req + "Statement",
                            new XElement(req + "IsUnstructuredFormat", "false"),
                            new XElement(req + "IsZippedPacket", "true"),
                            new XElement(req + "fileName", requestEGRNFileName.Split('\\').Last())),
                        new XElement(req + "File",
                            new XElement(req + "IsUnstructuredFormat", "true"),
                            new XElement(req + "IsZippedPacket", "true"),
                            new XElement(req + "fileName", requestEGRNFileName.Split('\\').Last() + ".sig")),
                        new XElement(req + "File",
                            new XElement(req + "IsUnstructuredFormat", "true"),
                            new XElement(req + "IsZippedPacket", "true"),
                            new XElement(req + "fileName", requestTechFileName.Split('\\').Last() + ".sig"))));

                result.SetAttributeValue(XNamespace.Xmlns + "req", req);
                return result;
            }
            else { return null; }
        }

        private XElement GetRequestEGRN(SMEVEGRN requestData)
        {
            XElement requestDetails = null;
            //var dfd = requestData.RequestDataType.GetEnumMeta();
            if (!string.IsNullOrEmpty(requestData.CadastralNUmber))
            {
                switch (requestData.RequestType)
                {
                    //case RequestType.requiredDataSubject:
                    //    requestEGRNDataAction = new XElement(tns + "requestEGRNDataAction",
                    //        new XElement(tns + "extractSubject",
                    //            new XElement(tns + "owner",
                    //                new XElement(tns + "person",
                    //                    new XElement(subj + "surname", requestData.PersonSurname),
                    //                    new XElement(subj + "firstname", requestData.PersonName),
                    //                    new XElement(subj + "idDocumentRef",
                    //                        new ))
                    //            )),
                    //    break;
                    case RequestType.requestEGRNDataAction:
                        switch (requestData.EGRNObjectType.Code)
                        {
                            case "002001003000": //помещение
                                requestDetails = new XElement(tns + "requestEGRNDataAction",
                                new XElement(tns + "extractDataAction",
                                    new XElement(tns + "object",
                                        new XElement(obj + "objectTypeCode", requestData.EGRNObjectType.Code),
                                        new XElement(obj + "cadastralNumber",
                                            new XElement(obj + "cadastralNumber", requestData.CadastralNUmber))),
                                    new XElement(tns + "requestType", requestData.RequestDataType.GetEnumMeta().Description)));
                                break;
                            case "002001002000": //здание
                                requestDetails = new XElement(tns + "requestEGRNDataAction",
                                new XElement(tns + "extractDataAction",
                                    new XElement(tns + "object",
                                        new XElement(obj + "objectTypeCode", requestData.EGRNObjectType.Code),
                                        new XElement(obj + "cadastralNumber",
                                            new XElement(obj + "cadastralNumber", requestData.CadastralNUmber))),
                                    new XElement(tns + "requestType", requestData.RequestDataType.GetEnumMeta().Description)));
                                break;
                        }

                        break;
                }
            }
            else
            {
                var b4fiasaddress = FiasAddressDomain.Get(requestData.RealityObject.FiasAddress.Id);
                var b4fias = FiasDomain.GetAll().FirstOrDefault(x => x.AOGuid == b4fiasaddress.StreetGuidId && x.ActStatus == FiasActualStatusEnum.Actual);
                XElement distr = null;
                XElement city = null;
                XElement locality = null;
                XElement street = null;
                XElement house = null;
                XElement building = null;
                //string house = "д. " + b4fiasaddress.House;
                //if (!string.IsNullOrEmpty(b4fiasaddress.Housing))
                //{
                //    house += ", корп. " + b4fiasaddress.Housing;
                //}
                //if (!string.IsNullOrEmpty(b4fiasaddress.Building))
                //{
                //    house += ", стр. " + b4fiasaddress.Building;
                //}

                house = new XElement(adr + "house",
                                    new XElement(adr + "type", "д"),
                                    new XElement(adr + "value", b4fiasaddress.House));
                if (!string.IsNullOrEmpty(b4fiasaddress.Housing))
                {
                    building = new XElement(adr + "building",
                                    new XElement(adr + "type", "к"),
                                    new XElement(adr + "value", b4fiasaddress.Housing));
                }
                if (b4fias != null)
                {
                    var localfias = FiasDomain.GetAll().FirstOrDefault(x => x.AOGuid == requestData.RealityObject.FiasAddress.PlaceGuidId && x.ActStatus == FiasActualStatusEnum.Actual);
                    if (localfias != null)
                    {
                        locality = new XElement(adr + "locality",
                                    new XElement(adr + "code", localfias.CodePlace),
                                    new XElement(adr + "type", localfias.ShortName),
                                    new XElement(adr + "name", localfias.OffName));
                    }
                    var streetfias = FiasDomain.GetAll().FirstOrDefault(x => x.AOGuid == requestData.RealityObject.FiasAddress.StreetGuidId && x.ActStatus == FiasActualStatusEnum.Actual);
                    if (streetfias != null)
                    {
                        street = new XElement(adr + "street",
                                    new XElement(adr + "code", streetfias.CodeStreet),
                                    new XElement(adr + "type", streetfias.ShortName),
                                    new XElement(adr + "name", streetfias.OffName));
                    }

                    var raion = requestData.RealityObject.Municipality;
                    if (raion.Name.Contains("Челябинский городской"))
                    {
                        city = new XElement(adr + "city",
                                             new XElement(adr + "code", "001"),
                                             new XElement(adr + "type", "г."),
                                             new XElement(adr + "name", "Челябинск"));
                    }
                    else
                    {
                        var fiasMun = FiasDomain.GetAll().FirstOrDefault(x => x.AOGuid == requestData.RealityObject.Municipality.FiasId && x.ActStatus == FiasActualStatusEnum.Actual);
                        if (fiasMun != null)
                        {
                            if (fiasMun.CodeArea != "000")
                            {
                                distr = new XElement(adr + "district",
                                             new XElement(adr + "code", fiasMun.CodeArea),
                                             new XElement(adr + "type", fiasMun.ShortName),
                                             new XElement(adr + "name", fiasMun.OffName));
                            }
                            var mosett = requestData.RealityObject.MoSettlement;
                            if (mosett != null)
                            {
                                var fiasmosett = FiasDomain.GetAll().FirstOrDefault(x => x.AOGuid == requestData.RealityObject.FiasAddress.PlaceGuidId && x.ActStatus == FiasActualStatusEnum.Actual);
                                if (fiasmosett != null)
                                {
                                    if (fiasmosett.CodeCity != "000")
                                    {
                                        city = new XElement(adr + "city",
                                                new XElement(adr + "code", fiasmosett.CodeCity),
                                                new XElement(adr + "type", fiasmosett.ShortName),
                                                new XElement(adr + "name", fiasmosett.OffName));
                                    }
                                }
                            }
                        }
                    }
                }
                switch (requestData.RequestType)
                {
                    //case RequestType.requiredDataSubject:
                    //    requestEGRNDataAction = new XElement(tns + "requestEGRNDataAction",
                    //        new XElement(tns + "extractSubject",
                    //            new XElement(tns + "owner",
                    //                new XElement(tns + "person",
                    //                    new XElement(subj + "surname", requestData.PersonSurname),
                    //                    new XElement(subj + "firstname", requestData.PersonName),
                    //                    new XElement(subj + "idDocumentRef",
                    //                        new ))
                    //            )),
                    //    break;
                    case RequestType.requestEGRNDataAction:
                        switch (requestData.EGRNObjectType.Code)
                        {
                            case "002001003000": //помещение
                                requestDetails = new XElement(tns + "requestEGRNDataAction",
                                new XElement(tns + "extractDataAction",
                                    new XElement(tns + "object",
                                        new XElement(obj + "objectTypeCode", requestData.EGRNObjectType.Code),
                                        new XElement(obj + "address",
                                            new XElement(adr + "fias", requestData.RealityObject.FiasAddress.HouseGuid),
                                              new XElement(adr + "region",
                                             new XElement(adr + "code", "74"),
                                             new XElement(adr + "type", "обл."),
                                             new XElement(adr + "name", "Челябинская")),
                                              distr,
                                              city,
                                              locality,
                                              street,
                                             house,
                                             building!=null? building:null,
                                               new XElement(adr + "apartment",
                                             new XElement(adr + "type", requestData.Room.Type == RoomType.Living ? "кв" : "пом"),
                                             new XElement(adr + "name", requestData.Room.RoomNum)))),
                                    new XElement(tns + "requestType", requestData.RequestDataType.GetEnumMeta().Description)));
                                break;
                            case "002001002000": //здание
                                requestDetails = new XElement(tns + "requestEGRNDataAction",
                                new XElement(tns + "extractDataAction",
                                    new XElement(tns + "object",
                                        new XElement(obj + "objectTypeCode", requestData.EGRNObjectType.Code),
                                              new XElement(obj + "address",
                                            new XElement(adr + "fias", requestData.RealityObject.FiasAddress.HouseGuid),
                                              new XElement(adr + "region",
                                             new XElement(adr + "code", "74"),
                                             new XElement(adr + "type", "обл."),
                                             new XElement(adr + "name", "Челябинская")),
                                              distr,
                                              city,
                                              locality,
                                              street,
                                              house,
                                              building != null ? building : null)),
                                    new XElement(tns + "requestType", requestData.RequestDataType.GetEnumMeta().Description)));
                                break;
                        }

                        break;
                }
            }
            string actionCode = "";
            string statementType = "";
            switch (requestData.RequestType)
            {
                case RequestType.requestCopyAction:
                    actionCode = "659511111111";
                    statementType = "558630100000";
                    break;
                case RequestType.requestEGRNDataAction:
                    actionCode = "659511111112";
                    statementType = "558630200000";
                    break;
                case RequestType.requestEGRNAccessAction:
                    actionCode = "659511111113";
                    statementType = "558630300000";
                    break;
            }
            var result = new XElement(tns + "EGRNRequest", new XAttribute("_id", "GZHI_" + requestData.Id),
        new XElement(tns + "header",
                    new XElement(stCom + "actionCode", actionCode),
                    new XElement(stCom + "statementType", statementType),
                    new XElement(stCom + "creationDate", DateTime.Now.ToString("O")),
                    new XElement(stCom + "serviceInfo",
                    new XElement(stCom + "requestCause", "Постановление Правительства Челябинской области от 19.09.2013 N 283-П (ред. от 20.10.2017) \"О Порядке осуществления регионального государственного жилищного надзора в Челябинской области\". Пп. 3 п. 9 Положения о Главном управлении \"Государственная жилищная инспекция Челябинской области\""))),
                new XElement(tns + "declarant",
                    new XAttribute("_id", declarantId),
                    new XElement(subj + "other",
                        new XElement(subj + "name", "Главное управление \"Государственная жилищная инспекция Челябинской области\""),
                        new XElement(subj + "inn", "7451374918"),
                        new XElement(subj + "ogrn", "1147451012400"),
                        new XElement(subj + "kpp", "745301001"),
                        new XElement(subj + "regDate", "2014-07-07")),
                    new XElement(subj + "declarantKind", requestData.EGRNApplicantType.Code)),
                new XElement(tns + "requestDetails", requestDetails),
                new XElement(tns + "deliveryDetails",
                    new XElement(stCom + "resultDeliveryMethod",
                        new XElement(stCom + "recieveResultTypeCode", "webService"),
                        new XElement(stCom + "dataReceiveForm", "electronic"))),
                new XElement(tns + "statementAgreements",
                    new XElement(stCom + "persDataProcessingAgreement", "01"),
                    new XElement(stCom + "actualDataAgreement", "03"),
                    new XElement(stCom + "qualityOfServiceAgreement", "01"),
                    new XElement(stCom + "qualityOfServiceTelephoneNumber", requestData.QualityPhone)));
            result.SetAttributeValue(XNamespace.Xmlns + "tns", tns);
            result.SetAttributeValue(XNamespace.Xmlns + "stCom", stCom);
            result.SetAttributeValue(XNamespace.Xmlns + "subj", subj);
            result.SetAttributeValue(XNamespace.Xmlns + "obj", obj);
            return result;
        }

        private XElement GetRequestTech(string statementFileName, List<string> fileNames)
        {
            var result = new XElement(requestTech + "request",
                new XElement(requestTech + "statementFile",
                    new XElement(requestTech + "fileName", statementFileName)));
            foreach (var fileName in fileNames)
            {
                result.Add(
                    new XElement(requestTech + "file",
                        new XElement(requestTech + "fileName", fileName)));
            }
            result.Add(
                new XElement(requestTech + "requestType", "111300003000"));
            return result;
        }

        //private XElement GetIdentifier(SMEVEGRN requestData)
        //{
        //    if (requestData.InnOgrn == InnOgrn.INN)
        //        return new XElement(ns1Namespace + "ИНН", requestData.INNReq);
        //    else
        //        return new XElement(ns1Namespace + "ОГРНИП", requestData.INNReq);
        //}

        private void ChangeState(SMEVEGRN requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVEGRNDomain.Update(requestData);
        }

        private void ChangeStateAnswer(SMEVEGRN requestData, RequestState state, string answer)
        {
            requestData.RequestState = state;
            requestData.Answer = answer;
            SMEVEGRNDomain.Update(requestData);
        }

        private void SetErrorState(SMEVEGRN requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVEGRNDomain.Update(requestData);
        }

        private void SaveFile(SMEVEGRN request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVEGRNFileDomain.Save(new SMEVEGRNFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRN = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private void SaveFile(SMEVEGRN request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет

            SMEVEGRNFileDomain.Save(new SMEVEGRNFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRN = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveAttachmentFile(SMEVEGRN request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            try
            {
                SMEVEGRNFileDomain.Save(new SMEVEGRNFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVEGRN = request,
                    SMEVFileType = SMEVFileType.ResponseAttachment,
                    FileInfo = _fileManager.SaveFile(fileName, data)
                });
            }
            catch
            {
                try
                {
                    SMEVEGRNFileDomain.Save(new SMEVEGRNFile
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1,
                        SMEVEGRN = request,
                        SMEVFileType = SMEVFileType.ResponseAttachment,
                        FileInfo = _fileManager.SaveFile("Answer.zip", data)
                    });
                }
                catch
                {
                    throw new Exception($"некорректное имя файла ответа {fileName}");
                }
            }
        }

        private void SaveFSAttachmentFile(SMEVEGRN request, FsAttachmentProxy ftpData)
        {
            //сохраняем отправленный пакет
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new
         Uri($"ftp://{ftpData.UserName}:{ftpData.Password}@172.20.3.12/{ftpData.FileName}"));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                //reqFTP.Credentials = new NetworkCredential(ftpData.UserName, ftpData.Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                Stream filestream = new MemoryStream();
                while (readCount > 0)
                {
                    filestream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                var fileInfo = _fileManager.SaveFile(filestream, ftpData.FileName);

                SMEVEGRNFileDomain.Save(new SMEVEGRNFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVEGRN = request,
                    SMEVFileType = SMEVFileType.ResponseAttachmentFTP,
                    FileInfo = fileInfo
                });
            }
            catch(Exception e)
            {
                try
                {
                    SMEVEGRNFileDomain.Save(new SMEVEGRNFile
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1,
                        SMEVEGRN = request,
                        SMEVFileType = SMEVFileType.Error,
                        FileInfo = _fileManager.SaveFile("Exception.txt", ($"{e.GetType().ToString()}\n{e.Message}\n{e.StackTrace}").GetBytes())
                    });
                }
                catch
                {
                }
            }
        }

        private void SaveAttachment(SMEVEGRN request, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVEGRNFileDomain.Save(new SMEVEGRNFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRN = request,
                SMEVFileType = SMEVFileType.RequestAttachment,
                FileInfo = _fileManager.SaveFile(fileName.Split('\\').Last(), File.ReadAllBytes(fileName))
            });
        }

        private void SaveFile(SMEVEGRN request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVEGRNFileDomain.Save(new SMEVEGRNFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRN = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVEGRN request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVEGRNFileDomain.Save(new SMEVEGRNFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVEGRN = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
