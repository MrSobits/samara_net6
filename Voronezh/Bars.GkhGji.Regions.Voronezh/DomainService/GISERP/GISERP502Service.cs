﻿using Bars.B4;
using Bars.B4.Config;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Namespaces;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
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
using SMEV3Library.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ERP501;
using System.Xml.Serialization;
using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class GISERP502Service : IGISERPService
    {
        #region Constants

        static XNamespace erpNamespace = @"urn://ru.gov.proc.erp.communication/5.0.2";
        static XNamespace erp_typesNamespace = @"urn://ru.gov.proc.erp.communication/types/5.0.2";

        private List<string> notDeletedCodes;

        #endregion

        #region Properties      
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }
        public IDomainService<ProsecutorOffice> ProsecutorOfficeDomain { get; set; }
        public IDomainService<GISERP> GISERPDomain { get; set; }
        public IDomainService<DisposalControlMeasures> DisposalControlMeasuresDomain { get; set; }
        public IDomainService<ProdCalendar> ProdCalendarDomain { get; set; }
        public IDomainService<DisposalAnnex> DisposalAnnexDomain { get; set; }
        public IDomainService<GISERPFile> GISERPFileDomain { get; set; }
        public IDomainService<Disposal> DisposalDomain { get; set; }
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
        public IWindsorContainer Container { get; set; }

        private FileAttachment reqAttachments;

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public GISERP502Service(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        public IDataResult GetListDisposal(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var giserpdocsServ = this.Container.Resolve<IDomainService<GISERP>>();
            var docidList = giserpdocsServ.GetAll()
                .Where(x => x.Disposal != null)
                .Select(x => x.Disposal.Id).Distinct().ToList();

            var data = DocumentGjiDomain.GetAll()
                .Where(x => x.ObjectCreateDate >= DateTime.Now.AddMonths(-26))
                .Where(x => !x.State.StartState)
                .Where(x => !docidList.Contains(x.Id))
                .Where(x => x.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Disposal)
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

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <param name="response">Ответ</param>
        /// <param name="indicator">Индикатор прогресса для таски</param>
        public bool TryProcessResponse(GISERP requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //сохранение данных
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveGISERPFile(requestData, response.SendedData, "GetResponceRequest.dat");
                SaveGISERPFile(requestData, response.ReceivedData, "GetResponceResponse.dat");
                response.Attachments.ForEach(x => SaveGISERPFile(requestData, x.FileData, x.FileName));

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
                        XElement newElement = XElement.Parse(response.MessagePrimaryContent.ToString());
                        var respElement = newElement.Element(erpNamespace + "Response");
                        var exportedCheckElement = respElement.Element(erp_typesNamespace + "Set");
                        var createInspectionElement = exportedCheckElement.Element(erp_typesNamespace + "CreateInspection");
                       
                        if (createInspectionElement != null)
                        {
                            var inspectionElement = createInspectionElement.Element(erp_typesNamespace + "Inspection");
                            foreach (XAttribute attr in inspectionElement.Attributes())
                            {
                                if (attr.Name.ToString().ToUpper() == "ERPID")
                                {
                                    requestData.Answer = "Создана проверка ИД = " + attr.Value;
                                    requestData.RequestState = RequestState.ResponseReceived;
                                    requestData.ERPID = attr.Value;
                                    try
                                    {
                                        var disposalFromRequest = DisposalDomain.Get(requestData.Disposal.Id);
                                        disposalFromRequest.ERPID = attr.Value;
                                        DisposalDomain.Update(disposalFromRequest);
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    //  GISERPDomain.Update(requestData);
                                }
                                if (attr.Name.ToString().ToUpper() == "GUID")
                                {
                                    requestData.INSPECTION_GUID = attr.Value;
                                }
                            }
                            var objectElement = inspectionElement.Element(erp_typesNamespace + "IObject");
                            if (objectElement != null)
                            {
                                foreach (XAttribute attr in objectElement.Attributes())
                                {
                                    if (attr.Name.ToString().ToUpper() == "NUMGUID")
                                    {
                                        requestData.OBJECT_GUID = attr.Value;
                                    }
                                }
                            }
                            var resultElement = objectElement.Element(erp_typesNamespace + "IResult");
                            if (resultElement != null)
                            {
                                foreach (XAttribute attr in resultElement.Attributes())
                                {
                                    if (attr.Name.ToString().ToUpper() == "NUMGUID")
                                    {
                                        requestData.RESULT_GUID = attr.Value;
                                    }
                                }
                            }
                            try
                            {
                                GISERPDomain.Update(requestData);
                            }
                            catch (Exception e)
                            {
                                SaveGISERPException(requestData, e, response.MessagePrimaryContent.ToString());
                                SetErrorState(requestData, "Ошибка сохранения " + e.ToString());
                                return false;
                            }
                        }
                        else
                        {
                            var updateInspectionElement = respElement.Element(erp_typesNamespace + "UpdateInspection");
                            if (updateInspectionElement != null)
                            {

                                requestData.Answer = "Обновлена проверка ИД =" + requestData.ERPID;
                               

                                requestData.RequestState = RequestState.ResponseReceived;
                                GISERPDomain.Update(requestData);
                                return true;
                            }
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
                var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
                Disposal disposal = disposalDomain.Get(protocolData);
                InspectionGji inspection = disposal.Inspection;
                //объявляем возвращаемые переменные
                ERPInspectionType inspType = ERPInspectionType.PP; // по умолчанию будет плановая
                ERPAddressType adrtype = ERPAddressType.TYPE_OTHER;
                ERPNoticeType noticeType = ERPNoticeType.TYPE_OTHER;
                ERPObjectType objType = ERPObjectType.TYPE_I;
                ERPReasonType reasonType = ERPReasonType.RSN_PP_OTHER;
                ERPRiskType riskType = ERPRiskType.CLASS_VI;
                KindKND kindKnd = KindKND.HousingSupervision;
                if (disposal.KindKNDGJI == KindKNDGJI.HousingSupervision)
                {
                    kindKnd = KindKND.HousingSupervision;
                }
                else if (disposal.KindKNDGJI == KindKNDGJI.LicenseControl)
                {
                    kindKnd = KindKND.LicenseControl;
                }
                
                DateTime dateStart = disposal.DateStart.HasValue ? disposal.DateStart.Value : disposal.ObjectCreateDate;
                string subjectAddress = string.Empty;
                string oktmo = string.Empty;
                string inspectionName = string.Empty;
                string carryoutEvents = string.Empty;
                string goals = string.Empty;
                try // получаем цели и задачи проверки для BaseChelyabinsk документов ГЖИ
                {
                    var disposalSurveyPurposeDomain = this.Container.Resolve<IDomainService<DisposalSurveyPurpose>>();
                    var disposalSurveyObjectiveDomain = this.Container.Resolve<IDomainService<DisposalSurveyObjective>>();//
                    var disposalVerificationSubjDomain = this.Container.Resolve<IDomainService<DisposalVerificationSubject>>();//
                    goals = "Цели проверки: " + disposalSurveyPurposeDomain.GetAll()
                        .Where(x => x.Disposal == disposal)
                        .AggregateWithSeparator(x => x.Description, "; ");
                    goals = goals + " Задачи проверки: " + disposalSurveyObjectiveDomain.GetAll()
                        .Where(x => x.Disposal == disposal)
                        .AggregateWithSeparator(x => x.Description, "; ");
                    goals = goals + " Предмет проверки: " + disposalVerificationSubjDomain.GetAll()
                       .Where(x => x.Disposal == disposal)
                       .AggregateWithSeparator(x => x.SurveySubject.Name, "; ");
                }
                catch
                {
                    goals = "Не удалось получить цели и задачи проверки";
                }
                if (disposal.KindCheck != null)
                {
                    List<TypeCheck> notPlanTypes = new List<TypeCheck> // внеплановые проверки, если такие указаны в распоряжении, то выгружаем как внеплановую
                    {
                        TypeCheck.Monitoring,
                        TypeCheck.NotPlannedDocumentation,
                        TypeCheck.NotPlannedDocumentationExit,
                        TypeCheck.NotPlannedExit,
                        TypeCheck.VisualSurvey,
                        TypeCheck.InspectionSurvey
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

                    if (notPlanTypes.Contains(disposal.KindCheck.Code))
                    {
                        inspType = ERPInspectionType.VP;
                    }
                    if (localInspectionAdress.Contains(disposal.KindCheck.Code))
                    {
                        typeAdressForCheck = TypeAdressForCheck.juradrContragent;//всегда либо дом либо юр адрес
                    }
                    else if (jurUKAdress.Contains(disposal.KindCheck.Code))
                    {
                        typeAdressForCheck = TypeAdressForCheck.juradrContragent;
                    }
                    else // во всех остальных случаях едем на дом к пострадавшим
                    {
                        typeAdressForCheck = TypeAdressForCheck.checkedMkd;
                    }

                }
                if (disposal.TypeDisposal == TypeDisposalGji.Base) // если основание не проверка исполнения предписания
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
                else if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)//проверка исполнения предписания
                {
                    var childrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
                    var parentDoc = childrenDomain.GetAll()
                        .Where(x => x.Children == disposal)
                        .Where(x => x.Parent != null && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription).FirstOrDefault();
                    if (parentDoc != null)
                    {
                        reasonType = ERPReasonType.RSN_VP_CVI;
                        inspectionName = $"Проверка исполнения предписания №{parentDoc.Parent.DocumentNumber} от {parentDoc.Parent.DocumentDate.Value.ToShortDateString()}";
                    }
                }

                var disposalControlMeasuresDomain = this.Container.Resolve<IDomainService<DisposalControlMeasures>>();

                carryoutEvents = disposalControlMeasuresDomain.GetAll()//мероприятия проверки для челябинск-стайл ГЖИ
                    .Where(x => x.Disposal == disposal)
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
                            subjectAddress = "Воронежская область, г. Воронеж, ул. Кирова, д. 6а"; // в воронеже все инспекторы сидят в одном месте
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

                if (disposal.KindKNDGJI == KindKNDGJI.LicenseControl)
                {
                    kindKnd = KindKND.LicenseControl;
                }               

                //получаем акт
                var actfromDisposal = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Parent.Id == disposal.Id)
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
                            .Where(x => x.Inspection == disposal.Inspection).ToList();

                    }
                }
                prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Воронежской области").FirstOrDefault();
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

        /// <summary>
        /// Отправка начисления в ГИС ГМП
        /// </summary>
        public bool SendInitiationRequest(GISERP requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                GISERPFileDomain.GetAll().Where(x => x.GISERP == requestData).ToList().ForEach(x => GISERPFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                if (requestData.GisErpRequestType == GisErpRequestType.Initialization)
                {
                    requestData.checkId = $"I_{Guid.NewGuid()}";
                }

                XElement request = GetInitiationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                indicator?.Report(null, 20, "Отправка запроса");
                List<FileAttachment> attList = new List<FileAttachment>();
                if (reqAttachments != null)
                {
                    attList.Add(reqAttachments);
                }
                var requestResult = _SMEV3Service.SendRequestAsync(request, attList.Count > 0 ? attList : null, true).GetAwaiter().GetResult();
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

        private XElement GetInitiationRequestXML(GISERP requestData)
        {
            if (requestData.GisErpRequestType == GisErpRequestType.Initialization)
            {
                return GetInitiationOrCorrection(requestData);
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

        private XElement GetInitiationOrCorrection(GISERP requestData)
        {
            //SPV_26 - жилищный надзор
            //SPV_2 - лицензионный контроль

            string iTypeId = string.Empty;
            string iNoticeTypeId = string.Empty;
            string IRISK_ID = string.Empty;
            string iSupervisionId = string.Empty;
            string frguServIdBk = string.Empty;
            string addressTypeId = string.Empty;
            string IREASON_ID = string.Empty;
            if (requestData.ERPInspectionType == ERPInspectionType.PP)
            {
                iTypeId = "0";
            }
            else
            {
                iTypeId = "1";
            }
            switch (requestData.ERPNoticeType) // смотрим что явилось основанием
            {
                case ERPNoticeType.TYPE_I: { iNoticeTypeId = "1"; } break;
                case ERPNoticeType.TYPE_II: { iNoticeTypeId = "2"; } break;
                case ERPNoticeType.TYPE_III: { iNoticeTypeId = "3"; } break;
                case ERPNoticeType.TYPE_OTHER: { iNoticeTypeId = "44"; } break;
            }
            switch (requestData.ERPRiskType) // смотрим что явилось основанием
            {
                case ERPRiskType.CLASS_I: { IRISK_ID = "CLASS_I"; } break;
                case ERPRiskType.CLASS_II: { IRISK_ID = "CLASS_II"; } break;
                case ERPRiskType.CLASS_III: { IRISK_ID = "CLASS_III"; } break;
                case ERPRiskType.CLASS_IV: { IRISK_ID = "CLASS_IV"; } break;
                case ERPRiskType.CLASS_V: { IRISK_ID = "CLASS_V"; } break;
                case ERPRiskType.CLASS_VI: { IRISK_ID = "CLASS_VI"; } break;
            }
            if (requestData.KindKND == KindKND.LicenseControl)
            {
                iSupervisionId = "1050";             
                frguServIdBk = "3640000010000306797";
            }
            else
            {
                iSupervisionId = "1060";
                frguServIdBk = "3640000010000026981";
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
            switch (requestData.ERPAddressType)
            {
                case ERPAddressType.TYPE_I: { addressTypeId = "1"; } break;
                case ERPAddressType.TYPE_II: { addressTypeId = "2"; } break;
                case ERPAddressType.TYPE_III: { addressTypeId = "3"; } break;
                case ERPAddressType.TYPE_OTHER: { addressTypeId = "44"; } break;
            }
            switch (requestData.ERPReasonType)
            {
                case ERPReasonType.RSN_PP_I: { IREASON_ID = "1"; } break;
                case ERPReasonType.RSN_PP_II: { IREASON_ID = "2"; } break;
                case ERPReasonType.RSN_PP_IV: { IREASON_ID = "4"; } break;
                case ERPReasonType.RSN_PP_OTHER: { IREASON_ID = "166"; } break;
                case ERPReasonType.RSN_VP_CVI: { IREASON_ID = "139"; } break;
                case ERPReasonType.LIC: { IREASON_ID = "107"; } break;
            }
            int DURATION_DAY = 1;
            if (requestData.KindKND == KindKND.HousingSupervision && IREASON_ID == "139")
            {
                IREASON_ID = "106";
            }

            if (disp.DateStart.HasValue && disp.DateEnd.HasValue)
            {
                var prodCalendarContainer = ProdCalendarDomain.GetAll()
                  .Where(x => x.ProdDate >= disp.DateStart.Value && x.ProdDate <= disp.DateEnd.Value).Select(x => x.ProdDate).ToList();

                DURATION_DAY = 0;
                for (DateTime dt = disp.DateStart.Value; dt <= disp.DateEnd.Value; dt = dt.AddDays(1))
                {
                    if ((dt.DayOfWeek != DayOfWeek.Saturday) && (dt.DayOfWeek != DayOfWeek.Sunday))
                    {
                        if(!prodCalendarContainer.Contains(dt))
                        DURATION_DAY++;
                    }
                }
                //TimeSpan t = disp.DateEnd.Value - disp.DateStart.Value;
                //if (t.Days > DURATION_DAY)
                //{
                //    DURATION_DAY = t.Days;
                //}
            }

            if (requestData.ERPInspectionType == ERPInspectionType.VP)
            {
                IREASON_ID = IREASON_ID.Replace("PP", "VP");
            }

            string OKATO_RAZDEL = string.Empty;
            string OKATO_KOD1 = string.Empty;
            string OKATO_KOD2 = "000";
            string OKATO_KOD3 = "000";
            if (!string.IsNullOrEmpty(requestData.OKATO.Trim()))
            {
                string fullOKATO = requestData.OKATO.Trim();
                if (fullOKATO.Substring(2, 1) == "2")
                {
                    OKATO_RAZDEL = "2";
                }
                else
                {
                    OKATO_RAZDEL = "1";
                }
                OKATO_KOD1 = fullOKATO.Substring(2, 3);
            }

            if (requestData.GisErpRequestType == GisErpRequestType.Initialization)
            {
                List<AuthorityServCommonType> AuthorityServCommonTypeList = new List<AuthorityServCommonType>();
                AuthorityServCommonTypeList.Add(new AuthorityServCommonType
                {
                    frguServIdBk = frguServIdBk
                });

                List<AuthorityCommonType> AuthorityCommonTypeList = new List<AuthorityCommonType>();
                AuthorityCommonTypeList.Add(new AuthorityCommonType
                {
                    frguOrgIdBk = "3640000010000024201",
                    IAuthorityServ = AuthorityServCommonTypeList.ToArray(),
                    IInspector = GetInspectors(disp)
                });

                var createInspection = new CreateInspectionRequestType
                {
                    Inspection = new InspectionCreateInspectionCommonType
                    {
                        isStartMonth = false,
                        isStartMonthSpecified = true,
                        iTypeId = iTypeId,
                        fzId = "0",
                        prosecId = requestData.ProsecutorOffice.Code,
                        startDate = disp.DateStart.HasValue ? disp.DateStart.Value : disp.DocumentDate.Value,
                        startDateSpecified = true,
                        domainId = requestData.OKATO,
                        IAuthority = AuthorityCommonTypeList.ToArray(),
                        IClassification = new ClassificationCommonType
                        {
                            iSupervisionId = iSupervisionId,
                            iCarryoutTypeId = iCarryoutTypeId,
                            IClassificationLb = GetNPAFromDisposal(disp),
                            iNoticeDate = disp.NcDate.HasValue ? disp.NcDate.Value : disp.DocumentDate.Value,
                            iNoticeDateSpecified = disp.NcDate.HasValue ? true:false
                        },
                        ISubject = GetSubjectCT(disp),
                        IObject = GetObjects(disp, requestData, addressTypeId),
                        IApprove = new IApproveCommonType
                        {
                            durationDay = DURATION_DAY.ToString(),
                            endDate = disp.DateEnd.HasValue ? disp.DateEnd.Value : disp.DocumentDate.Value.AddDays(20),
                            endDateSpecified = true,
                            inspTarget = requestData.Goals,
                            decisionPlace = disp.TypeAgreementResult == TypeAgreementResult.Agreed ? "394006, Воронежская обл, г. Воронеж, пер. Красноармейский, д. 12Б" : "",
                            decisionSignerTitle = !string.IsNullOrEmpty(disp.PositionProcAproove) ? disp.PositionProcAproove : "",
                            decisionSignerName = !string.IsNullOrEmpty(disp.FioProcAproove) ? disp.FioProcAproove : "",
                            IApproveDocs = GetApproveDocs(disp, requestData)
                        },
                        IPublish = new IPublishCommonType
                        {
                            IPublishStatus = "ASK_TO_PUBLISH"
                        },
                        IAttachments = GetAttachments(disp)


                    }
                };
                List<CreateInspectionRequestType> inspList = new List<CreateInspectionRequestType>();
                inspList.Add(createInspection);

                LetterToErpType request = new LetterToErpType
                {
                    Item = new LetterToErpTypeSet
                    {
                        Items = inspList.ToArray()
                    }
                };
                return ToXElement<LetterToErpType>(request);

            }               
            else
                return null;
        }

        private List<XElement> GetCorrection(GISERP requestData)
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
                ACT_DATE_CREATE = actCheckDoc.DocumentDate.HasValue? actCheckDoc.DocumentDate.Value.ToString("yyyy-MM-ddTHH:mm:ss"): actCheckDoc.ObjectCreateDate.ToString("yyyy-MM-ddTHH:mm:ss");  
            }
            string address = "";
            if (iCarryoutTypeId == "2")
            {
                address = "Воронежская область, г. Воронеж, ул. Кирова, д. 6а";
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
            else if(requestData.HasViolations == YesNoNotSet.Yes)
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
            if(act != null)
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
                        else if(actCheck != null)
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
                        else if(actRemoval != null)
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

        private List<XElement> GetInspectorsFromDisposal(Disposal diaposal)
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
                    newElement.Add(new XElement(erp_typesNamespace + "IInspector",
                               new XAttribute("fullName", x.Fio),
                               new XAttribute("position", x.Position + " " + zonal.ZonalInspection.NameGenetive),
                               new XAttribute("inspectorTypeId", "1")
                               ));
                    i++;
                });



            return newElement;
        }

        private InspectorCommonType[] GetInspectors(Disposal diaposal)
        {
            List<InspectorCommonType> newElement = new List<InspectorCommonType>();
            var inspectorsDomain = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            inspectorsDomain.GetAll()
                .Where(x => x.DocumentGji != null && x.DocumentGji.Id == diaposal.Id)
                .Select(x => x.Inspector).ToList().ForEach(x =>
                {
                    var zonal = zonalInspDomain.GetAll()
                    .Where(y => y.Inspector != null && y.Inspector == x).FirstOrDefault();
                    newElement.Add(new InspectorCommonType
                    {
                        fullName = x.Fio,
                        position = x.Position,
                        inspectorTypeId = "1"
                    });
                });
            return newElement.ToArray();
        }
        private List<XElement> GetNPAsFromDisposal(Disposal diaposal)
        {
            List<XElement> newElement = new List<XElement>();
            int i = 1;
            //var dispInsfFoundationCheckDomain = this.Container.Resolve<IDomainService<DisposalInspFoundationCheck>>();
            //dispInsfFoundationCheckDomain.GetAll()
            //    .Where(x => x.Disposal != null && x.Disposal.Id == diaposal.Id)
            //    .Select(x => x.InspFoundationCheck).ToList().ForEach(x =>
            //    {                   
            //        newElement.Add(new XElement(erp_typesNamespace + "IClassificationLb",
            //                   new XAttribute("lbDocumentName", x.Name)
            //                   ));
            //        i++;
            //    });

            List<string> pravOsnList = new List<string>();
            //if (diaposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation || diaposal.KindCheck.Code == TypeCheck.PlannedDocumentation)
            //{
            //    if (diaposal.KindKNDGJI == KindKNDGJI.LicenseControl)
            //    {
            //        pravOsnList.Clear();
            //        pravOsnList.Add("ст. 196 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
            //        pravOsnList.Add("ст. 10, 11 Федерального закона от 26.12.2008 № 294-ФЗ «О защите прав юридических лиц и индивидуальных предпринимателей при осуществлении государственного контроля (надзора) и муниципального контроля»;");
            //        pravOsnList.Add("ст. 19 Федерального закона от 04.05.2011 № 99-ФЗ «О лицензировании отдельных видов деятельности»;");
            //        pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799.");
            //    }
            //    else
            //    {
            //        pravOsnList.Clear();
            //        pravOsnList.Add("ст. 20 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
            //        pravOsnList.Add("ст. 10, 11 Федерального закона от 26.12.2008 № 294-ФЗ «О защите прав юридических лиц и индивидуальных предпринимателей при осуществлении государственного контроля (надзора) и муниципального контроля»;");
            //        pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799.");
            //    }
            //}
            //else if (diaposal.KindCheck.Code == TypeCheck.PlannedExit || diaposal.KindCheck.Code == TypeCheck.NotPlannedExit)
            //{
            //    if (diaposal.KindKNDGJI == KindKNDGJI.LicenseControl)
            //    {
            //        pravOsnList.Clear();
            //        pravOsnList.Add("ст. 196 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
            //        pravOsnList.Add("ст. 10, 12 Федерального закона от 26.12.2008 № 294-ФЗ «О защите прав юридических лиц и индивидуальных предпринимателей при осуществлении государственного контроля (надзора) и муниципального контроля»;");
            //        pravOsnList.Add("ст. 19 Федерального закона от 04.05.2011 № 99-ФЗ «О лицензировании отдельных видов деятельности»;");
            //        pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799.");
            //    }
            //    else
            //    {
            //        pravOsnList.Clear();
            //        pravOsnList.Add("ст. 20 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
            //        pravOsnList.Add("ст. 10, 12 Федерального закона от 26.12.2008 № 294-ФЗ «О защите прав юридических лиц и индивидуальных предпринимателей при осуществлении государственного контроля (надзора) и муниципального контроля»;");
            //        pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799.");
            //    }
            //}
            if (diaposal.KindKNDGJI == KindKNDGJI.LicenseControl)
            {
                pravOsnList.Clear();
                pravOsnList.Add("ч. 3 ст. 196 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
                pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799 «Об утверждении Положения о государственной жилищной инспекции Воронежской области».");
            }
            else
            {
                pravOsnList.Clear();
                pravOsnList.Add("ч. 4.2 ст. 20 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
                pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799 «Об утверждении Положения о государственной жилищной инспекции Воронежской области».");
            }
            foreach (string npa in pravOsnList)
            {
                newElement.Add(new XElement(erp_typesNamespace + "IClassificationLb",
                             new XAttribute("lbDocumentName", npa)
                             ));
            }


            return newElement;
            //return null; 
        }

        private IApproveDocsCommonType[] GetApproveDocs(Disposal disp, GISERP requestData)//Получаем список документов о согласовании
        {
            List<IApproveDocsCommonType> newElementList = new List<IApproveDocsCommonType>();
            newElementList.Add(new IApproveDocsCommonType
            {
                docDate = disp.DocumentDate.Value,
                docDateSpecified = true,
                docAtr = disp.DocumentNumber,
                iApproveDocId = "1"
            });

            if (!string.IsNullOrEmpty(disp.ProcAprooveNum) && disp.ProcAprooveDate.HasValue)
            {
                newElementList.Add(new IApproveDocsCommonType
                {
                    docDate = disp.ProcAprooveDate.Value,
                    docDateSpecified = true,
                    docAtr = disp.ProcAprooveNum,
                    iApproveDocId = "3"
                });
                newElementList.Add(new IApproveDocsCommonType
                {
                    docDate = disp.ProcAprooveDate.Value,
                    docDateSpecified = true,
                    docAtr = disp.ProcAprooveNum,
                    iApproveDocId = "4"
                });
            }


            return newElementList.ToArray();
        }
        private ObjectCommonType[] GetObjects(Disposal diaposal, GISERP requestData, string addressTypeId)
        {
            List<ObjectCommonType> newElement = new List<ObjectCommonType>();
            newElement.Add(new ObjectCommonType
            {
                address = diaposal.Inspection.Contragent != null ? diaposal.Inspection.Contragent.JuridicalAddress : requestData.SubjectAddress,
                addressTypeId = diaposal.Inspection.Contragent != null ? "1" : addressTypeId,
                iObjectTypeId = "4"
            });
            return newElement.ToArray();
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
        private IAttachmentsCommonType[] GetAttachments(Disposal diaposal)
        {
            List<IAttachmentsCommonType> newElement = new List<IAttachmentsCommonType>();
            List<IAttachmentCommonType> att = new List<IAttachmentCommonType>();
            var annex = DisposalAnnexDomain.GetAll()
                .Where(x => x.Disposal == diaposal && x.TypeAnnex == TypeAnnex.Disposal).FirstOrDefault();
            this.reqAttachments = new FileAttachment
            {
                FileData = annex.SignedFile != null ? _fileManager.GetFile(annex.SignedFile).ReadAllBytes() : _fileManager.GetFile(annex.File).ReadAllBytes(),
                FileGuid = Guid.NewGuid().ToString().ToUpper(),
                FileName = annex.SignedFile != null ? annex.SignedFile.FullName : annex.File.FullName
            };
            if (annex == null)
            {
                return null;
            }
            att.Add(new IAttachmentCommonType
            {
                attachmentId = reqAttachments.FileGuid
            });
            newElement.Add(new IAttachmentsCommonType
            {
                description = $"Скан образ документа Приказ ГЖИ N{diaposal.DocumentNumber} от {diaposal.DocumentDate.Value.ToString("dd.MM.yyyy")}",
                IAttachment = att.ToArray(),
                type = "Приказ"
            });
            return newElement.ToArray();
        }


        private SubjectCommonType GetSubjectCT(Disposal diaposal)
        {

            var contragent = diaposal.Inspection.Contragent;
            string ISubjectType = "0";

            if (contragent != null)
            {
                var localGovDomain = this.Container.Resolve<IDomainService<LocalGovernment>>();
                var omsu = localGovDomain.GetAll()
                    .Where(x => x.Contragent == contragent).FirstOrDefault();
                if (omsu != null)
                {
                    ISubjectType = "2";
                }
                else
                {
                    var ukDomain = this.Container.Resolve<IDomainService<ManagingOrganization>>();
                    var uk = ukDomain.GetAll()
                        .Where(x => x.Contragent == contragent).FirstOrDefault();
                    if (uk != null)
                    {
                        ISubjectType = "0";
                    }
                    else
                    {
                        var politicauthoritykDomain = this.Container.Resolve<IDomainService<PoliticAuthority>>();
                        var politicauthority = politicauthoritykDomain.GetAll()
                            .Where(x => x.Contragent == contragent).FirstOrDefault();
                        if (politicauthority != null)
                        {
                            ISubjectType = "1";
                        }
                    }
                }
                return new SubjectCommonType
                {
                    iSubjectTypeId = ISubjectType,
                    inn = contragent.Inn,
                    ogrn = contragent.Ogrn,
                    orgName = contragent.Name
                };
            }
            else
            {
                return null;
            }
        }

        private ClassificationLbCommonType[] GetNPAFromDisposal(Disposal diaposal)
        {
            List<ClassificationLbCommonType> newElement = new List<ClassificationLbCommonType>();
            List<string> pravOsnList = new List<string>();
            if (diaposal.KindKNDGJI == KindKNDGJI.LicenseControl)
            {
                pravOsnList.Clear();
                pravOsnList.Add("ч. 3 ст. 196 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
                pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799 «Об утверждении Положения о государственной жилищной инспекции Воронежской области».");
            }
            else
            {
                pravOsnList.Clear();
                pravOsnList.Add("ч. 4.2 ст. 20 Жилищного кодекса Российской Федерации от 29.12.2004 № 188-ФЗ;");
                pravOsnList.Add("п. 3.1 Положения о государственной жилищной инспекции Воронежской области, утвержденного постановлением правительства Воронежской области от 09.09.2013 № 799 «Об утверждении Положения о государственной жилищной инспекции Воронежской области».");
            }
            foreach (string npa in pravOsnList)
            {
                newElement.Add(new ClassificationLbCommonType
                {
                    lbDocumentName = npa
                });
            }

            return newElement.ToArray();
            //return null; 
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

        private XElement GetSubject(Disposal diaposal)
        {

            var contragent = diaposal.Inspection.Contragent;
            string ISubjectType = "0";

            if (contragent != null)
            {
                var localGovDomain = this.Container.Resolve<IDomainService<LocalGovernment>>();
                var omsu = localGovDomain.GetAll()
                    .Where(x => x.Contragent == contragent).FirstOrDefault();
                if (omsu != null)
                {
                    ISubjectType = "2";
                }
                else
                {
                    var ukDomain = this.Container.Resolve<IDomainService<ManagingOrganization>>();
                    var uk = ukDomain.GetAll()
                        .Where(x => x.Contragent == contragent).FirstOrDefault();
                    if (uk != null)
                    {
                        ISubjectType = "0";
                    }
                    else
                    {
                        var politicauthoritykDomain = this.Container.Resolve<IDomainService<PoliticAuthority>>();
                        var politicauthority = politicauthoritykDomain.GetAll()
                            .Where(x => x.Contragent == contragent).FirstOrDefault();
                        if (politicauthority != null)
                        {
                            ISubjectType = "1";
                        }
                    }
                }

                return new XElement(erp_typesNamespace + "ISubject",
                            new XAttribute("iSubjectTypeId", ISubjectType),
                            new XAttribute("ogrn", contragent.Ogrn),
                            new XAttribute("inn", contragent.Inn),
                            new XAttribute("orgName", contragent.Name)
                            );
            }
            else
            {
                return null;
            }
        }

        private  List<XElement> GetCarryoutEvents(GISERP requestData)//Получаем список мероприятий с датами начала и окончания
        {
            List<XElement> newElementList = new List<XElement>();

            var cmList = DisposalControlMeasuresDomain.GetAll()
                .Where(x => x.Disposal.Id == requestData.Disposal.Id)
                .Select(x=> new DisposalControlMeasures
                {
                    Description = x.Description,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd
                })
                .ToList();

            foreach (DisposalControlMeasures cm in cmList)
            {
                if (cm.DateStart.HasValue && cm.DateEnd.HasValue)
                {
                    var newElem = new XElement(erp_typesNamespace + "ICarryoutEvents",
                                  new XAttribute("eventsText", cm.Description),
                                  new XAttribute("startDate", cm.DateStart.Value.ToString("yyyy-MM-dd")),
                                  new XAttribute("endDate", cm.DateEnd.Value.ToString("yyyy-MM-dd")));
                    newElementList.Add(newElem);
                }
                else
                {
                    var newElem = new XElement(erp_typesNamespace + "ICarryoutEvents",
                                  new XAttribute("eventsText", cm.Description)
                              );
                    newElementList.Add(newElem);
                }
            }
            return newElementList;
        }

        private List<XElement> GetApproveDocs(Disposal disp)//Получаем список документов о согласовании
        {
            List<XElement> newElementList = new List<XElement>();

            newElementList.Add(new XElement(erp_typesNamespace + "IApproveDocs",
                         new XAttribute("docDate", disp.DocumentDate.Value.ToString("yyyy-MM-dd")),
                         new XAttribute("iApproveDocId", "1"),
                         new XAttribute("docAtr", disp.DocumentNumber)
                         ));
            if (!string.IsNullOrEmpty(disp.ProcAprooveNum) && disp.ProcAprooveDate.HasValue)
            {
                newElementList.Add(new XElement(erp_typesNamespace + "IApproveDocs",
                      new XAttribute("docDate", disp.ProcAprooveDate.Value.ToString("yyyy-MM-dd")),
                      new XAttribute("iApproveDocId", "3"),
                      new XAttribute("docAtr", disp.ProcAprooveNum)
                      ));

                newElementList.Add(new XElement(erp_typesNamespace + "IApproveDocs",
                       new XAttribute("docDate", disp.ProcAprooveDate.Value.ToString("yyyy-MM-dd")),
                       new XAttribute("iApproveDocId", "4"),
                       new XAttribute("docAtr", disp.ProcAprooveNum)
                       ));
            }

          
            return newElementList;
        }

        private List<XElement> GetResult(Disposal disposal, string ICARRYOUT_TYPE_ID, string SubjectAddress, string ADDRESS_TYPE_ID)
        {
            List<XElement> newElementList = new List<XElement>();
            string ACT_ADDRESS_TYPE_ID = ADDRESS_TYPE_ID;
            if (ICARRYOUT_TYPE_ID == "2")
            {
                SubjectAddress = "Воронежская область, г. Воронеж, ул. Кирова, д. 6а";
                ACT_ADDRESS_TYPE_ID = "44";
            }

            var actChecks = DocumentGjiChildrenDomain.GetAll()
             .Where(x => x.Parent == disposal)
             .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
             .Select(x => x.Children)
             .ToList();

            foreach (DocumentGji currentDoc in actChecks)
            {
                if (currentDoc.TypeDocumentGji == TypeDocumentGji.ActCheck)
                {
                    var actCheck = ActCheckDomain.Get(currentDoc.Id);
                    string ACT_DATE_CREATE = actCheck.DocumentDate.HasValue ? actCheck.DocumentDate.Value.ToString("yyyy-MM-ddTHH:mm:ss") : actCheck.ObjectCreateDate.ToString("yyyy-MM-ddTHH:mm:ss");
                    string ACT_ADDRESS = SubjectAddress;



                    XElement newElem = new XElement(erp_typesNamespace + "IResult",
                    new XAttribute("actDateCreate", ACT_DATE_CREATE),
                    new XAttribute("actAddress", ACT_ADDRESS),
                    new XAttribute("actAddressTypeId", ADDRESS_TYPE_ID)
                    );
                    var actCheckWitness = ActCheckWitnessDomain.GetAll()
                        .Where(x => x.ActCheck == actCheck).FirstOrDefault();
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
                    var actCheckPeriod = ActCheckPeriodDomain.GetAll()
                        .Where(x => x.ActCheck == actCheck).FirstOrDefault();

                    if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                    {
                        try
                        {
                            int DURATION_DAYS = 0;
                            var convertedDate = disposal.DateEnd.Value.Subtract(disposal.DateStart.Value).Days + 1;
                            string START_DATE = disposal.DateStart.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                            var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                                    .Where(x => x.ProdDate >= disposal.DateStart.Value && x.ProdDate <= disposal.DateEnd.Value).Select(x => x.ProdDate).ToList();
                            for (int i = 0; i < convertedDate; i++)
                            {
                                var checkedDate = disposal.DateStart.Value.AddDays(i);
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

                    newElem.Add(GetInspectorsResultFromDisposal(disposal));
                    var actcheckRealityObject = ActCheckRealityObjectDomain.GetAll()
                        .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                    if (actcheckRealityObject != null)
                    {
                        if (actcheckRealityObject.HaveViolation == YesNoNotSet.Yes)
                        {
                            var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                                       .Where(x => x.Inspection == disposal.Inspection).ToList();
                            if (inspViolations.Count > 0)
                            {
                                List<XElement> violationList = new List<XElement>();
                                foreach (InspectionGjiViol viol in inspViolations)
                                {
                                    string VIOLATION_NOTE = viol.Violation.Name;
                                    string NOTE = ViolationActionsRemovGjiDomain.GetAll()
                                        .Where(x => x.ViolationGji == viol.Violation)
                                        .AggregateWithSeparator(x => x.ActionsRemovViol.Name, ",");
                                    string IVIOLATION_TYPE_ID = "1";
                                    XElement violElem = new XElement(erp_typesNamespace + "IViolation",
                                     new XAttribute("violationNote", VIOLATION_NOTE),
                                     new XAttribute("violationAct", viol.Violation.NormativeDocNames),
                                     new XAttribute("iViolationTypeId", IVIOLATION_TYPE_ID)
                                         );
                                    var inspViolationStages = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                                         .Where(x => x.InspectionViolation == viol).ToList();
                                    foreach (InspectionGjiViolStage stage in inspViolationStages)
                                    {
                                        if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                                        {
                                            string CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                                            string DATE_APPOINTMENT = stage.Document.DocumentDate.HasValue ? stage.Document.DocumentDate.Value.ToString("yyyy-MM-dd") : stage.Document.ObjectCreateDate.ToString("yyyy-MM-dd");
                                            XElement presctElement = new XElement(erp_typesNamespace + "VInjunction",
                                                  new XAttribute("code", CODE),
                                                   new XAttribute("note", NOTE),
                                                   new XAttribute("dateAppointment", DATE_APPOINTMENT),
                                                   stage.DateFactRemoval.HasValue ? new XAttribute("executionNote", "Выполнено") : null,
                                                    stage.DatePlanRemoval.HasValue ? new XAttribute("executionDeadline", stage.DatePlanRemoval.Value.ToString("yyyy-MM-dd")) : null
                                                );
                                            violElem.Add(presctElement);
                                            //newReault.CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                                            //if (stage.Document.DocumentDate.HasValue)
                                            //    newReault.DATE_APPOINTMENT = stage.Document.DocumentDate.Value;
                                            //if (viol.DateFactRemoval != null)
                                            //{
                                            //    newReault.EXECUTION_NOTE = "Выполнено";
                                            //}
                                            //newReault.EXECUTION_DEADLINE = viol.DatePlanRemoval;
                                        }
                                    }
                                    violationList.Add(violElem);

                                }
                                newElem.Add(violationList);
                            }
                        }
                    }





                    newElementList.Add(newElem);
                }
            }
            if (newElementList.Count == 0)
            {
                XElement newElem = new XElement(erp_typesNamespace + "IResult",
                 new XAttribute("actAddressTypeId", ADDRESS_TYPE_ID)
                 );
                
                newElementList.Add(newElem);
            }
            //var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
            //           .Where(x => x.Inspection == disposal.Inspection).ToList();
            //foreach (InspectionGjiViol viol in inspViolations)
            //{
            //    var inspViolationStages = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
            //         .Where(x => x.InspectionViolation == viol).ToList();
            //    foreach (InspectionGjiViolStage stage in inspViolationStages)
            //    {
            //        if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
            //        {
            //            newReault.CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
            //            if (stage.Document.DocumentDate.HasValue)
            //                newReault.DATE_APPOINTMENT = stage.Document.DocumentDate.Value;
            //            if (viol.DateFactRemoval != null)
            //            {
            //                newReault.EXECUTION_NOTE = "Выполнено";
            //            }
            //            newReault.EXECUTION_DEADLINE = viol.DatePlanRemoval;
            //        }
            //    }
            //}



            return newElementList;






        }

        private void SetErrorState(GISERP requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            GISERPDomain.Update(requestData);
        }

        private void SaveGISERPFile(GISERP request, byte[] data, string fileName)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            GISERPFileDomain.Save(new GISERPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GISERP = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveGISERPFile(GISERP request, XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            GISERPFile faultRec = new GISERPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GISERP = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            };

            GISERPFileDomain.Save(faultRec);
        }

        private void SaveGISERPException(GISERP request, Exception exception, string val)
        {
            if (exception == null)
                return;

            GISERPFileDomain.Save(new GISERPFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                GISERP = request,
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

        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
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
            ExternalExchangeTestingFilesDomain.Save(new ExternalExchangeTestingFiles
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                Document = _fileManager.SaveFile(fileName, data),
                ClassDescription = messId,
                ClassName = "ProsecutorOffice",
                User = "Пока не установлен"

            });
        }

        private void SaveFile(XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            ExternalExchangeTestingFiles faultRec = new ExternalExchangeTestingFiles
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                ClassName = "ProsecutorOffice",
                User = "Пока не установлен",
                Document = _fileManager.SaveFile(stream, fileName)
            };

            ExternalExchangeTestingFilesDomain.Save(faultRec);
        }

        private void SaveException(Exception exception)
        {
            if (exception == null)
                return;

            ExternalExchangeTestingFilesDomain.Save(new ExternalExchangeTestingFiles
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                ClassName = "ProsecutorOffice",
                User = "Пока не установлен",
                Document = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeState(GISERP requestData, RequestState state)
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
