using Bars.B4;
using Bars.B4.Config;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
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
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.Gkh.Enums;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    public class GISERPService : IGISERPService
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
        static XNamespace faNamespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.4.0";
        static XNamespace erpNamespace = @"urn://ru.gov.proc.erp.communication/3.0.3";
        static XNamespace erp_typesNamespace = @"urn://ru.gov.proc.erp.communication/types/3.0.3";

        private List<string> notDeletedCodes;

        #endregion

        #region Properties      
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }

        public IDomainService<ProsecutorOffice> ProsecutorOfficeDomain { get; set; }

        public IDomainService<GISERP> GISERPDomain { get; set; }

        public IDomainService<GISERPFile> GISERPFileDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        public IDomainService<ActRemoval> ActRemovalDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRealityObjectDomain { get; set; }

        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }

        public IDomainService<ActCheckWitness> ActCheckWitnessDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }        //ViolationActionsRemovGji

        public IDomainService<ViolationActionsRemovGji> ViolationActionsRemovGjiDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public GISERPService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

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
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
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
                        var createInspectionElement = respElement.Element(erp_typesNamespace + "CreateInspection");
                        var inspectionElement = exportedCheckElement.Element(erp_typesNamespace + "INSPECTION");
                        if (inspectionElement != null)
                        {
                            foreach (XAttribute attr in inspectionElement.Attributes())
                            {
                                if (attr.Name.ToString().ToUpper() == "ERPID")
                                {
                                    requestData.Answer = "Создана проверка ИД = " + attr.Value;
                                    requestData.RequestState = RequestState.ResponseReceived;
                                    GISERPDomain.Update(requestData);
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            string msg = "";
                            var errors = exportedCheckElement.Element(erp_typesNamespace + "Errors");
                            if (errors != null)
                            {
                                foreach (XAttribute attr in errors.Attributes())
                                {
                                    if (attr.Name == "Text")
                                    {
                                        requestData.Answer = "Ошибка данных " + attr.Value;
                                        SetErrorState(requestData, "TryProcessResponse exception: " + attr.Value);
                                    }
                                }
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
                SetErrorState(requestData, "TryProcessResponse exception: " + e.Message);
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
                        .AggregateWithSeparator(x => x.SurveyPurpose.Name, "; ");
                    goals = goals + " Задачи проверки: " + disposalSurveyObjectiveDomain.GetAll()
                        .Where(x => x.Disposal == disposal)
                        .AggregateWithSeparator(x => x.SurveyObjective.Name, "; ");
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
                                    .Where(x => x.Inspection == inspection).FirstOrDefault();
                                inspType = ERPInspectionType.VP;
                                inspectionName = $"Проверка по обращению № {inspAppcit.AppealCits.NumberGji} от {inspAppcit.AppealCits.DateFrom.Value.ToShortDateString()}";

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
                    .AggregateWithSeparator(x => x.ControlActivity.Name, "; ");
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
                            subjectAddress = "Челябинская обл., г. Челябинск, ул. Энгельса, д.43"; // в воронеже все инспекторы сидят в одном месте
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

                if (inspection.Contragent != null)
                {
                    var licenseDomain = this.Container.Resolve<IDomainService<ManOrgLicense>>();
                    var license = licenseDomain.GetAll()
                        .Where(x => x.Contragent == inspection.Contragent)
                        .Where(x => !x.DateTermination.HasValue)
                        .Where(x => x.State.FinalState).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (license != null)
                    {
                        kindKnd = KindKND.LicenseControl;
                    }
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
                prosOffice = ProsecutorOfficeDomain.GetAll().Where(x => x.Name.Trim() == "Прокуратура Челябинской области").FirstOrDefault();
                oktmo = "75000000000";

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
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
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
                    requestData.Answer = "Поставлено в очередь";
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

            var result = new XElement(erpNamespace + "Request",
              new XElement(erp_typesNamespace + "Set",
                 GetInitiationOrCorrection(requestData)

                  )
          );

            result.SetAttributeValue(XNamespace.Xmlns + "erp", erpNamespace);
            result.SetAttributeValue(XNamespace.Xmlns + "erp_types", erp_typesNamespace);

            return result;


        }

        private XElement GetInitiationOrCorrection(GISERP requestData)
        {
            //SPV_26 - жилищный надзор
            //SPV_2 - лицензионный контроль

            string ITYPE_ID = string.Empty;
            string INOTICE_TYPE_ID = string.Empty;
            string IRISK_ID = string.Empty;
            string ISUPERVISION_ID = string.Empty;
            string FRGU_SERV_ID_BK = string.Empty;
            string ADDRESS_TYPE_ID = string.Empty;
            string IREASON_ID = string.Empty;
            if (requestData.ERPInspectionType == ERPInspectionType.PP)
            {
                ITYPE_ID = "PP";
            }
            else
            {
                ITYPE_ID = "VP";
            }
            switch (requestData.ERPNoticeType) // смотрим что явилось основанием
            {
                case ERPNoticeType.TYPE_I: { INOTICE_TYPE_ID = "TYPE_I"; } break;
                case ERPNoticeType.TYPE_II: { INOTICE_TYPE_ID = "TYPE_II"; } break;
                case ERPNoticeType.TYPE_III: { INOTICE_TYPE_ID = "TYPE_III"; } break;
                case ERPNoticeType.TYPE_OTHER: { INOTICE_TYPE_ID = "TYPE_OTHER"; } break;
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
                ISUPERVISION_ID = "SPV_1005";
                FRGU_SERV_ID_BK = "7400000000163062804";
            }
            else
            {
                ISUPERVISION_ID = "SPV_1009";
                FRGU_SERV_ID_BK = "7400000000163262794";
            }

            Disposal disp = DisposalDomain.Get(requestData.Disposal.Id);

            string ICARRYOUT_TYPE_ID = string.Empty;
            if (disp.KindCheck.Code == TypeCheck.InspectionSurvey || disp.KindCheck.Code == TypeCheck.NotPlannedExit || disp.KindCheck.Code == TypeCheck.PlannedExit || disp.KindCheck.Code == TypeCheck.VisualSurvey)
            {
                ICARRYOUT_TYPE_ID = "TYPE_I";
            }
            else if (disp.KindCheck.Code == TypeCheck.NotPlannedDocumentation || disp.KindCheck.Code == TypeCheck.PlannedDocumentation)
            {
                ICARRYOUT_TYPE_ID = "TYPE_II";
            }
            else if (disp.KindCheck.Code == TypeCheck.NotPlannedDocumentationExit || disp.KindCheck.Code == TypeCheck.PlannedDocumentationExit)
            {
                ICARRYOUT_TYPE_ID = "TYPE_III";
            }
            switch (requestData.ERPAddressType)
            {
                case ERPAddressType.TYPE_I: { ADDRESS_TYPE_ID = "TYPE_I"; } break;
                case ERPAddressType.TYPE_II: { ADDRESS_TYPE_ID = "TYPE_II"; } break;
                case ERPAddressType.TYPE_III: { ADDRESS_TYPE_ID = "TYPE_III"; } break;
                case ERPAddressType.TYPE_OTHER: { ADDRESS_TYPE_ID = "TYPE_OTHER"; } break;
            }
            switch (requestData.ERPReasonType)
            {
                case ERPReasonType.RSN_PP_I: { IREASON_ID = "RSN_PP_I"; } break;
                case ERPReasonType.RSN_PP_II: { IREASON_ID = "RSN_PP_II"; } break;
                case ERPReasonType.RSN_PP_IV: { IREASON_ID = "RSN_PP_IV"; } break;
                case ERPReasonType.RSN_PP_OTHER: { IREASON_ID = "RSN_PP_OTHER"; } break;
                case ERPReasonType.RSN_VP_CVI: { IREASON_ID = "RSN_VP_CVI"; } break;
            }
            int DURATION_DAY = 1;

            if (disp.DateStart.HasValue && disp.DateEnd.HasValue)
            {
                TimeSpan t = disp.DateEnd.Value - disp.DateStart.Value;
                if (t.Days > DURATION_DAY)
                {
                    DURATION_DAY = t.Days;
                }
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
                return new XElement(erp_typesNamespace + "Initialization",
                    new XElement(erp_typesNamespace + "INSPECTION",
                        new XAttribute("IS_START_MONTH", false),
                        new XAttribute("ITYPE_ID", ITYPE_ID), //идентификатор платежа, если их несколько
                        new XAttribute("FZ_ID", "294"),
                        new XAttribute("NAME", requestData.InspectionName),
                        //      new XAttribute("NAME", requestData.BillDate.ToString("yyyy-MM-ddTHH:mm:ss.ssszzz")),
                        new XAttribute("PROSEC_ID", requestData.ProsecutorOffice.Code),
                        new XAttribute("START_DATE", disp.DateStart.HasValue ? disp.DateStart.Value.ToString("yyyy-MM-dd") : disp.DocumentDate.Value.ToString("yyyy-MM-dd")),
                        new XElement(erp_typesNamespace + "DOMAIN_OKATO",
                            new XAttribute("OKATO_KOD1", OKATO_KOD1),
                            new XAttribute("OKATO_RAZDEL", OKATO_RAZDEL),
                            new XAttribute("OKATO_KOD2", OKATO_KOD2),
                            new XAttribute("OKATO_KOD3", OKATO_KOD3),
                            new XAttribute("OKATO_TER", "75"),
                            new XAttribute("FEDERAL_DISTRICT", "034")
                        ),
                        new XElement(erp_typesNamespace + "I_AUTHORITY",
                            new XAttribute("ADDRESS", "454091, г. Челябинск, ул. Энгельса, д.43"),
                            new XAttribute("FRGU_ORG_ID_BK", "7400000010000214395"),
                            new XAttribute("IS_MAIN", true),
                            new XElement(erp_typesNamespace + "I_AUTHORITY_SERV",
                                new XAttribute("IS_MAIN", true),
                                new XAttribute("FRGU_SERV_ID_BK", FRGU_SERV_ID_BK)
                                ),
                              //new XElement(erp_typesNamespace + "I_AUTHORITY_SERV",
                              //   new XAttribute("IS_MAIN", true),
                              //   new XAttribute("FRGU_SERV_ID_BK", "3640000010000026981")
                              //   ),
                              GetInspectorsFromDisposal(disp)
                            ),
                        new XElement(erp_typesNamespace + "I_CLASSIFICATION",
                           // new XAttribute("INOTICE_TYPE_ID", INOTICE_TYPE_ID),
                            new XAttribute("ISUPERVISION_ID", ISUPERVISION_ID),
                         //   new XAttribute("IRISK_ID", IRISK_ID),
                             disp.NcDate.HasValue ?
                            new XAttribute("INOTICE_DATE", disp.NcDate.HasValue ? disp.NcDate.Value.ToString("yyyy-MM-dd") : disp.DocumentDate.Value.ToString("yyyy-MM-dd")) : null,
                            new XAttribute("ICARRYOUT_TYPE_ID", ICARRYOUT_TYPE_ID)
                            ),
                         GetSubject(disp),
                          new XElement(erp_typesNamespace + "I_OBJECT",
                            new XAttribute("ADDRESS", requestData.SubjectAddress),
                            new XAttribute("ADDRESS_TYPE_ID", ADDRESS_TYPE_ID),
                            new XAttribute("IS_MAIN", true),
                            new XAttribute("IOBJECT_TYPE_ID", "TYPE_OTHER"),
                            GetResult(disp, ICARRYOUT_TYPE_ID, requestData.SubjectAddress, ADDRESS_TYPE_ID)
                            ),
                           new XElement(erp_typesNamespace + "I_APPROVE",
                            new XAttribute("DURATION_DAY", DURATION_DAY),
                            new XAttribute("END_DATE", disp.DateEnd.HasValue ? disp.DateEnd.Value.ToString("yyyy-MM-dd") : disp.DocumentDate.Value.AddDays(20).ToString("yyyy-MM-dd")),
                            new XAttribute("INSP_TARGET", requestData.Goals),
                            new XElement(erp_typesNamespace + "I_APPROVE_DOCS",
                                new XAttribute("DOC_DATE", disp.DocumentDate.Value.ToString("yyyy-MM-dd")),
                                new XAttribute("IAPPROVE_DOC_LINK_ID", "PP_ORDER"),
                                new XAttribute("IS_MAIN", true),
                                new XAttribute("DOC_ATR", disp.DocumentNumber)
                                ),
                            new XElement(erp_typesNamespace + "I_CARRYOUT_EVENTS",
                                new XAttribute("EVENTS_TEXT", requestData.CarryoutEvents),
                                new XAttribute("IS_MAIN", true)//IREASON_ID
                                ),
                            new XElement(erp_typesNamespace + "I_REASON",
                                new XAttribute("REASON_TEXT", requestData.InspectionName),
                                new XAttribute("IS_MAIN", true),
                                new XAttribute("IREASON_ID", IREASON_ID)
                                )
                            ),
                            new XElement(erp_typesNamespace + "I_PUBLISH",
                             new XElement(erp_typesNamespace + "I_PUBLISH_STATUS", "ASK_TO_PUBLISH")
                            )

                        )
                    );
            else
                return null;
        }


        #endregion

        #region Private methods

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
                    newElement.Add(new XElement(erp_typesNamespace + "I_INSPECTOR",
                               new XAttribute("IS_MAIN", i == 1 ? true : false),
                               new XAttribute("FULL_NAME", x.Fio),
                               new XAttribute("POSITION", x.Position + " " + zonal.ZonalInspection.NameGenetive),
                               new XAttribute("INSPECTOR_TYPE_ID", "TYPE_I")
                               ));
                    i++;
                });



            return newElement;
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
                    newElement.Add(new XElement(erp_typesNamespace + "I_RESULT_INSPECTOR",
                               new XAttribute("IS_MAIN", i == 1 ? true : false),
                               new XAttribute("FULL_NAME", x.Fio),
                               new XAttribute("POSITION", x.Position + " " + zonal.ZonalInspection.NameGenetive),
                               new XAttribute("INSPECTOR_TYPE_ID", "TYPE_I")
                               ));
                    i++;
                });



            return newElement;
        }

        private XElement GetSubject(Disposal diaposal)
        {

            var contragent = diaposal.Inspection.Contragent;
            string ISubjectType = "UL_IP";

            if (contragent != null)
            {
                var localGovDomain = this.Container.Resolve<IDomainService<LocalGovernment>>();
                var omsu = localGovDomain.GetAll()
                    .Where(x => x.Contragent == contragent).FirstOrDefault();
                if (omsu != null)
                {
                    ISubjectType = "OMS";
                }
                else
                {
                    var ukDomain = this.Container.Resolve<IDomainService<ManagingOrganization>>();
                    var uk = ukDomain.GetAll()
                        .Where(x => x.Contragent == contragent).FirstOrDefault();
                    if (uk != null)
                    {
                        ISubjectType = "UL_IP";
                    }
                    else
                    {
                        var politicauthoritykDomain = this.Container.Resolve<IDomainService<PoliticAuthority>>();
                        var politicauthority = politicauthoritykDomain.GetAll()
                            .Where(x => x.Contragent == contragent).FirstOrDefault();
                        if (politicauthority != null)
                        {
                            ISubjectType = "OGV";
                        }
                    }
                }

                return new XElement(erp_typesNamespace + "I_SUBJECT",
                            new XAttribute("ISUBJECT_TYPE_ID", ISubjectType),
                            new XAttribute("OGRN", contragent.Ogrn),
                            new XAttribute("INN", contragent.Inn),
                            new XAttribute("ORG_NAME", contragent.Name)
                            );
            }
            else
            {
                return null;
            }




        }

        private List<XElement> GetResult(Disposal disposal, string ICARRYOUT_TYPE_ID, string SubjectAddress, string ADDRESS_TYPE_ID)
        {
            List<XElement> newElementList = new List<XElement>();
            string ACT_ADDRESS_TYPE_ID = ADDRESS_TYPE_ID;
            if (ICARRYOUT_TYPE_ID == "TYPE_II")
            {
                SubjectAddress = "454091, г. Челябинск, ул. Энгельса, д.43";
                ACT_ADDRESS_TYPE_ID = "TYPE_OTHER";
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



                    XElement newElem = new XElement(erp_typesNamespace + "I_RESULT",
                    new XAttribute("ACT_DATE_CREATE", ACT_DATE_CREATE),
                    new XAttribute("ACT_ADDRESS", ACT_ADDRESS),
                    new XAttribute("ACT_ADDRESS_TYPE_ID", ADDRESS_TYPE_ID)
                    );
                    var actCheckWitness = ActCheckWitnessDomain.GetAll()
                        .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                    if (actCheckWitness != null)
                    {
                        if (actCheckWitness.IsFamiliar)
                        {
                            newElem.Add(new XAttribute("ACT_WAS_NOT_READ", false));
                            newElem.Add(new XAttribute("ACT_WAS_NOT_READ_NOTE", actCheckWitness.Position + ", " + actCheckWitness.Fio));
                        }
                        else
                        {
                            newElem.Add(new XAttribute("ACT_WAS_NOT_READ", true));
                            newElem.Add(new XAttribute("ACT_WAS_NOT_READ_NOTE", actCheckWitness.Position + ", " + actCheckWitness.Fio));
                        }
                    }
                    var actCheckPeriod = ActCheckPeriodDomain.GetAll()
                        .Where(x => x.ActCheck == actCheck).FirstOrDefault();

                    //if (actCheckPeriod.DateStart != null && actCheckPeriod.DateEnd != null)
                    //{
                    //    try
                    //    {
                    //        int DURATION_DAYS = 1;
                    //        string START_DATE = actCheckPeriod.DateStart.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    //        var convertedDate = actCheckPeriod.DateEnd.Value.Subtract(actCheckPeriod.DateStart.Value).Hours;
                    //        if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                    //        {
                    //            START_DATE = disposal.DateStart.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    //        }
                    //        int DURATION_HOURS = convertedDate;
                    //        if (DURATION_HOURS == 0)
                    //        {
                    //            DURATION_HOURS = 1;
                    //        }
                    //        newElem.Add(new XAttribute("START_DATE", START_DATE));
                    //        newElem.Add(new XAttribute("DURATION_HOURS", DURATION_HOURS));
                    //    }
                    //    catch { }
                    //}
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

                            newElem.Add(new XAttribute("START_DATE", START_DATE));
                            newElem.Add(new XAttribute("DURATION_DAYS", DURATION_DAYS));
                        }
                        catch { }
                    }

                    newElem.Add(GetInspectorsResultFromDisposal(disposal));
                    var actcheckRealityObject = ActCheckRealityObjectDomain.GetAll()
                        .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                    if (actcheckRealityObject != null)
                    {
                        if (actcheckRealityObject.HaveViolation != YesNoNotSet.Yes)
                        {
                            newElem.Add(new XAttribute("ABSENT_VIOLATION_NOTE", "Нарушений не выявлено"));
                            List<XElement> violationList = new List<XElement>();
                            XElement violElem = new XElement(erp_typesNamespace + "I_VIOLATION",
                                new XAttribute("IS_MAIN", true),
                                  new XAttribute("VIOLATION_NOTE", "Нарушений не выявлено"),
                                new XAttribute("IVIOLATION_TYPE_ID", "TYPE_OTHER")
                                    );
                            violationList.Add(violElem);
                            newElem.Add(violationList);
                        }
                        else
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
                                    string IVIOLATION_TYPE_ID = "TYPE_I";
                                    XElement violElem = new XElement(erp_typesNamespace + "I_VIOLATION",
                                     new XAttribute("VIOLATION_NOTE", VIOLATION_NOTE),
                                     new XAttribute("IS_MAIN", true),
                                     new XAttribute("VIOLATION_ACT", viol.Violation.NormativeDocNames),
                                     new XAttribute("IVIOLATION_TYPE_ID", IVIOLATION_TYPE_ID)
                                         );
                                    var inspViolationStages = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                                         .Where(x => x.InspectionViolation == viol).ToList();
                                    foreach (InspectionGjiViolStage stage in inspViolationStages)
                                    {
                                        if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                                        {
                                            string CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                                            string DATE_APPOINTMENT = stage.Document.DocumentDate.HasValue ? stage.Document.DocumentDate.Value.ToString("yyyy-MM-dd") : stage.Document.ObjectCreateDate.ToString("yyyy-MM-dd");
                                            XElement presctElement = new XElement(erp_typesNamespace + "V_INJUNCTION",
                                                  new XAttribute("CODE", CODE),
                                                   new XAttribute("NOTE", NOTE),
                                                   new XAttribute("DATE_APPOINTMENT", DATE_APPOINTMENT),
                                                   stage.DateFactRemoval.HasValue ? new XAttribute("EXECUTION_NOTE", "Выполнено") : null,
                                                    stage.DatePlanRemoval.HasValue ? new XAttribute("EXECUTION_DEADLINE", stage.DatePlanRemoval.Value.ToString("yyyy-MM-dd")) : null,
                                                    new XAttribute("IS_MAIN", true)
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
