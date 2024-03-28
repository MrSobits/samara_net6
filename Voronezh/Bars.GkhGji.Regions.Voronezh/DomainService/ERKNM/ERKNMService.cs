using Bars.B4;
using Bars.B4.Config;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Namespaces;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
using Bars.Gkh.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
using System.Xml.Serialization;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ERKNM;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using SMEV3Library.Entities;
using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class ERKNMService : IERKNMService
    {
        #region Constants

        static XNamespace bdiNamespace = @"http://roskazna.ru/gisgmp/xsd/BudgetIndex/2.1.1";
        static XNamespace orgNamespace = @"http://roskazna.ru/gisgmp/xsd/Organization/2.1.1";
        static XNamespace comNamespace = @"http://roskazna.ru/gisgmp/xsd/Common/2.1.1";
        static XNamespace chgNamespace = @"http://roskazna.ru/gisgmp/xsd/Charge/2.1.1";
        static XNamespace pkgNamespace = @"http://roskazna.ru/gisgmp/xsd/Package/2.1.1";
        static XNamespace reqNamespace = @"urn://roskazna.ru/gisgmp/xsd/services/import-charges/2.1.1";
        static XNamespace rfdNamespace = @"http://roskazna.ru/gisgmp/xsd/Refund/2.1.1";
        static XNamespace pmntNamespace = @"http://roskazna.ru/gisgmp/xsd/Payment/2.1.1";
        static XNamespace ns0Namespace = @"urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.1.1";
        static XNamespace scNamespace = @"http://roskazna.ru/gisgmp/xsd/SearchConditions/2.1.1";
        static XNamespace faNamespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.1.1";
        static XNamespace erpNamespace = @"urn://ru.gov.proc.erp.communication/4.0.1";
        static XNamespace erp_typesNamespace = @"urn://ru.gov.proc.erp.communication/types/4.0.1";

        private List<string> notDeletedCodes;
        public List<FileAttachment> RequestAttachments { get; set; }

        #endregion

        #region Properties      
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }
        public IDomainService<ProsecutorOffice> ProsecutorOfficeDomain { get; set; }
        public IDomainService<DecisionControlMeasures> DecisionControlMeasuresDomain { get; set; }
        public IDomainService<DecisionVerificationSubject> DecisionVerificationSubjectDomain { get; set; }
        public IDomainService<ERKNM> GISERPDomain { get; set; }
        public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }
        public IDomainService<ProdCalendar> ProdCalendarDomain { get; set; }
        public IDomainService<ERKNMFile> GISERPFileDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<Decision> DecisionDomain { get; set; }
        public IDomainService<DecisionControlList> DecisionControlListDomain { get; set; }
        public IDomainService<ActCheck> ActCheckDomain { get; set; }
        public IDomainService<ChelyabinskActRemoval> ActRemovalDomain { get; set; }
        public IDomainService<ActRemovalViolation> ActRemovalViolationDomain { get; set; }
        public IDomainService<ActCheckRealityObject> ActCheckRealityObjectDomain { get; set; }
        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }
        public IDomainService<ActRemovalPeriod> ActRemovalPeriodDomain { get; set; }
        public IDomainService<ActCheckWitness> ActCheckWitnessDomain { get; set; }
        public IDomainService<ActRemovalWitness> ActRemovalWitnessDomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<ViolationActionsRemovGji> ViolationActionsRemovGjiDomain { get; set; }
        public IDomainService<AppealCitsAdmonition> AppealCitsAdmonitionDomain { get; set; }
        public IDomainService<PreventiveVisit> PreventiveVisitDomain { get; set; }
        public IDomainService<PreventiveVisitPeriod> PreventiveVisitPeriodDomain { get; set; }
        public IDomainService<PreventiveVisitRealityObject> PreventiveVisitRealityObjectDomain { get; set; }
        public IDomainService<AppCitAdmonVoilation> AppCitAdmonVoilationDomain { get; set; }
        public IDomainService<PreventiveVisitResultViolation> PreventiveVisitResultViolationDomain { get; set; }
        public IDomainService<PreventiveVisitResult> PreventiveVisitResultDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public ERKNMService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        public IDataResult GetListDecision(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var giserpdocsServ = this.Container.Resolve<IDomainService<ERKNM>>();
            var docidList = giserpdocsServ.GetAll()
                .Where(x => x.Disposal != null)
                .Select(x => x.Disposal.Id).Distinct().ToList();

            var data = DocumentGjiDomain.GetAll()
                .Where(x => x.ObjectCreateDate >= DateTime.Now.AddMonths(-26))
                .Where(x => !x.State.StartState)
                .Where(x => !docidList.Contains(x.Id))
                .Where(x => x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Decision)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    x.TypeDocumentGji,
                    State = x.State.Name,
                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        public bool SendAskProsecOfficesRequest(IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request;
                request = GetPaymentRequestXML();
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                //requestData.BillFor = "Запрос оплат";
                string msgId = requestResult.MessageId;


                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestResult.SendedData, "SendRequestRequest.dat", msgId);
                SaveFile(requestResult.ReceivedData, "SendRequestResponse.dat", msgId);

                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SaveException(requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestResult.FaultXML, "Fault.xml");

                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани

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
                SaveException(e);
            }

            return false;
        }

        public enum TypeAdressForCheck
        {
            notSet,
            inGji,
            juradrContragent,
            checkedMkd
        }

        static XNamespace ns6 = @"urn://ru.gov.proc.erknm.communication/6.0.2";

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <param name="response">Ответ</param>
        /// <param name="indicator">Индикатор прогресса для таски</param>
        public bool TryProcessResponse(ERKNM requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //сохранение данных
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveGISERPFile(requestData, response.SendedData, "GetResponceRequest.dat");
                SaveGISERPFile(requestData, response.ReceivedData, "GetResponceResponse.dat");
                if (response.Attachments != null)
                {
                    response.Attachments.ForEach(x => SaveGISERPFile(requestData, x.FileData, x.FileName));
                }

                indicator?.Report(null, 70, "Обработка результата");

                //ошибки наши
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveGISERPException(requestData, response.Error.Exception, "");
                    return false;
                }

                //ACK - ставим вдумчиво - чтобы можнор было считать повторно ,если это наш косяк
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                //Ошибки присланные
                if (response.FaultXML != null)
                {
                    SaveGISERPFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveGISERPFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ЕРП, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = response.RequestRejected.Element(SMEVNamespaces12.TypesNamespace + "RejectionReasonDescription")?.Value.Cut(500);
                    GISERPDomain.Update(requestData);
                    return true;
                }
                else
                {
                    //ответ пустой?
                    if (response.MessagePrimaryContent == null)
                    {
                        SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                        return false;
                    }
                    else
                    {
                        indicator?.Report(null, 80, "Разбор содержимого");
                        try
                        {
                            var erknmResponse = response.MessagePrimaryContent.Element(ns6 + "Response");
                            if (requestData.ERKNMDocumentType == ERKNMDocumentType.Admonition)
                            {
                                if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
                                {
                                    LetterFromErknmType resp = Deserialize<LetterFromErknmType>(erknmResponse);
                                    if (resp != null)
                                    {
                                        if (resp.Item is MessageFromErknmSetType)
                                        {
                                            MessageFromErknmSetType set = resp.Item as MessageFromErknmSetType;
                                            foreach (var item in set.Items)
                                            {
                                                if (item is CreateInspectionResponseType)
                                                {
                                                    CreateInspectionResponseType crr = item as CreateInspectionResponseType;
                                                    if (crr != null && crr.RequestStatus.Value != "FAILURE")
                                                    {
                                                        requestData.Answer = $"Создано ПМ Номер = {crr.Inspection.erknmId}";
                                                        requestData.ERPID = crr.Inspection.erknmId;
                                                        requestData.OBJECT_GUID = crr.Inspection.iGuid;
                                                        GISERPDomain.Update(requestData);
                                                        var admon = AppealCitsAdmonitionDomain.Get(requestData.AppealCitsAdmonition.Id);
                                                        admon.ERKNMID = crr.Inspection.erknmId;
                                                        admon.ERKNMGUID = crr.Inspection.iGuid;
                                                        admon.AccessGuid = crr.Inspection.accessToken;
                                                        AppealCitsAdmonitionDomain.Update(admon);
                                                    }
                                                    else if (crr != null && crr.RequestStatus.Value == "FAILURE")
                                                    {
                                                        requestData.Answer = GetErrorsNodeValue(crr.Errors);
                                                        requestData.RequestState = RequestState.Error;
                                                        GISERPDomain.Update(requestData);
                                                    }
                                                    
                                                }

                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    LetterFromErknmType resp = Deserialize<LetterFromErknmType>(erknmResponse);
                                    if (resp != null)
                                    {
                                        if (resp.Item is MessageFromErknmSetType)
                                        {
                                            MessageFromErknmSetType set = resp.Item as MessageFromErknmSetType;
                                            foreach (var item in set.Items)
                                            {
                                                if (item is UpdateInspectionResponseType)
                                                {
                                                    UpdateInspectionResponseType crr = item as UpdateInspectionResponseType;
                                                    if (crr != null && crr.RequestStatus.Value != "FAILURE")
                                                    {
                                                        requestData.Answer = $"Обновлено ПМ Номер = {requestData.ERPID}";
                                                        GISERPDomain.Update(requestData);
                                                    }
                                                    else if (crr != null && crr.RequestStatus.Value == "FAILURE")
                                                    {
                                                        requestData.Answer = GetErrorsNodeValue(crr.Errors);
                                                        requestData.RequestState = RequestState.Error;
                                                        GISERPDomain.Update(requestData);
                                                    }
                                                }

                                            }
                                        }

                                    }

                                }

                            }
                            if (requestData.ERKNMDocumentType == ERKNMDocumentType.PreventiveAct)
                            {
                                if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
                                {
                                    LetterFromErknmType resp = Deserialize<LetterFromErknmType>(erknmResponse);
                                    if (resp != null)
                                    {
                                        if (resp.Item is MessageFromErknmSetType)
                                        {
                                            MessageFromErknmSetType set = resp.Item as MessageFromErknmSetType;
                                            foreach (var item in set.Items)
                                            {
                                                if (item is CreateInspectionResponseType)
                                                {
                                                    CreateInspectionResponseType crr = item as CreateInspectionResponseType;
                                                    if (crr != null && crr.RequestStatus.Value != "FAILURE")
                                                    {
                                                        requestData.Answer = $"Создан профвизит Номер = {crr.Inspection.erknmId}";
                                                        requestData.ERPID = crr.Inspection.erknmId;
                                                        requestData.OBJECT_GUID = crr.Inspection.iGuid;
                                                        GISERPDomain.Update(requestData);
                                                        var pv = PreventiveVisitDomain.Get(requestData.Disposal.Id);
                                                        pv.ERKNMID = crr.Inspection.erknmId;
                                                        pv.ERKNMGUID = crr.Inspection.iGuid;
                                                        pv.AccessGuid = crr.Inspection.accessToken;
                                                        PreventiveVisitDomain.Update(pv);
                                                    }
                                                    else if (crr != null && crr.RequestStatus.Value == "FAILURE")
                                                    {
                                                        requestData.Answer = GetErrorsNodeValue(crr.Errors);
                                                        requestData.RequestState = RequestState.Error;
                                                        GISERPDomain.Update(requestData);
                                                    }
                                                }

                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    LetterFromErknmType resp = Deserialize<LetterFromErknmType>(erknmResponse);
                                    if (resp != null)
                                    {
                                        if (resp.Item is MessageFromErknmSetType)
                                        {
                                            MessageFromErknmSetType set = resp.Item as MessageFromErknmSetType;
                                            foreach (var item in set.Items)
                                            {
                                                if (item is UpdateInspectionResponseType)
                                                {
                                                    UpdateInspectionResponseType crr = item as UpdateInspectionResponseType;
                                                    if (crr != null && crr.RequestStatus.Value != "FAILURE")
                                                    {
                                                        requestData.Answer = $"Обновлено ПМ Номер = {requestData.ERPID}";
                                                        GISERPDomain.Update(requestData);
                                                    }
                                                    else if (crr != null && crr.RequestStatus.Value == "FAILURE")
                                                    {
                                                        requestData.Answer = GetErrorsNodeValue(crr.Errors);
                                                        requestData.RequestState = RequestState.Error;
                                                    }
                                                }

                                            }
                                        }

                                    }

                                }

                            }
                            else if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
                            {
                                LetterFromErknmType resp = Deserialize<LetterFromErknmType>(erknmResponse);
                                if (resp != null)
                                {
                                    if (resp.Item is MessageFromErknmSetType)
                                    {
                                        MessageFromErknmSetType set = resp.Item as MessageFromErknmSetType;
                                        foreach (var item in set.Items)
                                        {
                                            if (item is CreateInspectionResponseType)
                                            {
                                                CreateInspectionResponseType crr = item as CreateInspectionResponseType;
                                                if (crr != null && crr.RequestStatus.Value != "FAILURE")
                                                {
                                                    requestData.Answer = $"Создано КНМ Номер = {crr.Inspection.erknmId}";
                                                    requestData.ERPID = crr.Inspection.erknmId;
                                                    requestData.OBJECT_GUID = crr.Inspection.iGuid;
                                                    GISERPDomain.Update(requestData);
                                                    var dec = DecisionDomain.Get(requestData.Disposal.Id);
                                                    dec.ERKNMID = crr.Inspection.accessToken;
                                                    dec.ERPID = crr.Inspection.erknmId;
                                                    DecisionDomain.Update(dec);

                                                    //формируем новое начисление
                                                    XElement request = GetSecondRequestXML(requestData, crr);
                                                    ChangeState(requestData, RequestState.Formed);

                                                    indicator?.Report(null, 20, "Отправка запроса");
                                                    var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                                                    requestData.MessageId = requestResult.MessageId;

                                                    GISERPDomain.Update(requestData);

                                                    indicator?.Report(null, 80, "Сохранение данных для отладки");
                                                    SaveGISERPFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                                                    SaveGISERPFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                                                    //
                                                    indicator?.Report(null, 90, "Обработка результата");
                                                    if (requestResult.Error != null)
                                                    {
                                                        SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                                                        SaveGISERPException(requestData, requestResult.Error.Exception, "");
                                                    }
                                                    else if (requestResult.FaultXML != null)
                                                    {
                                                        SaveGISERPFile(requestData, requestResult.FaultXML, "Fault.xml");
                                                        SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ЕРП, подробности в файле Fault.xml");
                                                        return false;
                                                    }
                                                    else if (requestResult.Status != "requestIsQueued")
                                                    {
                                                        SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        //изменяем статус
                                                        //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                                                        requestData.RequestState = RequestState.Queued;
                                                        if (string.IsNullOrEmpty(requestData.Answer))
                                                        {
                                                            requestData.Answer = "Поставлено в очередь";
                                                        }
                                                        GISERPDomain.Update(requestData);
                                                    }

                                                }
                                                else if (crr != null && crr.RequestStatus.Value == "FAILURE")
                                                {
                                                    requestData.Answer = GetErrorsNodeValue(crr.Errors);
                                                    requestData.RequestState = RequestState.Error;
                                                    GISERPDomain.Update(requestData);
                                                }

                                             


                                            }
                                            else if (item is UpdateInspectionResponseType)
                                            {
                                                UpdateInspectionResponseType crr = item as UpdateInspectionResponseType;
                                                if (crr != null && crr.RequestStatus.Value != "FAILURE")
                                                {
                                                    requestData.Answer = $"Обновлено КНМ Номер = {requestData.ERPID}";
                                                    GISERPDomain.Update(requestData);
                                                }
                                                else if (crr != null && crr.RequestStatus.Value == "FAILURE")
                                                {
                                                    requestData.Answer = GetErrorsNodeValue(crr.Errors);
                                                    requestData.RequestState = RequestState.Error;
                                                    GISERPDomain.Update(requestData);
                                                }
                                            }
                                        }
                                    }

                                }

                            }




                        }
                        catch (Exception ex)
                        {
                            requestData.Answer = ex.Message;
                        }

                    }


                    //ToDo: разбираем xml, которую прислал сервер

                }
                return false;
            }
            catch (Exception e)
            {
                SaveGISERPException(requestData, e, response.MessagePrimaryContent.ToString());
                SetErrorState(requestData, "TryProcessResponse exception: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessGetProsecutorOfficeResponse(GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                var idsForDel = ProsecutorOfficeDomain.GetAll()
                    .Select(x => x.Id).Distinct().ToList();
                foreach (long id in idsForDel)
                {
                    try
                    {
                        ProsecutorOfficeDomain.Delete(id);
                    }
                    catch
                    { }
                }
                //
                string msgId = response.MessageId;
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(response.SendedData, "GetResponseRequest.dat", msgId);
                SaveFile(response.ReceivedData, "GetResponseResponse.dat", msgId);
                //сохраняем все файлы, которые прислал сервер
                response.Attachments.ForEach(x => SaveFile(x.FileData, x.FileName, msgId));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SaveException(response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(response.FaultXML, "Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    SaveFile(response.RequestRejected, "RequestRejected.xml");
                }
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    //ToDo: что-нибудь придумать
                }
                else
                {
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");

                    var procOffResponse = response.MessagePrimaryContent.Element(erpNamespace + "Response");
                    if (procOffResponse == null)
                    {

                        return false;
                    }
                    var elementGet = procOffResponse.Element(erp_typesNamespace + "Get");

                    if (elementGet == null)
                    {
                        return false;
                    }

                    var elementAskProsecOffices = elementGet.Element(erp_typesNamespace + "AskProsecOffices");

                    if (elementAskProsecOffices == null)
                    {
                        return false;
                    }

                    notDeletedCodes = ProsecutorOfficeDomain.GetAll()
                        .Select(x => x.Code).Distinct().ToList();


                    foreach (var pros1lvlelement in elementAskProsecOffices.Elements())
                    {
                        ProceedElement(pros1lvlelement);
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                SaveException(e);

            }

            return false;
        }

        public object GetInspectionInfo(BaseParams baseParams)
        {
            //try
            //{
            //    var zonalById = this.Container.Resolve<IDomainService<ZonalInspection>>().Get((long)21);
            //    string str = zonalById.NameGenetive;
            //}
            //catch (Exception e)
            //{

            //}

            var prosOfficeDomain = this.Container.Resolve<IDomainService<ProsecutorOffice>>();
            TypeAdressForCheck typeAdressForCheck = TypeAdressForCheck.notSet; // будем определять адрес (дом, юр адрес или адрес отдела ГЖИ)
            ProsecutorOffice prosOffice = null; // отдел прокуратуры надо определить по МО из проверки
            var protocolData = baseParams.Params.GetAs("protocolData", 0L); // это идентификатор Disposal, энтити наследуется от DocumentGji
            if (protocolData > 0)
            {
                var decisionDomain = this.Container.Resolve<IDomainService<Decision>>();
                Decision decision = decisionDomain.Get(protocolData);
                InspectionGji inspection = decision.Inspection;
                //объявляем возвращаемые переменные
                ERPInspectionType inspType = ERPInspectionType.PP; // по умолчанию будет плановая
                ERPAddressType adrtype = ERPAddressType.TYPE_OTHER;
                ERPNoticeType noticeType = ERPNoticeType.TYPE_OTHER;
                ERPObjectType objType = ERPObjectType.TYPE_I;
                ERPReasonType reasonType = ERPReasonType.RSN_PP_OTHER;
                ERPRiskType riskType = ERPRiskType.CLASS_VI;
                KindKND kindKnd = KindKND.HousingSupervision;
                if (decision.KindKNDGJI == KindKNDGJI.HousingSupervision)
                {
                    kindKnd = KindKND.HousingSupervision;
                }
                else if (decision.KindKNDGJI == KindKNDGJI.LicenseControl)
                {
                    kindKnd = KindKND.LicenseControl;
                }

                DateTime dateStart = decision.DateStart.HasValue ? decision.DateStart.Value : decision.ObjectCreateDate;
                string subjectAddress = string.Empty;
                string oktmo = string.Empty;
                string inspectionName = string.Empty;
                string carryoutEvents = string.Empty;
                string goals = string.Empty;
                try // получаем цели и задачи проверки для BaseChelyabinsk документов ГЖИ
                {
                    var disposalSurveyPurposeDomain = this.Container.Resolve<IDomainService<DecisionInspectionReason>>();
                    var disposalSurveyObjectiveDomain = this.Container.Resolve<IDomainService<DisposalSurveyObjective>>();//
                    var disposalVerificationSubjDomain = this.Container.Resolve<IDomainService<DecisionVerificationSubject>>();//
                    goals = "Основание КНМ: " + disposalSurveyPurposeDomain.GetAll()
                        .Where(x => x.Decision == decision)
                        .AggregateWithSeparator(x => x.InspectionReason.Name + ". " + x.Description, "; ");
                    goals = goals + " Предмет КНМ: " + disposalVerificationSubjDomain.GetAll()
                       .Where(x => x.Decision == decision)
                       .AggregateWithSeparator(x => x.SurveySubject.Name, "; ");
                }
                catch
                {
                    goals = "Не удалось получить Основание и предмет КНМ";
                }
                if (decision.KindCheck != null)
                {
                    List<TypeCheck> notPlanTypes = new List<TypeCheck> // внеплановые проверки, если такие указаны в распоряжении, то выгружаем как внеплановую
                    {
                        TypeCheck.Monitoring,
                        TypeCheck.NotPlannedDocumentation,
                        TypeCheck.NotPlannedDocumentationExit,
                        TypeCheck.NotPlannedExit,
                        TypeCheck.VisualSurvey,
                        TypeCheck.InspectionSurvey,
                        TypeCheck.InspectVisit
                    };
                    List<TypeCheck> localInspectionAdress = new List<TypeCheck>//типы когда проверка осуществляется в ГЖИ
                    {
                        TypeCheck.NotPlannedDocumentation,
                        TypeCheck.PlannedDocumentation
                    };
                    List<TypeCheck> jurUKAdress = new List<TypeCheck>// проверяем с выездом по месту нахождения проверяемого контрагента
                    {
                        TypeCheck.NotPlannedDocumentationExit,
                        TypeCheck.PlannedDocumentationExit
                    };

                    if (notPlanTypes.Contains(decision.KindCheck.Code))
                    {
                        inspType = ERPInspectionType.VP;
                    }
                    if (localInspectionAdress.Contains(decision.KindCheck.Code))
                    {
                        typeAdressForCheck = TypeAdressForCheck.juradrContragent;//всегда либо дом либо юр адрес
                    }
                    else if (jurUKAdress.Contains(decision.KindCheck.Code))
                    {
                        typeAdressForCheck = TypeAdressForCheck.juradrContragent;
                    }
                    else // во всех остальных случаях едем на дом к пострадавшим
                    {
                        typeAdressForCheck = TypeAdressForCheck.checkedMkd;
                    }

                }
                if (decision.TypeDisposal == TypeDisposalGji.Base) // если основание не проверка исполнения предписания
                {

                    switch (inspection.TypeBase) // смотрим что явилось основанием
                    {
                        case TypeBase.CitizenStatement:
                            {
                                reasonType = ERPReasonType.RSN_PP_OTHER;
                                var inspectionAppCitDomain = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
                                var inspAppcit = inspectionAppCitDomain.GetAll()
                                    .Where(x => x.Inspection == inspection).ToList();
                                string inspNameAgg = "Проверка по обращению(обращениям)";
                                foreach (var rec in inspAppcit)
                                {
                                    if (inspNameAgg == "Проверка по обращению(обращениям)")
                                    {
                                        inspNameAgg += $" № {rec.AppealCits.DocumentNumber} от {rec.AppealCits.DateFrom.Value.ToShortDateString()}";
                                    }
                                    else
                                    {
                                        inspNameAgg += $", № {rec.AppealCits.DocumentNumber} от {rec.AppealCits.DateFrom.Value.ToShortDateString()}";
                                    }

                                }
                                inspType = ERPInspectionType.VP;
                                inspectionName = inspNameAgg;//$"Проверка по обращению № {inspAppcit.AppealCits.DocumentNumber} от {inspAppcit.AppealCits.DateFrom.Value.ToShortDateString()}";

                            }
                            break;
                        case TypeBase.PlanAction: { reasonType = ERPReasonType.RSN_PP_II; } break;
                        case TypeBase.PlanJuridicalPerson: { reasonType = ERPReasonType.RSN_PP_II; } break;
                        case TypeBase.PlanOMSU: { reasonType = ERPReasonType.RSN_PP_II; } break;
                    }

                }
                else if (decision.TypeDisposal == TypeDisposalGji.DocumentGji)//проверка исполнения предписания
                {
                    var childrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
                    var parentDoc = childrenDomain.GetAll()
                        .Where(x => x.Children == decision)
                        .Where(x => x.Parent != null && x.Parent.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription).FirstOrDefault();
                    if (parentDoc != null)
                    {
                        reasonType = ERPReasonType.RSN_VP_CVI;
                        inspectionName = $"Проверка исполнения предписания №{parentDoc.Parent.DocumentNumber} от {parentDoc.Parent.DocumentDate.Value.ToShortDateString()}";
                    }
                }

                var disposalControlMeasuresDomain = this.Container.Resolve<IDomainService<DecisionControlMeasures>>();

                carryoutEvents = disposalControlMeasuresDomain.GetAll()//мероприятия проверки для челябинск-стайл ГЖИ
                    .Where(x => x.Decision == decision)
                    .AggregateWithSeparator(x => x.Description, "; ");
                carryoutEvents = carryoutEvents.Trim();

                var inspectionPlaceDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                var realObj = inspectionPlaceDomain.GetAll().Where(x => x.Inspection == inspection).FirstOrDefault();//первый дом из проверяемых домов в Inspection

                switch (typeAdressForCheck)
                {
                    case TypeAdressForCheck.checkedMkd:
                        {
                            if (realObj != null)
                            {
                                oktmo = realObj.RealityObject.Municipality.Okato;
                                subjectAddress = /*realObj.RealityObject.Municipality.Name + ", " + */realObj.RealityObject.Address;
                                adrtype = ERPAddressType.TYPE_II;
                                prosOffice = prosOfficeDomain.GetAll()//получаем прокуратуру по МО в справочнике прокуратур, который надо руками дозаполнить сначала
                                    .Where(x => x.Municipality == realObj.RealityObject.Municipality).FirstOrDefault();
                            }
                            else
                            {
                                subjectAddress = "Не удалось установить адрес проверки";
                            }
                        }
                        break;
                    case TypeAdressForCheck.inGji:
                        {
                            if (realObj == null)
                            {
                                adrtype = ERPAddressType.TYPE_I;
                                try
                                {
                                    var contragent = inspection.Contragent;
                                    if (contragent != null && contragent.Municipality != null)
                                    {
                                        var zonalInspectionDomain = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
                                        var zonalInspection = zonalInspectionDomain.GetAll()
                                            .Where(x => contragent.Municipality == x.Municipality).FirstOrDefault();
                                        subjectAddress = zonalInspection.ZonalInspection.Address;
                                        oktmo = contragent.Municipality.Okato;
                                        prosOffice = prosOfficeDomain.GetAll()
                                    .Where(x => x.Municipality == contragent.Municipality).FirstOrDefault();
                                    }
                                }
                                catch
                                {
                                    subjectAddress = "Невозможно определить адрес отдела ГЖИ";
                                }

                            }
                            else
                            {
                                adrtype = ERPAddressType.TYPE_OTHER;
                                var zonalInspectionDomain = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
                                var zonalInspection = zonalInspectionDomain.GetAll()
                                    .Where(x => realObj.RealityObject.Municipality == x.Municipality).FirstOrDefault();
                                oktmo = realObj.RealityObject.Municipality.Okato;
                                subjectAddress = zonalInspection.ZonalInspection.Address;
                                prosOffice = prosOfficeDomain.GetAll()
                                   .Where(x => x.Municipality == realObj.RealityObject.Municipality).FirstOrDefault();
                            }
                            subjectAddress = "394018, г. Воронеж, ул. Кирова, д. 6а"; // в воронеже все инспекторы сидят в одном месте
                        }
                        break;
                    case TypeAdressForCheck.juradrContragent:
                        {
                            adrtype = ERPAddressType.TYPE_I;
                            try
                            {
                                var contragent = inspection.Contragent;
                                if (contragent != null && contragent.Municipality != null)
                                {
                                    subjectAddress = contragent.JuridicalAddress;
                                    oktmo = contragent.Municipality.Oktmo;
                                    prosOffice = prosOfficeDomain.GetAll()
                                   .Where(x => x.Municipality == contragent.Municipality).FirstOrDefault();
                                }
                            }
                            catch
                            {
                                subjectAddress = "Невозможно определить адрес юрлица";
                            }

                        }
                        break;
                }
                //Получаем вид контроля, для чего проверяем контору на лицензиата

                if (decision.KindKNDGJI == KindKNDGJI.LicenseControl)
                {
                    kindKnd = KindKND.LicenseControl;
                }

                //получаем акт
                var actfromDisposal = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Parent.Id == decision.Id)
                    .Where(x => x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck || x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
                    .Select(x => x.Children).FirstOrDefault();

                DateTime? ACT_DATE_CREATE = null;
                var REPRESENTATIVE_FULL_NAME = string.Empty;
                var REPRESENTATIVE_POSITION = string.Empty;
                DateTime? START_DATE = null;
                var DURATION_HOURS = 0;
                var HasViolations = YesNoNotSet.No;

                if (actfromDisposal != null && actfromDisposal.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck)
                {
                    var actCheck = this.Container.Resolve<IDomainService<ActCheck>>().Get(actfromDisposal.Id);
                    if (actCheck != null)
                    {
                        ACT_DATE_CREATE = actCheck.DocumentDate;
                        var witness = this.Container.Resolve<IDomainService<ActCheckWitness>>().GetAll()
                            .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                        if (witness != null)
                        {
                            REPRESENTATIVE_POSITION = witness.Position;
                            REPRESENTATIVE_FULL_NAME = witness.Fio;
                        }
                        var period = ActCheckPeriodDomain.GetAll()
                         .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                        if (period != null)
                        {
                            START_DATE = period.DateStart;
                            if (period.DateStart != null && period.DateEnd != null)
                            {

                                try
                                {
                                    var convertedDate = period.DateEnd.Value.Subtract(period.DateStart.Value).Hours;
                                    DURATION_HOURS = convertedDate;
                                }
                                catch { }
                            }

                        }

                        var viol = this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                            .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                        if (viol != null && viol.HaveViolation == YesNoNotSet.Yes)
                        {
                            HasViolations = YesNoNotSet.Yes;
                        }
                        var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                            .Where(x => x.Inspection == decision.Inspection).ToList();

                    }
                }
                prosOffice = decision.ProsecutorOffice;
                //  prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Воронежской области").FirstOrDefault();
                oktmo = "1030200000000001";

                var data = new
                {
                    inspType,
                    adrtype,
                    noticeType,
                    objType,
                    oktmo,
                    reasonType,
                    riskType,
                    inspectionName,
                    carryoutEvents,
                    subjectAddress,
                    dateStart,
                    prosOffice,
                    goals,
                    kindKnd,
                    ACT_DATE_CREATE,
                    REPRESENTATIVE_FULL_NAME,
                    REPRESENTATIVE_POSITION,
                    START_DATE,
                    DURATION_HOURS,
                    HasViolations
                };

                return data;
            }
            else
            {
                return null;
            }
        }

        public ERKNM GetERKNMInfo(BaseParams baseParams)
        {
            var docType = baseParams.Params.GetAs<ERKNMDocumentType>("typeDoc");
            //GetERKNMByDisposal
            switch (docType)
            {
                case ERKNMDocumentType.Decision:
                    return GetERKNMByDisposal(baseParams);
                case ERKNMDocumentType.Admonition:
                    return GetERKNMByAdmonition(baseParams);
                case ERKNMDocumentType.PreventiveAct:
                    return GetERKNMByPreventiveAct(baseParams);
                default: return null;
            }
        }
        private ERKNM GetERKNMByAdmonition(BaseParams baseParams)
        {
            ERKNM newERKNM = new ERKNM();
            newERKNM.ERKNMDocumentType = ERKNMDocumentType.Admonition;
            newERKNM.GisErpRequestType = ERKNMRequestType.Initialization;
            var prosOfficeDomain = this.Container.Resolve<IDomainService<ProsecutorOffice>>();
            TypeAdressForCheck typeAdressForCheck = TypeAdressForCheck.notSet; // будем определять адрес (дом, юр адрес или адрес отдела ГЖИ)
            ProsecutorOffice prosOffice = null; // отдел прокуратуры надо определить по МО из проверки
            var protocolData = baseParams.Params.GetAs("protocolData", 0L);
            if (protocolData > 0)
            {
                var admonitionDomain = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();
                AppealCitsAdmonition admon = admonitionDomain.Get(protocolData);
                newERKNM.AppealCitsAdmonition = new AppealCitsAdmonition { Id = admon.Id };
                //объявляем возвращаемые переменные
                newERKNM.ERPInspectionType = ERPInspectionType.VP;
                ERPNoticeType noticeType = ERPNoticeType.TYPE_OTHER;
                ERPObjectType objType = ERPObjectType.TYPE_I;
                ERPReasonType reasonType = ERPReasonType.RSN_PP_OTHER;
                ERPRiskType riskType = ERPRiskType.CLASS_VI;
                newERKNM.KindKND = admon.KindKND;

                DateTime dateStart = admon.DocumentDate.HasValue ? admon.DocumentDate.Value : admon.ObjectCreateDate;
                string subjectAddress = string.Empty;
                string oktmo = string.Empty;
                string inspectionName = string.Empty;
                string carryoutEvents = string.Empty;
                string goals = "Основание ПМ: (ФЗ 248) Наличие у контрольного (надзорного) органа сведений о готовящихся или возможных нарушениях обязательных требований, а также о непосредственных нарушениях обязательных требований, если указанные сведения не соответствуют утвержденным индикаторам риска нарушения обязательных требований";
                newERKNM.InspectionName = "Объявление предостережения";

                if (prosOffice == null)
                {
                    prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Воронежской области").FirstOrDefault();
                }
                oktmo = "1030200000000001";
                newERKNM.ERPReasonType = ERPReasonType.RSN_PP_OTHER;
                newERKNM.ERPAddressType = ERPAddressType.TYPE_II;
                newERKNM.ERPObjectType = objType;
                newERKNM.ERPNoticeType = noticeType;
                newERKNM.ERPRiskType = riskType;
                newERKNM.CarryoutEvents = carryoutEvents;
                newERKNM.SubjectAddress = subjectAddress;
                newERKNM.OKATO = oktmo;
                newERKNM.ERPNoticeType = noticeType;
                newERKNM.InspectionName = inspectionName;
                newERKNM.Goals = goals;
                newERKNM.ProsecutorOffice = prosOffice;



                return newERKNM;
            }
            else
            {
                return null;
            }
        }

        private ERKNM GetERKNMByPreventiveAct(BaseParams baseParams)
        {
            ERKNM newERKNM = new ERKNM();
            newERKNM.ERKNMDocumentType = ERKNMDocumentType.PreventiveAct;
            newERKNM.GisErpRequestType = ERKNMRequestType.Initialization;
            var prosOfficeDomain = this.Container.Resolve<IDomainService<ProsecutorOffice>>();
            TypeAdressForCheck typeAdressForCheck = TypeAdressForCheck.notSet; // будем определять адрес (дом, юр адрес или адрес отдела ГЖИ)
            ProsecutorOffice prosOffice = null; // отдел прокуратуры надо определить по МО из проверки
            var protocolData = baseParams.Params.GetAs("protocolData", 0L);
            if (protocolData > 0)
            {
                var preventiveVisitDomain = this.Container.Resolve<IDomainService<PreventiveVisit>>();
                PreventiveVisit prev = preventiveVisitDomain.Get(protocolData);
                newERKNM.Disposal = new DocumentGji { Id = prev.Id };
                //объявляем возвращаемые переменные
                newERKNM.ERPInspectionType = ERPInspectionType.VP;
                ERPNoticeType noticeType = ERPNoticeType.TYPE_OTHER;
                ERPObjectType objType = ERPObjectType.TYPE_I;
                ERPReasonType reasonType = ERPReasonType.RSN_PP_OTHER;
                ERPRiskType riskType = ERPRiskType.CLASS_VI;
                newERKNM.KindKND = prev.KindKND == KindKNDGJI.LicenseControl? KindKND.LicenseControl:KindKND.HousingSupervision;

                DateTime dateStart = prev.DocumentDate.HasValue ? prev.DocumentDate.Value : prev.ObjectCreateDate;
                string subjectAddress = string.Empty;
                string oktmo = string.Empty;
                string inspectionName = string.Empty;
                string carryoutEvents = string.Empty;
                string goals = "Основание ПМ: (ФЗ 248) Наличие у контрольного (надзорного) органа сведений о готовящихся или возможных нарушениях обязательных требований, а также о непосредственных нарушениях обязательных требований, если указанные сведения не соответствуют утвержденным индикаторам риска нарушения обязательных требований";
                newERKNM.InspectionName = "Объявление предостережения";

                if (prosOffice == null)
                {
                    prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Воронежской области").FirstOrDefault();
                }
                oktmo = "1030200000000001";
                newERKNM.ERPReasonType = ERPReasonType.RSN_PP_OTHER;
                newERKNM.ERPAddressType = ERPAddressType.TYPE_II;
                newERKNM.ERPObjectType = objType;
                newERKNM.ERPNoticeType = noticeType;
                newERKNM.ERPRiskType = riskType;
                newERKNM.CarryoutEvents = carryoutEvents;
                newERKNM.SubjectAddress = subjectAddress;
                newERKNM.OKATO = oktmo;
                newERKNM.ERPNoticeType = noticeType;
                newERKNM.InspectionName = inspectionName;
                newERKNM.Goals = goals;
                newERKNM.ProsecutorOffice = prosOffice;



                return newERKNM;
            }
            else
            {
                return null;
            }
        }

        public ERKNM GetERKNMByDisposal(BaseParams baseParams)
        {
            ERKNM newERKNM = new ERKNM();
            newERKNM.ERKNMDocumentType = ERKNMDocumentType.Decision;
            newERKNM.GisErpRequestType = ERKNMRequestType.Initialization;

            var prosOfficeDomain = this.Container.Resolve<IDomainService<ProsecutorOffice>>();
            TypeAdressForCheck typeAdressForCheck = TypeAdressForCheck.notSet; // будем определять адрес (дом, юр адрес или адрес отдела ГЖИ)
            ProsecutorOffice prosOffice = null; // отдел прокуратуры надо определить по МО из проверки
            var protocolData = baseParams.Params.GetAs("protocolData", 0L); // это идентификатор Disposal, энтити наследуется от DocumentGji
            if (protocolData > 0)
            {
                var decisionDomain = this.Container.Resolve<IDomainService<Decision>>();
                Decision decision = decisionDomain.Get(protocolData);
                newERKNM.Disposal = new DocumentGji { Id = decision.Id };
                InspectionGji inspection = decision.Inspection;
                //объявляем возвращаемые переменные
                ERPInspectionType inspType = ERPInspectionType.PP; // по умолчанию будет плановая
                newERKNM.ERPInspectionType = ERPInspectionType.PP;
                ERPAddressType adrtype = ERPAddressType.TYPE_OTHER;
                ERPNoticeType noticeType = ERPNoticeType.TYPE_OTHER;
                ERPObjectType objType = ERPObjectType.TYPE_I;
                ERPReasonType reasonType = ERPReasonType.RSN_PP_OTHER;
                ERPRiskType riskType = ERPRiskType.CLASS_VI;
                KindKND kindKnd = KindKND.HousingSupervision;
                if (decision.KindKNDGJI == KindKNDGJI.HousingSupervision)
                {
                    kindKnd = KindKND.HousingSupervision;
                    newERKNM.KindKND = KindKND.HousingSupervision;
                }
                else if (decision.KindKNDGJI == KindKNDGJI.LicenseControl)
                {
                    kindKnd = KindKND.LicenseControl;
                    newERKNM.KindKND = KindKND.LicenseControl;
                }

                DateTime dateStart = decision.DateStart.HasValue ? decision.DateStart.Value : decision.ObjectCreateDate;
                string subjectAddress = string.Empty;
                string oktmo = string.Empty;
                string inspectionName = string.Empty;
                string carryoutEvents = string.Empty;
                string goals = string.Empty;
                try // получаем цели и задачи проверки для BaseChelyabinsk документов ГЖИ
                {
                    var disposalSurveyPurposeDomain = this.Container.Resolve<IDomainService<DecisionInspectionReason>>();
                    var disposalSurveyObjectiveDomain = this.Container.Resolve<IDomainService<DisposalSurveyObjective>>();//
                    var disposalVerificationSubjDomain = this.Container.Resolve<IDomainService<DecisionVerificationSubject>>();//
                    goals = "Основание КНМ: " + disposalSurveyPurposeDomain.GetAll()
                        .Where(x => x.Decision == decision)
                        .AggregateWithSeparator(x => x.InspectionReason.Name + ". " + x.Description, "; ");
                    goals = goals + " Предмет КНМ: " + disposalVerificationSubjDomain.GetAll()
                       .Where(x => x.Decision == decision)
                       .AggregateWithSeparator(x => x.SurveySubject.Name, "; ");
                    newERKNM.Goals = goals;
                }
                catch
                {
                    goals = "Не удалось получить Основание и предмет КНМ";
                }
                if (decision.KindCheck != null)
                {
                    List<TypeCheck> notPlanTypes = new List<TypeCheck> // внеплановые проверки, если такие указаны в распоряжении, то выгружаем как внеплановую
                    {
                        TypeCheck.Monitoring,
                        TypeCheck.NotPlannedDocumentation,
                        TypeCheck.NotPlannedDocumentationExit,
                        TypeCheck.NotPlannedExit,
                        TypeCheck.VisualSurvey,
                        TypeCheck.InspectionSurvey,
                        TypeCheck.InspectVisit
                    };
                    List<TypeCheck> localInspectionAdress = new List<TypeCheck>//типы когда проверка осуществляется в ГЖИ
                    {
                        TypeCheck.NotPlannedDocumentation,
                        TypeCheck.PlannedDocumentation
                    };
                    List<TypeCheck> jurUKAdress = new List<TypeCheck>// проверяем с выездом по месту нахождения проверяемого контрагента
                    {
                        TypeCheck.NotPlannedDocumentationExit,
                        TypeCheck.PlannedDocumentationExit
                    };

                    if (notPlanTypes.Contains(decision.KindCheck.Code))
                    {
                        inspType = ERPInspectionType.VP;
                        newERKNM.ERPInspectionType = ERPInspectionType.VP;
                    }
                    if (localInspectionAdress.Contains(decision.KindCheck.Code))
                    {
                        typeAdressForCheck = TypeAdressForCheck.juradrContragent;//всегда либо дом либо юр адрес
                    }
                    else if (jurUKAdress.Contains(decision.KindCheck.Code))
                    {
                        typeAdressForCheck = TypeAdressForCheck.juradrContragent;
                    }
                    else // во всех остальных случаях едем на дом к пострадавшим
                    {
                        typeAdressForCheck = TypeAdressForCheck.checkedMkd;
                    }

                }
                if (decision.TypeDisposal == TypeDisposalGji.Base) // если основание не проверка исполнения предписания
                {

                    switch (inspection.TypeBase) // смотрим что явилось основанием
                    {
                        case TypeBase.CitizenStatement:
                            {
                                reasonType = ERPReasonType.RSN_PP_OTHER;
                                var inspectionAppCitDomain = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
                                var inspAppcit = inspectionAppCitDomain.GetAll()
                                    .Where(x => x.Inspection == inspection).ToList();
                                string inspNameAgg = "Проверка по обращению(обращениям)";
                                foreach (var rec in inspAppcit)
                                {
                                    if (inspNameAgg == "Проверка по обращению(обращениям)")
                                    {
                                        inspNameAgg += $" № {rec.AppealCits.DocumentNumber} от {rec.AppealCits.DateFrom.Value.ToShortDateString()}";
                                    }
                                    else
                                    {
                                        inspNameAgg += $", № {rec.AppealCits.DocumentNumber} от {rec.AppealCits.DateFrom.Value.ToShortDateString()}";
                                    }

                                }
                                inspType = ERPInspectionType.VP;
                                newERKNM.ERPInspectionType = ERPInspectionType.VP;
                                inspectionName = inspNameAgg;//$"Проверка по обращению № {inspAppcit.AppealCits.DocumentNumber} от {inspAppcit.AppealCits.DateFrom.Value.ToShortDateString()}";
                                newERKNM.InspectionName = inspectionName;

                            }
                            break;
                        case TypeBase.PlanAction: { reasonType = ERPReasonType.RSN_PP_II; } break;
                        case TypeBase.PlanJuridicalPerson: { reasonType = ERPReasonType.RSN_PP_II; } break;
                        case TypeBase.PlanOMSU: { reasonType = ERPReasonType.RSN_PP_II; } break;

                    }

                }
                else if (decision.TypeDisposal == TypeDisposalGji.DocumentGji)//проверка исполнения предписания
                {
                    var childrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
                    var parentDoc = childrenDomain.GetAll()
                        .Where(x => x.Children == decision)
                        .Where(x => x.Parent != null && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription).FirstOrDefault();
                    if (parentDoc != null)
                    {
                        reasonType = ERPReasonType.RSN_VP_CVI;
                        inspectionName = $"Проверка исполнения предписания №{parentDoc.Parent.DocumentNumber} от {parentDoc.Parent.DocumentDate.Value.ToShortDateString()}";
                    }
                }

                var disposalControlMeasuresDomain = this.Container.Resolve<IDomainService<DecisionControlMeasures>>();

                carryoutEvents = disposalControlMeasuresDomain.GetAll()//мероприятия проверки для челябинск-стайл ГЖИ
                    .Where(x => x.Decision == decision)
                    .AggregateWithSeparator(x => x.Description, "; ");
                carryoutEvents = carryoutEvents.Trim();

                var inspectionPlaceDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                var realObj = inspectionPlaceDomain.GetAll().Where(x => x.Inspection == inspection).FirstOrDefault();//первый дом из проверяемых домов в Inspection

                switch (typeAdressForCheck)
                {
                    case TypeAdressForCheck.checkedMkd:
                        {
                            if (realObj != null)
                            {
                                oktmo = realObj.RealityObject.Municipality.Okato;
                                subjectAddress = /*realObj.RealityObject.Municipality.Name + ", " + */realObj.RealityObject.Address;
                                adrtype = ERPAddressType.TYPE_II;
                                prosOffice = prosOfficeDomain.GetAll()//получаем прокуратуру по МО в справочнике прокуратур, который надо руками дозаполнить сначала
                                    .Where(x => x.Municipality == realObj.RealityObject.Municipality).FirstOrDefault();
                            }
                            else
                            {
                                subjectAddress = "Не удалось установить адрес проверки";
                            }
                        }
                        break;
                    case TypeAdressForCheck.inGji:
                        {
                            if (realObj == null)
                            {
                                adrtype = ERPAddressType.TYPE_I;
                                try
                                {
                                    var contragent = inspection.Contragent;
                                    if (contragent != null && contragent.Municipality != null)
                                    {
                                        var zonalInspectionDomain = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
                                        var zonalInspection = zonalInspectionDomain.GetAll()
                                            .Where(x => contragent.Municipality == x.Municipality).FirstOrDefault();
                                        subjectAddress = zonalInspection.ZonalInspection.Address;
                                        oktmo = contragent.Municipality.Okato;
                                        prosOffice = prosOfficeDomain.GetAll()
                                    .Where(x => x.Municipality == contragent.Municipality).FirstOrDefault();
                                    }
                                }
                                catch
                                {
                                    subjectAddress = "Невозможно определить адрес отдела ГЖИ";
                                }

                            }
                            else
                            {
                                adrtype = ERPAddressType.TYPE_OTHER;
                                var zonalInspectionDomain = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
                                var zonalInspection = zonalInspectionDomain.GetAll()
                                    .Where(x => realObj.RealityObject.Municipality == x.Municipality).FirstOrDefault();
                                oktmo = realObj.RealityObject.Municipality.Okato;
                                subjectAddress = zonalInspection.ZonalInspection.Address;
                                prosOffice = prosOfficeDomain.GetAll()
                                   .Where(x => x.Municipality == realObj.RealityObject.Municipality).FirstOrDefault();
                            }
                            subjectAddress = "394018, г. Воронеж, ул. Кирова, д. 6а"; // в воронеже все инспекторы сидят в одном месте
                        }
                        break;
                    case TypeAdressForCheck.juradrContragent:
                        {
                            adrtype = ERPAddressType.TYPE_I;
                            try
                            {
                                var contragent = inspection.Contragent;
                                if (contragent != null && contragent.Municipality != null)
                                {
                                    subjectAddress = contragent.JuridicalAddress;
                                    oktmo = contragent.Municipality.Oktmo;
                                    prosOffice = prosOfficeDomain.GetAll()
                                   .Where(x => x.Municipality == contragent.Municipality).FirstOrDefault();
                                }
                            }
                            catch
                            {
                                subjectAddress = "Невозможно определить адрес юрлица";
                            }

                        }
                        break;
                }
                //Получаем вид контроля, для чего проверяем контору на лицензиата

                if (decision.KindKNDGJI == KindKNDGJI.LicenseControl)
                {
                    kindKnd = KindKND.LicenseControl;
                }

                //получаем акт
                var actfromDisposal = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Parent.Id == decision.Id)
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    .Select(x => x.Children).FirstOrDefault();

                DateTime? ACT_DATE_CREATE = null;
                var REPRESENTATIVE_FULL_NAME = string.Empty;
                var REPRESENTATIVE_POSITION = string.Empty;
                DateTime? START_DATE = null;
                var DURATION_HOURS = 0;
                var HasViolations = YesNoNotSet.No;

                if (actfromDisposal != null && actfromDisposal.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    var actCheck = this.Container.Resolve<IDomainService<ActCheck>>().Get(actfromDisposal.Id);
                    if (actCheck != null)
                    {
                        ACT_DATE_CREATE = actCheck.DocumentDate;
                        var witness = this.Container.Resolve<IDomainService<ActCheckWitness>>().GetAll()
                            .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                        if (witness != null)
                        {
                            REPRESENTATIVE_POSITION = witness.Position;
                            REPRESENTATIVE_FULL_NAME = witness.Fio;
                        }
                        var period = ActCheckPeriodDomain.GetAll()
                         .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                        if (period != null)
                        {
                            START_DATE = period.DateStart;
                            if (period.DateStart != null && period.DateEnd != null)
                            {

                                try
                                {
                                    var convertedDate = period.DateEnd.Value.Subtract(period.DateStart.Value).Hours;
                                    DURATION_HOURS = convertedDate;
                                }
                                catch { }
                            }

                        }

                        var viol = this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                            .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                        if (viol != null && viol.HaveViolation == YesNoNotSet.Yes)
                        {
                            HasViolations = YesNoNotSet.Yes;
                        }
                        var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                            .Where(x => x.Inspection == decision.Inspection).ToList();

                    }
                }
                prosOffice = decision.ProsecutorOffice;
                if (prosOffice == null)
                {
                    prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Воронежской области").FirstOrDefault();
                }
                //Прокуратура Челябинской области 
                //  prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Воронежской области").FirstOrDefault();
                oktmo = "1030200000000001";
                newERKNM.ERPReasonType = reasonType;
                newERKNM.ERPAddressType = adrtype;
                newERKNM.ERPObjectType = objType;
                newERKNM.ERPNoticeType = noticeType;
                newERKNM.ERPRiskType = riskType;
                newERKNM.CarryoutEvents = carryoutEvents;
                newERKNM.SubjectAddress = subjectAddress;
                newERKNM.OKATO = oktmo;
                newERKNM.ERPNoticeType = noticeType;
                newERKNM.InspectionName = inspectionName;
                newERKNM.KindKND = kindKnd;
                newERKNM.Goals = goals;
                newERKNM.START_DATE = START_DATE;
                newERKNM.ProsecutorOffice = prosOffice;
                newERKNM.DURATION_HOURS = DURATION_HOURS;
                newERKNM.HasViolations = HasViolations;
                newERKNM.REPRESENTATIVE_FULL_NAME = REPRESENTATIVE_FULL_NAME;
                newERKNM.REPRESENTATIVE_POSITION = REPRESENTATIVE_POSITION;


                return newERKNM;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Отправка начисления в ГИС ГМП
        /// </summary>
        public bool SendInitiationRequest(ERKNM requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                GISERPFileDomain.GetAll().Where(x => x.ERKNM == requestData).ToList().ForEach(x => GISERPFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
                {
                    requestData.checkId = $"I_{Guid.NewGuid()}";
                }
                RequestAttachments = new List<FileAttachment>();
                XElement request = GetInitiationRequestXML(requestData);
                if (RequestAttachments.Count == 0)
                {
                    RequestAttachments = null;
                }
                ChangeState(requestData, RequestState.Formed);

                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, RequestAttachments, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;

                GISERPDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveGISERPFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveGISERPFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                //
                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveGISERPException(requestData, requestResult.Error.Exception, "");
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveGISERPFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ЕРП, подробности в файле Fault.xml");
                    return false;
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                    return false;
                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = RequestState.Queued;
                    if (string.IsNullOrEmpty(requestData.Answer))
                    {
                        requestData.Answer = "Поставлено в очередь";
                    }
                    GISERPDomain.Update(requestData);
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
                SaveGISERPException(requestData, e, "");
                SetErrorState(requestData, "SendCalcRequest exception: " + e.Message);
            }

            return false;
        }

        private XElement GetInitiationRequestXML(ERKNM requestData)
        {
            if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
            {
                var result = GetInitiationOrCorrection(requestData);

                return result;
            }
            else if (requestData.GisErpRequestType == ERKNMRequestType.Correction && requestData.ERKNMDocumentType == ERKNMDocumentType.Admonition)
            {
                var result = GetAdmonitionCorrection(requestData);
                return result;
            }
            else
            {
                var result = new XElement(erpNamespace + "Request",
                new XElement(erp_typesNamespace + "Set",
                   GetCorrection(requestData)
                    ));

                result.SetAttributeValue(XNamespace.Xmlns + "erp", erpNamespace);
                result.SetAttributeValue(XNamespace.Xmlns + "erp_types", erp_typesNamespace);

                return result;
            }


        }

        private XElement GetAdmonitionCorrection(ERKNM requestData)
        {
            //SPV_26 - жилищный надзор
            //SPV_2 - лицензионный контроль
            if (requestData.ERKNMDocumentType == ERKNMDocumentType.Admonition)
            {
                LetterToErknmType request = new LetterToErknmType
                {
                    Item = new LetterToErknmTypeSet
                    {
                        Items = GetItemsAdmonition(requestData)
                    }
                };

                return ToXElement<LetterToErknmType>(request);
            }

            return null;

        }

        private XElement GetInitiationOrCorrection(ERKNM requestData)
        {
            if (requestData.ERKNMDocumentType == ERKNMDocumentType.Decision)
            {
                LetterToErknmType request = new LetterToErknmType
                {
                    Item = new LetterToErknmTypeSet
                    {
                        Items = GetItems(requestData)
                    }
                };

                return ToXElement<LetterToErknmType>(request);
            }
            else if (requestData.ERKNMDocumentType == ERKNMDocumentType.Admonition)
            {
                LetterToErknmType request = new LetterToErknmType
                {
                    Item = new LetterToErknmTypeSet
                    {
                        Items = GetItemsAdmonition(requestData)
                    }
                };

                return ToXElement<LetterToErknmType>(request);
            }
            else if (requestData.ERKNMDocumentType == ERKNMDocumentType.PreventiveAct)
            {
                LetterToErknmType request = new LetterToErknmType
                {
                    Item = new LetterToErknmTypeSet
                    {
                        Items = GetItemsPreventiveVisit(requestData)
                    }
                };

                return ToXElement<LetterToErknmType>(request);
            }
            return null;
        }

        private object[] GetItemsPreventiveVisit(ERKNM requestData)
        {
            if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
            {
                return GetInitRequestPreventiveVisit(requestData);
            }
            //else if (requestData.GisErpRequestType == ERKNMRequestType.Correction)
            //{
            //    return GetUpdateRequestAdmonition(requestData);
            //}
            else
                return null;
        }

        private CreateInspectionRequestType[] GetInitRequestPreventiveVisit(ERKNM requestData)
        {
            List<CreateInspectionRequestType> reqList = new List<CreateInspectionRequestType>();
            var preventiveVisit = PreventiveVisitDomain.Get(requestData.Disposal.Id);
            var preventiveVisitPeriod = PreventiveVisitPeriodDomain.GetAll().FirstOrDefault(x => x.PreventiveVisit == preventiveVisit);
            TimeSpan time = TimeSpan.Zero;
            DateTime startDate = DateTime.MinValue;
            DateTime stopDate = DateTime.MinValue;
            if (preventiveVisitPeriod != null && preventiveVisitPeriod.DateStart.HasValue && preventiveVisitPeriod.DateEnd.HasValue)
            {
                time = preventiveVisitPeriod.DateEnd.Value - preventiveVisitPeriod.DateStart.Value;
                startDate = preventiveVisitPeriod.DateStart.Value;
                stopDate = preventiveVisitPeriod.DateEnd.Value;
            }
            else
            {
                time = preventiveVisit.DocumentDate.Value.AddMinutes(30) - preventiveVisit.DocumentDate.Value;
                startDate = preventiveVisit.DocumentDate.Value;
                stopDate = preventiveVisit.DocumentDate.Value;
            }

            var days = time.Days + 1;
            var req = new CreateInspectionRequestType
            {
                Inspection = new InspectionCreate
                {
                    publicationDate = DateTime.Now,
                    typeId = GetInspectionType(requestData.ERPInspectionType),
                    startDate = startDate,
                    stopDate = stopDate,
                    stopDateSpecified = true,
                    prosecutorOrganizationId = requestData.ProsecutorOffice.ERKNMCode,
                    districtId = "1033360000000001",
                    noticeDate = preventiveVisit.DocumentDate.HasValue ? preventiveVisit.DocumentDate.Value : DateTime.Now,
                    noticeDateSpecified = preventiveVisit.DocumentDate.HasValue,
                    noticeMethodId = "44",
                    clientTime = DateTime.Now,
                    clientTimeSpecified = true,
                    sendToApprove = false,
                    sendToApproveSpecified = true,
                    Item = new Pm
                    {
                        noteWarning = GetNoteWarningPrevVisit(preventiveVisit),
                        isHasNotification = true,
                        dateOfNotification = preventiveVisit.DocumentDate.HasValue ? preventiveVisit.DocumentDate.Value : DateTime.Now,
                        dateOfNotificationSpecified = preventiveVisit.DocumentDate.HasValue,
                        knoOrganization = new IDictionary
                        {
                            dictId = "AF002594-A4F0-11EB-BCBC-0242AC130002",
                            dictVersionId = "0ae952cd-7ad3-1d97-817a-e19d91ce002a"
                        },
                        supervisionType = new IDictionary
                        {
                            dictId = "1f27d942-a52e-11eb-bcbc-0242ac130002",
                            dictVersionId = preventiveVisit.KindKND == KindKNDGJI.LicenseControl ? "0af4cd2e-78cb-109b-8178-e981360303e8" : "0af4cd2e-78cb-109b-8178-e98132c2037c"
                        },
                        pmType = new IDictionary
                        {
                            dictId = "e8703904-a545-11eb-bcbc-0242ac130002",
                            dictVersionId = "736b18c7-3544-4cbb-9672-29cfd87b5673"
                        },
                        inspectors = GetInspectorsFromDisposal(preventiveVisit.Id),
                        objects = GetObjectsPrevVisit(preventiveVisit),
                        reasons = GetReasonsPrevVisit(),
                        subject = GetSubjectPrevVisit(preventiveVisit),
                        contentWarning = new IDocument
                        {
                            documentTypeId = "120",
                            description = "акт профвизита",
                            guid = requestData.RESULT_GUID
                        }

                    }

                }
            };
            reqList.Add(req);
            return reqList.ToArray();
        }

        private ISubject GetSubjectPrevVisit(PreventiveVisit pv)
        {
            var cg = pv.Contragent;
            switch (pv.PersonInspection)
            {
                case PersonInspection.Organization:
                    return new ISubject
                    {
                        inn = cg.Inn,
                        ogrn = cg.Ogrn,
                        organizationName = cg.Name
                    };
                case PersonInspection.Official:
                    if (string.IsNullOrEmpty(pv.PhysicalPersonINN))
                    {
                        return new ISubject
                        {
                            isFiz = true,
                            isFizSpecified = true,
                            organizationName = pv.PhysicalPerson
                        };
                    }
                    return new ISubject
                    {
                        isFiz = true,
                        isFizSpecified = true,
                        inn = pv.PhysicalPersonINN,
                        organizationName = pv.PhysicalPerson
                    };
                case PersonInspection.PhysPerson:
                    if (string.IsNullOrEmpty(pv.PhysicalPersonINN))
                    {
                        return new ISubject
                        {
                            isFiz = true,
                            isFizSpecified = true,
                            organizationName = pv.PhysicalPerson
                        };
                    }
                    return new ISubject
                    {
                        isFiz = true,
                        isFizSpecified = true,
                        inn = pv.PhysicalPersonINN,
                        organizationName = pv.PhysicalPerson
                    };
                default:
                    return new ISubject
                    {
                        inn = cg.Inn,
                        ogrn = cg.Ogrn,
                        organizationName = cg.Name
                    };

            }
        }

        private ReasonWithRisk[] GetReasonsPrevVisit()
        {

            List<ReasonWithRisk> subList = new List<ReasonWithRisk>();
            subList.Add(new ReasonWithRisk
            {
                reason = new IReason
                {
                    reasonTypeId = "301",
                    note = "(ФЗ 248) Наличие у контрольного (надзорного) органа сведений о готовящихся или возможных нарушениях обязательных требований, а также о непосредственных нарушениях обязательных требований, если указанные сведения не соответствуют утвержденным индикаторам риска нарушения обязательных требований"
                }
            });
            return subList.ToArray();
        }

        private IObject[] GetObjectsPrevVisit(PreventiveVisit pv)
        {
            List<IObject> subList = new List<IObject>();
            var ro = PreventiveVisitRealityObjectDomain.GetAll().FirstOrDefault(x => x.PreventiveVisit == pv);
            if (ro != null)
            {
                subList.Add(new IObject
                {
                    address = ro.RealityObject.FiasAddress.AddressName,                  
                    objectType = new IDictionary
                    {
                        dictId = "641d3956-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0af4cd2e-78cb-109b-8178-ea2e8d46040d"
                    },
                    objectKind = new IDictionary
                    {
                        dictId = "641d3bb8-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0ae953e3-7c84-1ce2-817c-a2908a753d2c"
                    },
                    objectSubKind = new IDictionary
                    {
                        dictId = "641d3bb8-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0ae953e3-7c84-1ce2-817c-a2908a753d2c"
                    }
                });
            }
            return subList.ToArray();
        }

        private string GetNoteWarningPrevVisit(PreventiveVisit pv)
        {
            var ro = PreventiveVisitRealityObjectDomain.GetAll().FirstOrDefault(x => x.PreventiveVisit == pv);
            string note = "по адресу: ";
            if (ro != null)
            {
                note += ro.RealityObject.FiasAddress.AddressName;
                note += " ";
            }
            else
            {
                note += "адрес не указан ";
            }
            var violations = PreventiveVisitResultViolationDomain.GetAll().Where(x => x.PreventiveVisitResult.PreventiveVisit.Id == pv.Id).ToList();
            if (violations.Count == 0)
            {
                note += "нарушения не указаны";
            }
            foreach (var viol in violations)
            {
                note += $"{viol.ViolationGji.Name} ";
            }
            return note;
        }

        private object[] GetItemsAdmonition(ERKNM requestData)
        {
            if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
            {
                return GetInitRequestAdmonition(requestData);
            }
            else if (requestData.GisErpRequestType == ERKNMRequestType.Correction)
            {
                return GetUpdateRequestAdmonition(requestData);
            }
            else
                return null;
        }

        private UpdateInspectionRequestType[] GetUpdateRequestAdmonition(ERKNM requestData)
        {
            List<UpdateInspectionRequestType> reqList = new List<UpdateInspectionRequestType>();
            var admonition = AppealCitsAdmonitionDomain.Get(requestData.AppealCitsAdmonition.Id);
            var cg = admonition.Contragent;
            TimeSpan time = admonition.PerfomanceDate.Value - admonition.DocumentDate.Value;
            var days = time.Days + 1;
            var req = new UpdateInspectionRequestType
            {
                iGuid = requestData.OBJECT_GUID,
                Items = GetAdmonitionUpdateItems(requestData)

            };
            reqList.Add(req);
            return reqList.ToArray();
        }

        private IDocumentAttachmentsInsert[] GetAdmonitionUpdateItems(ERKNM requestData)
        {
            List<IDocumentAttachmentsInsert> updateList = new List<IDocumentAttachmentsInsert>();
            List<IAttachment> attList = new List<IAttachment>();
            FileAttachment att = null;
            string filguid = Guid.NewGuid().ToString();
            var admonFile = AppealCitsAdmonitionDomain.Get(requestData.AppealCitsAdmonition.Id)?.SignedFile;
            string contentId = $"_{admonFile.Id}.{admonFile.Extention}";
            if (admonFile != null)
            {
                att = new FileAttachment
                {
                    FileData = _fileManager.GetFile(admonFile).ReadAllBytes(),
                    //    FileGuid = filguid,
                    FileName = contentId
                };
                RequestAttachments.Add(att);
                attList.Add(new IAttachment
                {
                    fileName = contentId,
                    guid = filguid
                });
                updateList.Add(new IDocumentAttachmentsInsert
                {
                    IDocumentGuid = requestData.RESULT_GUID,
                    fileName = contentId,
                    guid = filguid
                });
            }
            //FileAttachment

            return updateList.ToArray();
        }

        private CreateInspectionRequestType[] GetInitRequestAdmonition(ERKNM requestData)
        {
            List<CreateInspectionRequestType> reqList = new List<CreateInspectionRequestType>();
            var admonition = AppealCitsAdmonitionDomain.Get(requestData.AppealCitsAdmonition.Id);
            var cg = admonition.Contragent;
            TimeSpan time = admonition.PerfomanceDate.Value - admonition.DocumentDate.Value;
            var days = time.Days + 1;
            var req = new CreateInspectionRequestType
            {
                Inspection = new InspectionCreate
                {
                    publicationDate = DateTime.Now,
                    typeId = GetInspectionType(requestData.ERPInspectionType),
                    startDate = admonition.DocumentDate.Value,
                    stopDate = admonition.PerfomanceDate.HasValue ? admonition.PerfomanceDate.Value : DateTime.Now,
                    stopDateSpecified = admonition.PerfomanceDate.HasValue,
                    prosecutorOrganizationId = requestData.ProsecutorOffice.ERKNMCode,
                    districtId = "1030200000000001",
                    noticeDate = admonition.DocumentDate.HasValue ? admonition.DocumentDate.Value : DateTime.Now,
                    noticeDateSpecified = admonition.DocumentDate.HasValue,
                    noticeMethodId = "44",
                    clientTime = DateTime.Now,
                    clientTimeSpecified = true,
                    sendToApprove = false,
                    sendToApproveSpecified = true,
                    Item = new Pm
                    {
                        noteWarning = GetNoteWarning(admonition),
                        isHasNotification = true,
                        dateOfNotification = admonition.DocumentDate.HasValue ? admonition.DocumentDate.Value : DateTime.Now,
                        dateOfNotificationSpecified = admonition.DocumentDate.HasValue,
                        knoOrganization = new IDictionary
                        {
                            dictId = "AF002594-A4F0-11EB-BCBC-0242AC130002",
                            dictVersionId = "0ae952cd-7ad3-1d97-817a-e19d91ce002a"
                        },
                        supervisionType = new IDictionary
                        {
                            dictId = "1f27d942-a52e-11eb-bcbc-0242ac130002",
                            dictVersionId = admonition.KindKND == KindKND.LicenseControl ? "0af4cd2e-78cb-109b-8178-e981360303e8" : "0af4cd2e-78cb-109b-8178-e98132c2037c"
                        },
                        pmType = new IDictionary
                        {
                            dictId = "e8703904-a545-11eb-bcbc-0242ac130002",
                            dictVersionId = "e8c7c3a3-b8b8-475f-925a-5b913a79cb6f"
                        },
                        inspectors = GetInspectorsFromAdmonition(admonition),
                        objects = GetObjects(admonition),
                        reasons = GetReasons(admonition),
                        subject = GetSubject(admonition),
                        contentWarning = new IDocument
                        {
                            documentTypeId = "120",
                            description = "Файл предостережения",
                            guid = requestData.RESULT_GUID
                        }

                    }

                }
            };
            reqList.Add(req);
            return reqList.ToArray();
        }

        private ISubject GetSubject(AppealCitsAdmonition admon)
        {
            var cg = admon.Contragent;
            switch (admon.PayerType)
            {
                case PayerType.Juridical:
                    return new ISubject
                    {
                        inn = cg.Inn,
                        ogrn = cg.Ogrn,
                        organizationName = cg.Name
                    };
                case PayerType.IP:
                    return new ISubject
                    {
                        inn = cg.Inn,
                        ogrn = cg.Ogrn,
                        organizationName = cg.Name
                    };
                case PayerType.Physical:
                    if (string.IsNullOrEmpty(admon.FizINN))
                    {
                        return new ISubject
                        {
                            isFiz = true,
                            isFizSpecified = true,
                            organizationName = admon.FIO
                        };
                    }
                    return new ISubject
                    {
                        isFiz = true,
                        isFizSpecified = true,
                        inn = admon.FizINN,
                        organizationName = admon.FIO
                    };
                default:
                    return new ISubject
                    {
                        inn = cg.Inn,
                        ogrn = cg.Ogrn,
                        organizationName = cg.Name
                    };

            }
        }
        private ReasonWithRisk[] GetReasons(AppealCitsAdmonition admon)
        {

            List<ReasonWithRisk> subList = new List<ReasonWithRisk>();
            subList.Add(new ReasonWithRisk
            {
                reason = new IReason
                {
                    reasonTypeId = admon.InspectionReasonERKNM.ERKNMId,
                    note = admon.InspectionReasonERKNM.Name
                }
            });
            return subList.ToArray();
        }
        private IObject[] GetObjects(AppealCitsAdmonition admon)
        {
            List<IObject> subList = new List<IObject>();
            if (admon.RealityObject != null)
            {
                subList.Add(new IObject
                {
                    address = admon.RealityObject.FiasAddress.AddressName,
                    objectType = new IDictionary
                    {
                        dictId = "641d3956-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0af4cd2e-78cb-109b-8178-ea2e8d46040d"
                    },
                    objectKind = new IDictionary
                    {
                        dictId = "641d3bb8-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0ae953e3-7c84-1ce2-817c-a2908a753d2c"
                    },
                    objectSubKind = new IDictionary
                    {
                        dictId = "641d3bb8-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0ae953e3-7c84-1ce2-817c-a2908a753d2c"
                    }
                });
            }
            return subList.ToArray();
        }
        private IInspector[] GetInspectorsFromAdmonition(AppealCitsAdmonition adm)
        {
            List<IInspector> newElement = new List<IInspector>();

            newElement.Add(new IInspector
            {
                fullName = adm.Executor.Fio,
                position = new IDictionary
                {
                    dictId = "003d7ce8-c474-11eb-8529-0242ac130003",
                    dictVersionId = adm.Executor.ERKNMPositionGuid
                }
            });
            return newElement.ToArray();
        }
        public IDomainService<AppCitAdmonAppeal> AppCitAdmonAppealDomain { get; set; }
        private string GetNoteWarning(AppealCitsAdmonition admon)
        {
            var appcit = AppCitAdmonAppealDomain.GetAll().Where(x => x.AppealCitsAdmonition.Id == admon.Id).AsEnumerable()
                .AggregateWithSeparator(x => x.AppealCits.NumberGji + " от " + x.AppealCits.DateFrom.Value.ToString("dd.MM.yyyy"), ";");
            string note = $"В обращении (обращениях) {appcit} заявитель сообщил о нарушении {admon.Contragent?.Name} по адресу: ";
            if (admon.RealityObject != null)
            {
                note += admon.RealityObject.FiasAddress.AddressName;
                note += " ";
            }
            else
            {
                note += "адрес не указан ";
            }
            var violations = AppCitAdmonVoilationDomain.GetAll().Where(x => x.AppealCitsAdmonition.Id == admon.Id).ToList();
            if (violations.Count == 0)
            {
                note += "нарушения не указаны";
            }
            foreach (var viol in violations)
            {
                note += $"{viol.ViolationGji.Name} ";
            }
            return note;
        }

        private XElement GetSecondRequestXML(ERKNM requestData, CreateInspectionResponseType resp)
        {
            //SPV_26 - жилищный надзор
            //SPV_2 - лицензионный контроль
            LetterToErknmType request = new LetterToErknmType
            {
                Item = new LetterToErknmTypeSet
                {
                    Items = GetUpdateInspectionItems(requestData, resp)
                }
            };

            return ToXElement<LetterToErknmType>(request);
        }

        private object[] GetItems(ERKNM requestData)
        {
            if (requestData.GisErpRequestType == ERKNMRequestType.Initialization)
            {
                return GetInitRequest(requestData);
            }
            else
                return null;
        }

        private UpdateInspectionRequestType[] GetUpdateInspectionItems(ERKNM requestData, CreateInspectionResponseType resp)
        {
            List<UpdateInspectionRequestType> reqList = new List<UpdateInspectionRequestType>();

            var req = new UpdateInspectionRequestType
            {
                iGuid = resp.Inspection.iGuid,
                Items = GetUpdateItems(requestData, resp, 1)
            };
            reqList.Add(req);
            reqList.Add(new UpdateInspectionRequestType
            {
                iGuid = resp.Inspection.iGuid,
                Items = GetUpdateItems(requestData, resp, 2)
            });
            return reqList.ToArray();
        }
        private object[] GetUpdateItems(ERKNM requestData, CreateInspectionResponseType resp, int i)
        {
            //мероприятия проверки
            List<object> itemsList = new List<object>();
            if (i == 1)
            {
                var decCM = DecisionControlMeasuresDomain.GetAll()
                 .Where(x => x.Decision.Id == requestData.Disposal.Id).ToList();
                foreach (var cm in decCM)
                {
                    if (!string.IsNullOrEmpty(cm.ControlActivity.ERKNMGuid))
                    {
                        itemsList.Add(new IEventsInsert
                        {
                            startDate = cm.DateStart.Value,
                            startDateSpecified = true,
                            stopDate = cm.DateEnd.Value,
                            stopDateSpecified = true,
                            @event = new IDictionary
                            {
                                dictId = "f0f9a79a-a5b3-11eb-bcbc-0242ac130002",
                                dictVersionId = cm.ControlActivity.ERKNMGuid
                            }
                        });
                    }
                }
            }
            if (i == 2)
            {
                //IRequirementsInsert
                var requirements = DecisionVerificationSubjectDomain.GetAll()
                    .Where(x => x.Decision.Id == requestData.Disposal.Id).ToList();
                foreach (var rec in requirements)
                {
                    if (!string.IsNullOrEmpty(rec.SurveySubject.Formulation))
                    {
                        itemsList.Add(new IRequirementsInsert
                        {
                            requirement = new IDictionary
                            {
                                dictId = "a43f056e-b95c-11eb-8529-0242ac130003",
                                dictVersionId = rec.SurveySubject.Formulation
                            }
                        });
                    }
                }
            }

            return itemsList.ToArray();
        }
        private CreateInspectionRequestType[] GetInitRequest(ERKNM requestData)
        {
            List<CreateInspectionRequestType> reqList = new List<CreateInspectionRequestType>();
            var dec = DecisionDomain.Get(requestData.Disposal.Id);
            TimeSpan time = dec.DateEnd.Value - dec.DateStart.Value;
            var hours = 1;
            if (dec.TimeVisitStart.HasValue && dec.TimeVisitEnd.HasValue)
            {
                var hoursCalc = dec.TimeVisitEnd.Value.Hour - dec.TimeVisitStart.Value.Hour;
                if (hoursCalc > 0)
                {
                    hours = hoursCalc;
                }
            }
            var days = time.Days + 1;
            var req = new CreateInspectionRequestType
            {
                Inspection = new InspectionCreate
                {
                    publicationDate = DateTime.Now,
                    typeId = GetInspectionType(requestData.ERPInspectionType),
                    startDate = dec.DateStart.Value,
                    stopDate = dec.DateEnd.HasValue ? dec.DateEnd.Value : DateTime.Now,
                    stopDateSpecified = dec.DateEnd.HasValue,
                    prosecutorOrganizationId = requestData.ProsecutorOffice.ERKNMCode,
                    districtId = "1030200000000001",
                    noticeDate = dec.NcDate.HasValue ? dec.NcDate.Value : dec.DocumentDate.Value,
                    noticeDateSpecified = dec.NcDate.HasValue,
                    noticeMethodId = "44",
                    clientTime = DateTime.Now,
                    clientTimeSpecified = true,
                    sendToApprove = dec.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement,
                    sendToApproveSpecified = true,
                    Item = new I
                    {
                        isAppealed = false,
                        durationDays = days.ToString(),
                        durationHours = dec.TimeVisitStart.HasValue ? hours.ToString() : "0",
                        isChecklistsUsed = GetCheckListUsed(dec.Id),
                        decision = new IDecision
                        {
                            dateTimeDecision = new DateTime(dec.DocumentDate.Value.Year, dec.DocumentDate.Value.Month, dec.DocumentDate.Value.Day,
                            dec.HourOfProceedings.HasValue ? dec.HourOfProceedings.Value : 12, dec.MinuteOfProceedings.HasValue ? dec.MinuteOfProceedings.Value : 0, 0),
                            dateTimeDecisionSpecified = true,
                            fioSigner = dec.IssuedDisposal.Fio,
                            numberDecision = dec.DocumentNumber,
                            placeDecision = "394018, г. Воронеж, ул. Кирова, д. 6а",
                            titleSigner = new IDictionary
                            {
                                dictId = "b6fc9844-a528-11eb-bcbc-0242ac130002",
                                dictVersionId = dec.IssuedDisposal.ERKNMTitleSignerGuid
                            }
                        },
                        approve = GetIAprove(dec),
                        organizations = GetSubjects(dec),
                        inspectors = GetInspectorsFromDisposal(dec.Id),
                        places = GetPlaces(dec),
                        objects = GetObjects(dec),
                        organizationDocuments = GetProvidedDocGji(dec),
                        reasons = GetReasons(dec, requestData.InspectionName),
                        kindControl = new IDictionary
                        {
                            dictId = "1F27D942-A52E-11EB-BCBC-0242AC130002",
                            dictVersionId = requestData.KindKND == KindKND.LicenseControl ? "0af4cd2e-78cb-109b-8178-e981360303e8" : "0af4cd2e-78cb-109b-8178-e98132c2037c"
                        },
                        kind = new IDictionary
                        {
                            dictId = "D2EC803A-A53E-11EB-BCBC-0242AC130002",
                            dictVersionId = GetKind(dec)
                        },
                        //                        kindControl = GetTypeControl(dec.KindKNDGJI)
                        knoOrganization = new IDictionary
                        {
                            dictId = "AF002594-A4F0-11EB-BCBC-0242AC130002",
                            dictVersionId = "0ae952cd-7ad3-1d97-817a-e19d91ce002a"
                        }
                    }

                }
            };
            reqList.Add(req);
            return reqList.ToArray();
        }

        private string GetKind(Decision dec)
        {

            //Инспекционный визит 48793256-fd08-4fd2-a14c-2c95e4b86bff
            //Документарная проверка 20753bee-3a33-4647-9181-2f6f30c87288
            //Выездная проверка 97fd2739-d336-4813-bebb-b363f743545a
            //Наблюдение за соблюдением обязательных требований (предостережение) e29b8808-a81e-4169-8a92-b68bf552a123
            // Выездное обследование 49c2268e - 56af - 422f - af5a - 80b31d70afd8
            switch (dec.KindCheck.Code)
            {
                case TypeCheck.InspectVisit:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130033";
                case TypeCheck.NotPlannedDocumentation:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130053";
                case TypeCheck.NotPlannedDocumentationExit:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130053";
                case TypeCheck.PlannedDocumentation:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130053";
                case TypeCheck.PlannedDocumentationExit:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130053";
                case TypeCheck.PlannedExit:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130063";
                case TypeCheck.NotPlannedExit:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130063";
                default:
                    return "d2ec803a-a53e-11eb-bcbc-0242ac130063";
            }
        }

        private IObject[] GetObjects(Decision dec)
        {
            var cg = dec.Inspection.Contragent;
            List<IObject> subList = new List<IObject>();
            var inspectionPlaceDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            inspectionPlaceDomain.GetAll().Where(x => x.Inspection == dec.Inspection).Select(x => x.RealityObject).ToList().ForEach(x =>
            {
                subList.Add(new IObject
                {
                    address = $"{x.Municipality.Name}, {x.Address}",
                    objectType = new IDictionary
                    {
                        dictId = "641d3956-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0af4cd2e-78cb-109b-8178-ea2e8d46040d"
                    },
                    objectKind = new IDictionary
                    {
                        dictId = "641d3bb8-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0ae95208-7c84-1d7f-817d-089cebf506c8"
                    },
                    objectSubKind = new IDictionary
                    {
                        dictId = "641d3bb8-a5b1-11eb-bcbc-0242ac130002",
                        dictVersionId = "0ae95208-7c84-1d7f-817d-089cebf506c8"
                    },
                });
            });

            return subList.ToArray();
        }

        private ReasonWithRisk[] GetReasons(Decision dec, string inspName)
        {
            var cg = dec.Inspection.Contragent;
            List<ReasonWithRisk> subList = new List<ReasonWithRisk>();
            if (dec.TypeDisposal == TypeDisposalGji.Base)
            {
                if (dec.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    subList.Add(new ReasonWithRisk
                    {
                        reason = new IReason
                        {
                            //reasonTypeId = "205",
                            reasonTypeId = "316",
                            note = inspName
                        }
                    });
                }
                if (dec.Inspection.TypeBase == TypeBase.PlanAction)
                {
                    subList.Add(new ReasonWithRisk
                    {
                        reason = new IReason
                        {
                            reasonTypeId = "208",
                            note = $"План мероприятий на {DateTime.Now.Year} год"
                        }
                    });
                }
            }
            else if (dec.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                subList.Add(new ReasonWithRisk
                {
                    reason = new IReason
                    {
                        reasonTypeId = "314",
                        note = inspName
                    }
                });
            }

            return subList.ToArray();
        }
        private IString[] GetProvidedDocGji(Decision dec)
        {
            List<IString> subList = new List<IString>();

            var provdocPlaceDomain = this.Container.Resolve<IDomainService<DecisionProvidedDoc>>();

            provdocPlaceDomain.GetAll().Where(x => x.Decision == dec).Select(x => x.ProvidedDoc).ToList().ForEach(x =>
            {
                subList.Add(new IString
                {
                    value = $"{x.Name}"
                });
            });
            return subList.ToArray();
        }

        private IString[] GetPlaces(Decision dec)
        {
            List<IString> subList = new List<IString>();
            //subList.Add(new IString
            //{
            //    value = dec.Inspection.Contragent.JuridicalAddress
            //});
            var inspectionPlaceDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            inspectionPlaceDomain.GetAll().Where(x => x.Inspection == dec.Inspection).Select(x => x.RealityObject).ToList().ForEach(x =>
            {
                subList.Add(new IString
                {
                    value = $"{x.Municipality.Name}, {x.Address}"
                });
            });
            return subList.ToArray();
        }
        public IDomainService<DecisionControlSubjects> DecisionControlSubjectsDomain { get; set; }
        private ISubject[] GetSubjects(Decision dec)
        {
            var recCS = DecisionControlSubjectsDomain.GetAll().Where(x => x.Decision == dec).ToList();
            List<ISubject> subList = new List<ISubject>();
            foreach (var subject in recCS)
            {
                switch (subject.PersonInspection)
                {
                    case PersonInspection.Organization:
                        subList.Add(new ISubject
                        {
                            inn = subject.Contragent.Inn,
                            ogrn = subject.Contragent.Ogrn,
                            organizationName = subject.Contragent.Name
                        });
                        break;
                    case PersonInspection.PhysPerson:
                        subList.Add(new ISubject
                        {
                            isFiz = true,
                            inn = subject.PhysicalPersonINN,
                            isFizSpecified = true,
                            organizationName = subject.PhysicalPerson
                        });
                        break;
                    case PersonInspection.Official:
                        subList.Add(new ISubject
                        {
                            isFiz = true,
                            isFizSpecified = true,
                            inn = subject.PhysicalPersonINN,
                            organizationName = subject.PhysicalPerson
                        });
                        break;
                }
            }
            if (subList.Count == 0)
            {
                var cg = dec.Inspection.Contragent;

                subList.Add(new ISubject
                {
                    inn = cg.Inn,
                    ogrn = cg.Ogrn,
                    organizationName = cg.Name
                });
            }
            return subList.ToArray();
        }

        private IApprove GetIAprove(Decision dec)
        {

            if (dec.TypeAgreementResult == TypeAgreementResult.Agreed)
            {
                return new IApprove
                {
                    approveRequiredId = dec.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement ? "1" : "2",
                    dateDecision = dec.ProcAprooveDate.Value,
                    dateDecisionSpecified = dec.ProcAprooveDate.HasValue,
                    numberDecision = dec.ProcAprooveNum,
                    fioSigner = dec.FioProcAproove,
                    titleSigner = dec.PositionProcAproove
                };
            }
            if (dec.TypeAgreementResult == TypeAgreementResult.NotAgreed)
            {
                return new IApprove
                {
                    approveRequiredId = dec.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement ? "1" : "2",
                    dateDecision = dec.ProcAprooveDate.Value,
                    dateDecisionSpecified = dec.ProcAprooveDate.HasValue,
                    numberDecision = dec.ProcAprooveNum,
                    fioSigner = dec.FioProcAproove,
                    titleSigner = dec.PositionProcAproove,
                    dateRejectReason = dec.ProcAprooveDate.Value,
                };
            }
            return new IApprove
            {
                approveRequiredId = dec.TypeAgreementProsecutor == TypeAgreementProsecutor.RequiresAgreement ? "1" : "2",
            };
        }      

        private bool GetCheckListUsed(long decId)
        {
            var cnt = DecisionControlListDomain.GetAll().Where(x => x.Decision.Id == decId).Count();
            return cnt > 0;
        }
        private string GetInspectionType(ERPInspectionType type)
        {
            switch (type)
            {
                case ERPInspectionType.VP:
                    return "5";//Внеплановое КНМ
                default:
                    return "4";//Плановое КНМ
            }
        }

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private T Deserialize<T>(XElement element)
        where T : class
        {
            XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }


        private List<XElement> GetCorrection(ERKNM requestData)
        {
            //SPV_26 - жилищный надзор
            //SPV_2 - лицензионный контроль
            List<XElement> correctionElements = new List<XElement>();
            string iTypeId = string.Empty;
            string iNoticeTypeId = string.Empty;
            string IRISK_ID = string.Empty;
            string iSupervisionId = string.Empty;
            string frguServIdBk = string.Empty;
            string addressTypeId = string.Empty;
            string IREASON_ID = string.Empty;

            switch (requestData.ERPAddressType)
            {
                case ERPAddressType.TYPE_I: { addressTypeId = "1"; } break;
                case ERPAddressType.TYPE_II: { addressTypeId = "2"; } break;
                case ERPAddressType.TYPE_III: { addressTypeId = "3"; } break;
                case ERPAddressType.TYPE_OTHER: { addressTypeId = "44"; } break;
            }

            Disposal disp = DisposalDomain.Get(requestData.Disposal.Id);

            string iCarryoutTypeId = string.Empty;
            if (disp.KindCheck.Code == TypeCheck.InspectionSurvey || disp.KindCheck.Code == TypeCheck.NotPlannedExit || disp.KindCheck.Code == TypeCheck.PlannedExit || disp.KindCheck.Code == TypeCheck.VisualSurvey)
            {
                iCarryoutTypeId = "1";
            }
            else if (disp.KindCheck.Code == TypeCheck.NotPlannedDocumentation || disp.KindCheck.Code == TypeCheck.PlannedDocumentation)
            {
                iCarryoutTypeId = "2";
            }
            else if (disp.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit || disp.KindCheck.Code == TypeCheck.PlannedDocumentationExit)
            {
                iCarryoutTypeId = "3";
            }

            var actCheckDoc = DocumentGjiChildrenDomain.GetAll()
            .Where(x => x.Parent.Id == disp.Id)
            .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
            .Select(x => x.Children)
            .FirstOrDefault();

            //  var actCheck = ActCheckDomain.Get(actCheckDoc.Id);
            string ACT_DATE_CREATE = GetActCreateDate(actCheckDoc);// actCheckDoc.DocumentDate.HasValue ? actCheckDoc.DocumentDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : actCheckDoc.ObjectCreateDate.ToString("yyyy-MM-ddTHH:mm:ss");
            if (string.IsNullOrEmpty(ACT_DATE_CREATE))
            {
                //if (disp.ObjectVisitEnd.HasValue && disp.TimeVisitEnd.HasValue)
                //{
                //    DateTime dd = disp.ObjectVisitEnd.Value;
                //    DateTime dt = disp.TimeVisitEnd.Value;
                //    DateTime disposalActDate = new DateTime(dd.Year, dd.Month, dd.Day, dt.Hour, dt.Minute, 0);
                //    ACT_DATE_CREATE = disposalActDate.ToString("yyyy-MM-ddTHH:mm:ss");
                //}
                ACT_DATE_CREATE = actCheckDoc.DocumentDate.HasValue ? actCheckDoc.DocumentDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : actCheckDoc.ObjectCreateDate.ToString("yyyy-MM-ddTHH:mm:ss");
            }
            string address = "";
            if (iCarryoutTypeId == "2")
            {
                address = "394018, г. Воронеж, ул. Кирова, д. 6а";
                addressTypeId = "44";
            }
            else
            {
                address = requestData.SubjectAddress;
            }
            string ACT_ADDRESS = address;


            var actCheckWitness = ActCheckWitnessDomain.GetAll()
                      .Where(x => x.ActCheck.Id == actCheckDoc.Id).FirstOrDefault();


            XElement newElem = new XElement(erp_typesNamespace + "UpdateIResult",
                new XAttribute("numGuid", requestData.RESULT_GUID),
                new XAttribute("actDateCreate", ACT_DATE_CREATE),
                new XAttribute("actAddress", ACT_ADDRESS),
                new XAttribute("actAddressTypeId", addressTypeId));
            if (actCheckWitness != null)
            {
                if (actCheckWitness.IsFamiliar)
                {
                    newElem.Add(new XAttribute("actWasNotRead", false));
                }
                else
                {
                    newElem.Add(new XAttribute("actWasNotRead", true));
                }
            }
            else
            {
                var actRemovalWitness = ActRemovalWitnessDomain.GetAll()
                      .Where(x => x.ActRemoval.Id == actCheckDoc.Id).FirstOrDefault();
                if (actRemovalWitness != null)
                {
                    if (actRemovalWitness.IsFamiliar)
                    {
                        newElem.Add(new XAttribute("actWasNotRead", false));
                    }
                    else
                    {
                        newElem.Add(new XAttribute("actWasNotRead", true));
                    }
                }
            }


            if (disp.DateStart.HasValue && disp.DateEnd.HasValue)
            {
                try
                {
                    int DURATION_DAYS = 0;
                    var convertedDate = disp.DateEnd.Value.Subtract(disp.DateStart.Value).Days + 1;
                    string START_DATE = disp.DateStart.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                            .Where(x => x.ProdDate >= disp.DateStart.Value && x.ProdDate <= disp.DateEnd.Value).Select(x => x.ProdDate).ToList();
                    for (int i = 0; i < convertedDate; i++)
                    {
                        var checkedDate = disp.DateStart.Value.AddDays(i);
                        if (prodCalendarContainer.Contains(checkedDate))
                        {
                            continue;
                        }
                        if (checkedDate.DayOfWeek == DayOfWeek.Saturday || checkedDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            continue;
                        }
                        DURATION_DAYS++;
                    }
                    if (DURATION_DAYS <= 0)
                    {
                        DURATION_DAYS = 1;
                    }

                    newElem.Add(new XAttribute("startDate", START_DATE));
                    newElem.Add(new XAttribute("durationDays", DURATION_DAYS));
                }
                catch { }
            }
            XElement updateInspectionResult = new XElement(erp_typesNamespace + "UpdateInspection",
                  new XAttribute("guid", requestData.INSPECTION_GUID), newElem);

            correctionElements.Add(updateInspectionResult);

            //InsertIResultInspector
            XElement insertIResultInspector = new XElement(erp_typesNamespace + "UpdateInspection",
                new XAttribute("guid", requestData.INSPECTION_GUID), GetCorrectionInspectorsResultFromDisposal(disp, requestData.RESULT_GUID));

            correctionElements.Add(insertIResultInspector);

            //InsertIViolation
            var actcheckRealityObject = ActCheckRealityObjectDomain.GetAll()
                      .Where(x => x.ActCheck.Id == actCheckDoc.Id).FirstOrDefault();
            List<XElement> violationList = new List<XElement>();
            if (actcheckRealityObject != null)
            {
                if (actcheckRealityObject.HaveViolation == YesNoNotSet.Yes)
                {
                    var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                               .Where(x => x.Inspection == disp.Inspection).ToList();
                    if (inspViolations.Count > 0)
                    {

                        foreach (InspectionGjiViol viol in inspViolations)
                        {
                            string VIOLATION_NOTE = viol.Violation.Name;
                            string NOTE = ViolationActionsRemovGjiDomain.GetAll()
                                .Where(x => x.ViolationGji == viol.Violation)
                                .AggregateWithSeparator(x => x.ActionsRemovViol.Name, ",");
                            string IVIOLATION_TYPE_ID = "1";
                            XElement violElem = new XElement(erp_typesNamespace + "InsertIViolation",
                             new XAttribute("rGuid", requestData.RESULT_GUID),
                             new XAttribute("violationNote", VIOLATION_NOTE),
                             new XAttribute("violationAct", viol.Violation.NormativeDocNames),
                             new XAttribute("iViolationTypeId", IVIOLATION_TYPE_ID)
                                 );
                            //var inspViolationStages = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                            //             .Where(x => x.InspectionViolation == viol).ToList();
                            //foreach (InspectionGjiViolStage stage in inspViolationStages)
                            //{
                            //    if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                            //    {
                            //        string CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                            //        string DATE_APPOINTMENT = stage.Document.DocumentDate.HasValue ? stage.Document.DocumentDate.Value.ToString("yyyy-MM-dd") : stage.Document.ObjectCreateDate.ToString("yyyy-MM-dd");
                            //        XElement presctElement = new XElement(erp_typesNamespace + "VInjunction",
                            //              new XAttribute("code", CODE),
                            //               new XAttribute("note", NOTE),
                            //               new XAttribute("dateAppointment", DATE_APPOINTMENT),
                            //               stage.DateFactRemoval.HasValue ? new XAttribute("executionNote", "Выполнено") : null,
                            //                stage.DatePlanRemoval.HasValue ? new XAttribute("executionDeadline", stage.DatePlanRemoval.Value.ToString("yyyy-MM-dd")) : null
                            //            );
                            //        violElem.Add(presctElement);
                            //        //newReault.CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                            //        //if (stage.Document.DocumentDate.HasValue)
                            //        //    newReault.DATE_APPOINTMENT = stage.Document.DocumentDate.Value;
                            //        //if (viol.DateFactRemoval != null)
                            //        //{
                            //        //    newReault.EXECUTION_NOTE = "Выполнено";
                            //        //}
                            //        //newReault.EXECUTION_DEADLINE = viol.DatePlanRemoval;
                            //    }
                            //}
                            violationList.Add(violElem);

                        }

                    }
                }
            }
            else if (requestData.HasViolations == YesNoNotSet.Yes)
            {
                var actRemoval = ActRemovalDomain.GetAll()
                    .Where(x => x.Id == actCheckDoc.Id).FirstOrDefault();
                if (actRemoval != null)
                {
                    var actRemovalViolations = ActRemovalViolationDomain.GetAll()
                        .Where(x => x.Document.Id == actRemoval.Id).ToList();
                    foreach (ActRemovalViolation viol in actRemovalViolations)
                    {
                        string VIOLATION_NOTE = viol.InspectionViolation.Violation.Name;
                        string NOTE = ViolationActionsRemovGjiDomain.GetAll()
                            .Where(x => x.ViolationGji == viol.InspectionViolation.Violation)
                            .AggregateWithSeparator(x => x.ActionsRemovViol.Name, ",");
                        string IVIOLATION_TYPE_ID = "1";
                        XElement violElem = new XElement(erp_typesNamespace + "InsertIViolation",
                         new XAttribute("rGuid", requestData.RESULT_GUID),
                         new XAttribute("violationNote", VIOLATION_NOTE),
                         new XAttribute("violationAct", viol.InspectionViolation.Violation.NormativeDocNames),
                         new XAttribute("iViolationTypeId", IVIOLATION_TYPE_ID)
                             );
                        violationList.Add(violElem);
                    }

                }
            }
            if (violationList.Count > 0)
            {
                foreach (XElement vilationElement in violationList)
                {
                    XElement insertIViolation = new XElement(erp_typesNamespace + "UpdateInspection",
                      new XAttribute("guid", requestData.INSPECTION_GUID), vilationElement);
                    correctionElements.Add(insertIViolation);
                }

            }
            return correctionElements;
        }


        #endregion

        #region Private methods
        private string GetActCreateDate(DocumentGji act)
        {
            if (act != null)
            {
                if (act.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    var actCheck = ActCheckDomain.Get(act.Id);
                    var actCheckPeriod = ActCheckPeriodDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id).FirstOrDefault();
                    if (actCheckPeriod != null)
                    {
                        if (actCheckPeriod.DateStart.HasValue)
                        {
                            DateTime dateStart;
                            DateTime.TryParse(actCheckPeriod.TimeEnd, out dateStart);
                            DateTime actD = actCheckPeriod.DateStart.Value;
                            DateTime actDate = new DateTime(actD.Year, actD.Month, actD.Day, dateStart.Hour, dateStart.Minute, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else if (actCheckPeriod.DateCheck.HasValue && actCheck.DocumentTime.HasValue)
                        {
                            DateTime actD = actCheckPeriod.DateStart.Value;
                            DateTime dt = actCheck.DocumentTime.Value;
                            DateTime actDate = new DateTime(actD.Year, actD.Month, actD.Day, dt.Hour, dt.Minute, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }


                    }
                    else
                    {

                        if (actCheck != null && actCheck.DocumentTime.HasValue)
                        {
                            DateTime dd = actCheck.DocumentDate.Value;
                            DateTime dt = actCheck.DocumentTime.Value;
                            DateTime actDate = new DateTime(dd.Year, dd.Month, dd.Day, dt.Hour, dt.Minute, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else if (actCheck != null)
                        {
                            DateTime dd = actCheck.DocumentDate.Value;
                            DateTime actDate = new DateTime(dd.Year, dd.Month, dd.Day, 12, 0, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                    }
                }
                else if (act.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                {
                    var actRemovalPeriod = ActRemovalPeriodDomain.GetAll()
                    .Where(x => x.ActRemoval.Id == act.Id).FirstOrDefault();
                    if (actRemovalPeriod != null)
                    {
                        if (actRemovalPeriod.DateStart.HasValue && !string.IsNullOrEmpty(actRemovalPeriod.TimeStart))
                        {
                            DateTime dateStart;
                            DateTime.TryParse(actRemovalPeriod.TimeEnd, out dateStart);
                            DateTime actD = actRemovalPeriod.DateStart.Value;
                            DateTime actDate = new DateTime(actD.Year, actD.Month, actD.Day, dateStart.Hour, dateStart.Minute, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                    }
                    else
                    {
                        var actRemoval = ActRemovalDomain.Get(act.Id);
                        if (actRemoval != null && actRemoval.DocumentTime.HasValue)
                        {
                            DateTime dd = actRemoval.DocumentDate.Value;
                            DateTime dt = actRemoval.DocumentTime.Value;
                            DateTime actDate = new DateTime(dd.Year, dd.Month, dd.Day, dt.Hour, dt.Minute, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else if (actRemoval != null && actRemoval.DocumentDate.HasValue)
                        {
                            DateTime dd = actRemoval.DocumentDate.Value;
                            DateTime actDate = new DateTime(dd.Year, dd.Month, dd.Day, 12, 0, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                        else if (actRemoval != null)
                        {
                            DateTime dd = actRemoval.ObjectCreateDate;
                            DateTime actDate = new DateTime(dd.Year, dd.Month, dd.Day, 12, 0, 0);
                            return actDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        }
                    }
                }
            }
            return "";
        }

        private IInspector[] GetInspectorsFromDisposal(long decId)
        {
            List<IInspector> newElement = new List<IInspector>();
            var inspectorsDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            inspectorsDomain.GetAll()
                .Where(x => x.DocumentGji != null && x.DocumentGji.Id == decId)
                .Select(x => x.Inspector).ToList().ForEach(x =>
                {
                    var zonal = zonalInspDomain.GetAll()
                    .Where(y => y.Inspector != null && y.Inspector == x).FirstOrDefault();
                    newElement.Add(new IInspector
                    {
                        fullName = x.Fio,
                        position = new IDictionary
                        {
                            dictId = "003d7ce8-c474-11eb-8529-0242ac130003",
                            dictVersionId = x.ERKNMPositionGuid
                        }
                    });
                });

            return newElement.ToArray();
        }
        private List<XElement> GetInspectorsResultFromDisposal(Disposal diaposal)
        {
            List<XElement> newElement = new List<XElement>();
            int i = 1;
            var inspectorsDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            inspectorsDomain.GetAll()
                .Where(x => x.DocumentGji != null && x.DocumentGji.Id == diaposal.Id)
                .Select(x => x.Inspector).ToList().ForEach(x =>
                {
                    var zonal = zonalInspDomain.GetAll()
                    .Where(y => y.Inspector != null && y.Inspector == x).FirstOrDefault();
                    newElement.Add(new XElement(erp_typesNamespace + "IResultInspector",
                               new XAttribute("fullName", x.Fio),
                               new XAttribute("position", x.Position + " " + zonal.ZonalInspection.NameGenetive),
                               new XAttribute("inspectorTypeId", "1")
                               ));
                    i++;
                });



            return newElement;
        }

        private List<XElement> GetCorrectionInspectorsResultFromDisposal(Disposal diaposal, string rGuid)
        {
            List<XElement> newElement = new List<XElement>();
            int i = 1;
            var inspectorsDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            inspectorsDomain.GetAll()
                .Where(x => x.DocumentGji != null && x.DocumentGji.Id == diaposal.Id)
                .Select(x => x.Inspector).ToList().ForEach(x =>
                {
                    var zonal = zonalInspDomain.GetAll()
                    .Where(y => y.Inspector != null && y.Inspector == x).FirstOrDefault();
                    newElement.Add(new XElement(erp_typesNamespace + "InsertIResultInspector",
                         new XAttribute("rGuid", rGuid),
                               new XAttribute("fullName", x.Fio),
                               new XAttribute("position", x.Position + " " + zonal.ZonalInspection.NameGenetive),
                               new XAttribute("inspectorTypeId", "1")
                               ));
                    i++;
                });



            return newElement;
        }

        private void SetErrorState(ERKNM requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            GISERPDomain.Update(requestData);
        }

        private void SaveGISERPFile(ERKNM request, byte[] data, string fileName)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            GISERPFileDomain.Save(new ERKNMFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                ERKNM = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private string GetErrorsNodeValue(ApplicationError[] errors)
        {
            string result = string.Empty;
            if (errors.Length>0)
            {
                result = errors.ToList().AggregateWithSeparator(x=> x.text,";");
            }
            return result;
        }
        private void SaveGISERPFile(ERKNM request, XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            ERKNMFile faultRec = new ERKNMFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                ERKNM = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            };

            GISERPFileDomain.Save(faultRec);
        }

        private void SaveGISERPException(ERKNM request, Exception exception, string val)
        {
            if (exception == null)
                return;

            GISERPFileDomain.Save(new ERKNMFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                ERKNM = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}\n{val}").GetBytes())
            });
        }

        private void ProceedElement(XElement element)
        {
            if (element.Name == erp_typesNamespace + "Prosecutor")
            {
                string regionsCode = element.Attribute("RegionsCode")?.Value;
                string localAreasCode = element.Attribute("LocalAreasCode")?.Value;
                string name = element.Attribute("Name")?.Value;
                string federalCentersCode = element.Attribute("FederalCentersCode")?.Value;
                string сode = element.Attribute("Code")?.Value;
                if (!notDeletedCodes.Contains(сode))
                {
                    ProsecutorOffice proff = new ProsecutorOffice
                    {
                        Code = сode,
                        Name = name,
                        FederalCentersCode = federalCentersCode,
                        LocalAreasCode = localAreasCode,
                        RegionsCode = regionsCode,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 0
                    };
                    try
                    {
                        ProsecutorOfficeDomain.Save(proff);
                    }
                    catch (Exception e)
                    {

                    }
                }
                var childElements = element.Elements();
                if (childElements != null)
                {
                    foreach (var childElement in childElements)
                    {
                        ProceedElement(childElement);
                    }
                }

            }
        }  
        /// <summary>
        /// Генерация XML запроса о платежах с TimeInterval
        /// </summary>
        private XElement GetPaymentRequestXML()
        {

            var result = new XElement(erpNamespace + "Request",
                new XElement(erp_typesNamespace + "Get",
                    new XAttribute("ATTRIBUTE_MODEL_TYPE", "prosec"),
                    new XElement(erp_typesNamespace + "AskProsecOffices"

                    )
                )
            );

            result.SetAttributeValue(XNamespace.Xmlns + "erp", erpNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "erp_types", erp_typesNamespace);
            return result;
        }

        private void SaveFile(byte[] data, string fileName, string messId)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            //ExternalExchangeTestingFilesDomain.Save(new ExternalExchangeTestingFiles
            //{
            //    ObjectCreateDate = DateTime.Now,
            //    ObjectEditDate = DateTime.Now,
            //    ObjectVersion = 1,
            //    Document = _fileManager.SaveFile(fileName, data),
            //    ClassDescription = messId,
            //    ClassName = "ProsecutorOffice",
            //    User = "Пока не установлен"

            //});
        }

        private void SaveFile(XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            //ExternalExchangeTestingFiles faultRec = new ExternalExchangeTestingFiles
            //{
            //    ObjectCreateDate = DateTime.Now,
            //    ObjectEditDate = DateTime.Now,
            //    ObjectVersion = 1,
            //    ClassName = "ProsecutorOffice",
            //    User = "Пока не установлен",
            //    Document = _fileManager.SaveFile(stream, fileName)
            //};

            //ExternalExchangeTestingFilesDomain.Save(faultRec);
        }

        private void SaveException(Exception exception)
        {
            if (exception == null)
                return;

            //ExternalExchangeTestingFilesDomain.Save(new ExternalExchangeTestingFiles
            //{
            //    ObjectCreateDate = DateTime.Now,
            //    ObjectEditDate = DateTime.Now,
            //    ObjectVersion = 1,
            //    ClassName = "ProsecutorOffice",
            //    User = "Пока не установлен",
            //    Document = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            //});
        }

        private void ChangeState(ERKNM requestData, RequestState state)
        {
            requestData.RequestState = state;
            GISERPDomain.Update(requestData);
        }

        #endregion

        #region Nested classes
        internal class Identifiers
        {
            internal string SenderIdentifier;
            internal string SenderRole;
            internal string OriginatorID;
        }

        #endregion

    }
}
