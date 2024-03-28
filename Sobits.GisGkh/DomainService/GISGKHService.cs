using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.Suggestion;
using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.GkhCr.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;
using GisGkhLibrary.HcsCapitalRepairAsync;
using GisGkhLibrary.HouseManagementAsync;
using GisGkhLibrary.NsiCommonAsync;
using GisGkhLibrary.NsiServiceAsync;
using GisGkhLibrary.RegOrgCommonAsync;
using GisGkhLibrary.Services;
using Ionic.Zip;
using Ionic.Zlib;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class GISGKHService : IGISGKHService
    {
        #region Constants



        #endregion

        #region Properties              

        public IRepository<RealityObject> RealityObjectRepo { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IDomainService<GisGkhPremises> GisGkhPremisesDomain { get; set; }

        public IDomainService<GisGkhDownloads> GisGkhDownloadsDomain { get; set; }

        public IDomainService<AttachmentField> AttachmentFieldDomain { get; set; }

        public IDomainService<AppealCitsAttachment> AppealCitsAttachmentDomain { get; set; }
        public IDomainService<CitizenSuggestionFiles> CitizenSuggestionFilesDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<Decision> DecisionDomain { get; set; }
        public IDomainService<Resolution> ResolutionDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<Protocol> ProtocolDomain { get; set; }
        public IDomainService<ProgramCr> ProgramDomain { get; set; }
        public IDomainService<BuildContract> ContractDomain { get; set; }
        public IDomainService<PerformedWorkAct> WorkActDomain { get; set; }
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }
        public IDomainService<BuildContractTypeWork> TypeWorkCrDomain { get; set; }
        public IRepository<AppealCits> AppealCitsRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        public IGkhUserManager _userManager { get; set; }
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        private IFileService _fileService;

        #endregion

        #region Constructors

        public GISGKHService(IFileManager fileManager, IFileService fileService, IGkhUserManager userManager)
        {
            _fileManager = fileManager;
            _fileService = fileService;
            _userManager = userManager;
    }

        #endregion

        #region Public methods

        public string GetXML(long reqId)
        {
            GisGkhRequests req = GisGkhRequestsDomain.GetAll()
                .Where(x => x.Id == reqId)
                .FirstOrDefault();
            if (req != null && (req.RequestState == GisGkhRequestState.Formed || req.RequestState == GisGkhRequestState.Error))
            {
                try
                {
                    var fileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.request)
                        .FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(fileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        return Encoding.UTF8.GetString(data);
                    }
                    else
                    {
                        throw new Exception("Не найден неподписанный запрос");
                    }
                }
                catch (Exception e)
                {
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка при получении неподписанного запроса: " + e.Message);
                }
            }
            else
            {
                //req.RequestState = GisGkhRequestState.Error;
                //GisGkhRequestsDomain.Update(req);
                throw new Exception("Запрос не находится в статусе \"Пакет сформирован\"");
            }
        }

        public void SaveSignedXML(long reqId, string signedData)
        {
            if (GisGkhRequestsDomain == null)
            {
                throw new Exception("Ошибка при сохранении подписанного запроса: GisGkhRequestsDomain null");
            }
            GisGkhRequests req = GisGkhRequestsDomain.GetAll()
                .Where(x => x.Id == reqId)
                .FirstOrDefault();
            if (req != null && (req.RequestState == GisGkhRequestState.Formed || req.RequestState == GisGkhRequestState.Error))
            {
                try
                {
                    var signedXml = new XmlDocument();
                    signedXml.LoadXml(Uri.UnescapeDataString(signedData));
                    var signedFiles = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhFileType == GisGkhFileType.signedRequest)
                        .Where(x => x.GisGkhRequests == req).ToList();
                    foreach (var signedFile in signedFiles)
                    {
                        GisGkhRequestsFileDomain.Delete(signedFile.Id);
                        _fileManager.Delete(signedFile.FileInfo);
                    }
                    SaveFile(req, GisGkhFileType.signedRequest, signedXml, "signedRequest.xml");
                    req.RequestState = GisGkhRequestState.Signed;
                    GisGkhRequestsDomain.Update(req);
                    //return true;
                }
                catch (Exception e)
                {
                    req.RequestState = GisGkhRequestState.Error;
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка при сохранении подписанного запроса: " + e.Message + e.StackTrace);
                }
                //return SendRequest(baseParams, reqId);
            }
            else
            {
                throw new Exception("Запрос не находится в статусе \"Пакет сформирован\"");
            }
        }

        public bool SendRequest(long reqId)
        {
            if (GisGkhRequestsDomain == null)
            {
                throw new Exception("GisGkhRequestsDomain == null");
            }
            if (reqId ==0)
            {
                throw new Exception("reqId == 0");
            }
            GisGkhRequests req = GisGkhRequestsDomain.GetAll()
                .Where(x => x.Id == reqId)
                .FirstOrDefault();
            if (req != null && (req.RequestState == GisGkhRequestState.Formed) || (req.RequestState == GisGkhRequestState.Signed))
            {
                try
                {
                    var fileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.signedRequest)
                        .FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(fileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        string xml = Encoding.UTF8.GetString(data);
                        try
                        {
                            var orgPPAGUID = "";
                            Operator thisOperator = _userManager.GetActiveOperator();
                            if (thisOperator.GisGkhContragent != null)
                            {
                                orgPPAGUID = thisOperator.GisGkhContragent.GisGkhOrgPPAGUID ?? "";
                            }
                            switch (req.TypeRequest)
                            {
                                case GisGkhTypeRequest.exportNsiList:
                                case GisGkhTypeRequest.exportNsiRaoList:
                                    exportNsiListRequest nsiListRequest = DeserializeData<exportNsiListRequest>(Uri.UnescapeDataString(xml));
                                    var nsiListResp = NsiServiceCommonAsync.exportNsiListSend(nsiListRequest);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(nsiListResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = nsiListResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = nsiListResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportNsiItems:
                                    exportNsiItemRequest nsiItemRequest = DeserializeData<exportNsiItemRequest>(Uri.UnescapeDataString(xml));
                                    var nsiItemResp = NsiServiceCommonAsync.exportNsiItemSend(nsiItemRequest);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(nsiItemResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = nsiItemResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = nsiItemResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportNsiPagingItems:
                                    exportNsiPagingItemRequest nsiPagingItemRequest = DeserializeData<exportNsiPagingItemRequest>(Uri.UnescapeDataString(xml));
                                    var nsiPagingItemResp = NsiServiceCommonAsync.exportNsiPagingItemSend(nsiPagingItemRequest);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(nsiPagingItemResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = nsiPagingItemResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = nsiPagingItemResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportDataProviderNsiItem:
                                    exportDataProviderNsiItemRequest nsiDataProviderItemRequest = DeserializeData<exportDataProviderNsiItemRequest>(Uri.UnescapeDataString(xml));
                                    var nsiDataProviderItemResp = NsiServiceAsync.exportDataProviderNsiItemSend(nsiDataProviderItemRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(nsiDataProviderItemResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = nsiDataProviderItemResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = nsiDataProviderItemResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportBriefApartmentHouse:
                                    exportBriefApartmentHouseRequest briefApartmentHouseRequest = DeserializeData<exportBriefApartmentHouseRequest>(Uri.UnescapeDataString(xml));
                                    var briefApartmentHouseResp = HouseManagementAsyncService.exportBriefApartmentHouseSend(briefApartmentHouseRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(briefApartmentHouseResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = briefApartmentHouseResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = briefApartmentHouseResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportHouseData:
                                    exportHouseRequest houseDataRequest = DeserializeData<exportHouseRequest>(Uri.UnescapeDataString(xml));
                                    var houseDataResp = HouseManagementAsyncService.exportHouseDataSend(houseDataRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(houseDataResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = houseDataResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = houseDataResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportAccountData:
                                    GisGkhLibrary.HouseManagementAsync.exportAccountRequest exportAccountRequest = DeserializeData<GisGkhLibrary.HouseManagementAsync.exportAccountRequest>(Uri.UnescapeDataString(xml));
                                    var accountDataResp = HouseManagementAsyncService.exportAccountDataSend(exportAccountRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(accountDataResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = accountDataResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = accountDataResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportOrgRegistry:
                                    exportOrgRegistryRequest orgRegistryRequest = DeserializeData<exportOrgRegistryRequest>(Uri.UnescapeDataString(xml));
                                    var orgRegistryResp = RegOrgCommonAsync.exportOrgRegistrySend(orgRegistryRequest);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(orgRegistryResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = orgRegistryResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = orgRegistryResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportRegionalProgram:
                                    exportRegionalProgramRequest exportRegionalProgramRequest = DeserializeData<exportRegionalProgramRequest>(Uri.UnescapeDataString(xml));
                                    var regionalProgramResp = HcsCapitalRepairAsync.exportRegionalProgramSend(exportRegionalProgramRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(regionalProgramResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = regionalProgramResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = regionalProgramResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportPlan:
                                    exportPlanRequest exportPlanRequest = DeserializeData<exportPlanRequest>(Uri.UnescapeDataString(xml));
                                    var planResp = HcsCapitalRepairAsync.exportPlanSend(exportPlanRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(planResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = planResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = planResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportPlanWork:
                                    exportPlanWorkRequest exportPlanWorkRequest = DeserializeData<exportPlanWorkRequest>(Uri.UnescapeDataString(xml));
                                    var planWorkResp = HcsCapitalRepairAsync.exportPlanWorkSend(exportPlanWorkRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(planWorkResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = planWorkResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = planWorkResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importAccountData:
                                    GisGkhLibrary.HouseManagementAsync.importAccountRequest importAccountRequest = DeserializeData<GisGkhLibrary.HouseManagementAsync.importAccountRequest>(Uri.UnescapeDataString(xml));
                                    var importAccountDataResp = HouseManagementAsyncService.importAccountDataSend(importAccountRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importAccountDataResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importAccountDataResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importAccountDataResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportExaminations:
                                    GisGkhLibrary.InspectionServiceAsync.exportExaminationsRequest exportExaminationsRequest = DeserializeData<GisGkhLibrary.InspectionServiceAsync.exportExaminationsRequest>(Uri.UnescapeDataString(xml));
                                    var exportExaminationsResp = InspectionServiceAsync.exportExaminationsSend(exportExaminationsRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportExaminationsResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportExaminationsResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportExaminationsResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importExaminations:
                                    GisGkhLibrary.InspectionServiceAsync.importExaminationsRequest importExaminationsRequest = DeserializeData<GisGkhLibrary.InspectionServiceAsync.importExaminationsRequest>(Uri.UnescapeDataString(xml));
                                    var importExaminationsResp = InspectionServiceAsync.importExaminationsSend(importExaminationsRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importExaminationsResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importExaminationsResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importExaminationsResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importCurrentExaminations:
                                    GisGkhLibrary.InspectionServiceAsync.importExaminationsRequest importCurrentExaminationsRequest = DeserializeData<GisGkhLibrary.InspectionServiceAsync.importExaminationsRequest>(Uri.UnescapeDataString(xml));
                                    var importCurrentExaminationsResp = InspectionServiceAsync.importExaminationsSend(importCurrentExaminationsRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importCurrentExaminationsResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importCurrentExaminationsResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importCurrentExaminationsResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportInspectionPlans:
                                    GisGkhLibrary.InspectionServiceAsync.exportInspectionPlansRequest exportInspectionPlansRequest = DeserializeData<GisGkhLibrary.InspectionServiceAsync.exportInspectionPlansRequest>(Uri.UnescapeDataString(xml));
                                    var exportInspectionPlansResp = InspectionServiceAsync.exportInspectionPlansSend(exportInspectionPlansRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportInspectionPlansResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportInspectionPlansResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportInspectionPlansResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importInspectionPlan:
                                    GisGkhLibrary.InspectionServiceAsync.importInspectionPlanRequest importInspectionPlanRequest = DeserializeData<GisGkhLibrary.InspectionServiceAsync.importInspectionPlanRequest>(Uri.UnescapeDataString(xml));
                                    var importInspectionPlanResp = InspectionServiceAsync.importInspectionPlanSend(importInspectionPlanRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importInspectionPlanResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importInspectionPlanResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importInspectionPlanResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportDecisionsFormingFund:
                                    GisGkhLibrary.HcsCapitalRepairAsync.exportDecisionsFormingFundRequest exportDecisionsFormingFundRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.exportDecisionsFormingFundRequest>(Uri.UnescapeDataString(xml));
                                    var exportDecisionsFormingFundResp = HcsCapitalRepairAsync.exportDecisionsFormingFundSend(exportDecisionsFormingFundRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportDecisionsFormingFundResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportDecisionsFormingFundResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportDecisionsFormingFundResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importDecisionsFormingFund:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importDecisionsFormingFundRequest importDecisionsFormingFundRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importDecisionsFormingFundRequest>(Uri.UnescapeDataString(xml));
                                    var importDecisionsFormingFundResp = HcsCapitalRepairAsync.importDecisionsFormingFundSend(importDecisionsFormingFundRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importDecisionsFormingFundResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importDecisionsFormingFundResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importDecisionsFormingFundResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportPaymentDocumentData:
                                    GisGkhLibrary.BillsServiceAsync.exportPaymentDocumentRequest exportPaymentDocumentRequest = DeserializeData<GisGkhLibrary.BillsServiceAsync.exportPaymentDocumentRequest>(Uri.UnescapeDataString(xml));
                                    var exportPaymentDocumentResp = BillServiceAsync.exportPaymentDocumentDataSend(exportPaymentDocumentRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportPaymentDocumentResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportPaymentDocumentResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportPaymentDocumentResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importPaymentDocumentData:
                                    GisGkhLibrary.BillsServiceAsync.importPaymentDocumentRequest importPaymentDocumentRequest = DeserializeData<GisGkhLibrary.BillsServiceAsync.importPaymentDocumentRequest>(Uri.UnescapeDataString(xml));
                                    var importPaymentDocumentResp = BillServiceAsync.importPaymentDocumentDataSend(importPaymentDocumentRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importPaymentDocumentResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importPaymentDocumentResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importPaymentDocumentResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importPlan:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importPlanRequest importPlanRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importPlanRequest>(Uri.UnescapeDataString(xml));
                                    var importPlanResp = HcsCapitalRepairAsync.importPlanSend(importPlanRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importPlanResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importPlanResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importPlanResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importPlanWork:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importPlanWorkRequest importPlanWorkRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importPlanWorkRequest>(Uri.UnescapeDataString(xml));
                                    var importPlanWorkResp = HcsCapitalRepairAsync.importPlanWorkSend(importPlanWorkRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importPlanWorkResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importPlanWorkResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importPlanWorkResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importBuildContract:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importContractsRequest importContractsRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importContractsRequest>(Uri.UnescapeDataString(xml));
                                    var importContractsResp = HcsCapitalRepairAsync.importBuildContractSend(importContractsRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importContractsResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importContractsResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importContractsResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importPerfWorkAct:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importCertificatesRequest importCertificatesRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importCertificatesRequest>(Uri.UnescapeDataString(xml));
                                    var importCertificatesResp = HcsCapitalRepairAsync.importPerfWorkActSend(importCertificatesRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importCertificatesResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importCertificatesResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importCertificatesResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importRegionalProgram:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importRegionalProgramRequest importRegionalProgramRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importRegionalProgramRequest>(Uri.UnescapeDataString(xml));
                                    var importRegionalProgramResp = HcsCapitalRepairAsync.importRegionalProgramSend(importRegionalProgramRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importRegionalProgramResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importRegionalProgramResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importRegionalProgramResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importRegionalProgramWork:
                                    GisGkhLibrary.HcsCapitalRepairAsync.importRegionalProgramWorkRequest importRegionalProgramWorkRequest = DeserializeData<GisGkhLibrary.HcsCapitalRepairAsync.importRegionalProgramWorkRequest>(Uri.UnescapeDataString(xml));
                                    var importRegionalProgramWorkResp = HcsCapitalRepairAsync.importRegionalProgramWorkSend(importRegionalProgramWorkRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importRegionalProgramWorkResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importRegionalProgramWorkResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importRegionalProgramWorkResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportDecreesAndDocumentsData:
                                    GisGkhLibrary.RapServiceAsync.ExportDecreesAndDocumentsRequest exportDecreesAndDocumentsRequest = DeserializeData<GisGkhLibrary.RapServiceAsync.ExportDecreesAndDocumentsRequest>(Uri.UnescapeDataString(xml));
                                    var exportDecreesAndDocumentsDataResp = RapServiceAsync.exportDecreesAndDocumentsDataSend(exportDecreesAndDocumentsRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportDecreesAndDocumentsDataResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportDecreesAndDocumentsDataResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportDecreesAndDocumentsDataResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importDecreesAndDocumentsData:
                                    GisGkhLibrary.RapServiceAsync.ImportDecreesAndDocumentsRequest importDecreesAndDocumentsRequest = DeserializeData<GisGkhLibrary.RapServiceAsync.ImportDecreesAndDocumentsRequest>(Uri.UnescapeDataString(xml));
                                    var importDecreesAndDocumentsDataResp = RapServiceAsync.importDecreesAndDocumentsDataSend(importDecreesAndDocumentsRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importDecreesAndDocumentsDataResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importDecreesAndDocumentsDataResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importDecreesAndDocumentsDataResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportLicense:
                                    GisGkhLibrary.LicenseServiceAsync.exportLicenseRequest exportLicenseRequest = DeserializeData<GisGkhLibrary.LicenseServiceAsync.exportLicenseRequest>(Uri.UnescapeDataString(xml));
                                    var exportLicenseResp = LicenseServiceAsync.exportLicenseSend(exportLicenseRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportLicenseResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportLicenseResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportLicenseResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportAppeal:
                                case GisGkhTypeRequest.exportAppealCR:
                                    GisGkhLibrary.AppealsServiceAsync.exportAppealRequest exportAppealRequest = DeserializeData<GisGkhLibrary.AppealsServiceAsync.exportAppealRequest>(Uri.UnescapeDataString(xml));
                                    var exportAppealResp = AppealsServiceAsync.exportAppealSend(exportAppealRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportAppealResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportAppealResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportAppealResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.exportDebtRequests:
                                    GisGkhLibrary.DebtRequestAsync.exportDSRsRequest exportDebtRequest = DeserializeData<GisGkhLibrary.DebtRequestAsync.exportDSRsRequest>(Uri.UnescapeDataString(xml));
                                    var exportDebtResp = DRsServiceAsync.exportDebtRequestSend(exportDebtRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(exportDebtResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = exportDebtResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = exportDebtResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importDebtRequests:
                                    GisGkhLibrary.DebtRequestAsync.importDSRResponsesRequest importDebtRequest = DeserializeData<GisGkhLibrary.DebtRequestAsync.importDSRResponsesRequest>(Uri.UnescapeDataString(xml));
                                    var importDebtResp = DRsServiceAsync.exportDebtResponceSend(importDebtRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importDebtResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importDebtResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importDebtResp.Ack.MessageGUID;
                                    break;
                                case GisGkhTypeRequest.importAnswer:
                                case GisGkhTypeRequest.importAnswerCR:
                                    GisGkhLibrary.AppealsServiceAsync.importAnswerRequest importAnswerRequest = DeserializeData<GisGkhLibrary.AppealsServiceAsync.importAnswerRequest>(Uri.UnescapeDataString(xml));
                                    var importAnswerResp = AppealsServiceAsync.importAnswerSend(importAnswerRequest, orgPPAGUID);
                                    SaveFile(req, GisGkhFileType.requestStateAnswer, SerializeRequest(importAnswerResp), "requestResponse.xml");
                                    req.RequesterMessageGUID = importAnswerResp.Ack.RequesterMessageGUID;
                                    req.MessageGUID = importAnswerResp.Ack.MessageGUID;
                                    break;
                                default:
                                    throw new Exception("Неизвестный тип запроса");
                            }
                            req.RequestState = GisGkhRequestState.Queued;
                            req.Operator = thisOperator;
                            GisGkhRequestsDomain.Update(req);
                        }
                        catch (Exception e)
                        {
                            throw new Exception($"Ошибка при отправке запроса: {e.Message}\r\nStackTrace: {e.StackTrace}\r\nInnerException: " +
                                $"{e.InnerException?.Message}\r\nInnerException StackTrace: {e.InnerException?.StackTrace}");
                        }
                        return true;
                    }
                    else
                    {
                        throw new Exception("Не найден подписанный запрос");
                    }
                }
                catch (Exception e)
                {
                    req.RequestState = GisGkhRequestState.Error;
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: " + e.Message + e.StackTrace);
                }
            }
            else
            {
                throw new Exception("Ошибка получения запроса");
            }
        }

        public IDataResult ListDisposalAndDecisionForGisGkh(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            //var data = DisposalDomain.GetAll()
            //    .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "")
            //    .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // пока только новые проверки
            //    .Where(x => x.State.FinalState) // в конечном статусе
            //    .Select(x => new
            //    {
            //        x.Id,
            //        x.DocumentNumber,
            //        State = GisGkhExaminationState.notSent
            //    }).Filter(loadParams, Container);


            //var inProc = DisposalDomain.GetAll()
            //    .Where(x => x.GisGkhTransportGuid != null && x.GisGkhTransportGuid != "")
            //    .Where(x => x.State.FinalState) // в конечном статусе
            //    .Select(x => new
            //    {
            //        x.Id,
            //        x.DocumentNumber,
            //        State = GisGkhExaminationState.inProcess
            //    })
            //    .Filter(loadParams, Container).AsEnumerable();


            // Собираем все варианты по распоряжением
            var dispNotSent = DisposalDomain.GetAll()
                //g .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "") // нет гуида - не отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.notSent,
                    TypeDocumentGji = TypeDocumentGji.Disposal
                })
                .Filter(loadParams, Container).AsEnumerable();

            var dispSentWithoutResult = DisposalDomain.GetAll()
                //.Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "") // есть гуид - проверка отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Where(x => DocumentGjiChildrenDomain.GetAll().Where(y => y.Parent == x)
                            .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                            .Where(y => y.Children.State.FinalState) // в конечном статусе
                            .Any(y => y.Children.GisGkhGuid == null || y.Children.GisGkhGuid == "") // есть документ результата без гуида
                                )
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.sentWithoutResult,
                    TypeDocumentGji = TypeDocumentGji.Disposal
                })
                .Filter(loadParams, Container).AsEnumerable();

            var dispSentWithoutPrescr = DisposalDomain.GetAll()
            //    .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "") // есть гуид - проверка отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Where(x => PrescriptionDomain.GetAll()
                            .Where(y => y.Inspection == x.Inspection && y.DocumentNumber.Contains(x.DocumentNumber))
                            .Where(y => y.GisGkhTransportGuid == null || y.GisGkhTransportGuid == "") // предписания без транспорт гуидов
                            .Where(y => y.GisGkhGuid == null || y.GisGkhGuid == "") // и без гуидов
                            .Where(y => y.State.FinalState) // в конечном статусе
                            .Any())
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.sentWithoutDocs,
                    TypeDocumentGji = TypeDocumentGji.Disposal
                })
                .Filter(loadParams, Container).AsEnumerable();

            var dispSentWithoutProt = DisposalDomain.GetAll()
            //    .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "") // есть гуид - проверка отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Where(x => ProtocolDomain.GetAll()
                            .Where(y => y.Inspection == x.Inspection && y.DocumentNumber.Contains(x.DocumentNumber))
                            .Where(y => y.GisGkhTransportGuid == null || y.GisGkhTransportGuid == "") // протоколы без транспорт гуидов
                            .Where(y => y.GisGkhGuid == null || y.GisGkhGuid == "") // и без гуидов
                            .Where(y => y.State.FinalState) // в конечном статусе
                            .Any())
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.sentWithoutDocs,
                    TypeDocumentGji = TypeDocumentGji.Disposal
                })
                .Filter(loadParams, Container).AsEnumerable();

            // Собираем все варианты по решениям
            var decNotSent = DecisionDomain.GetAll()
                //.Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "") // нет гуида - не отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.notSent,
                    TypeDocumentGji = TypeDocumentGji.Decision
                })
                .Filter(loadParams, Container).AsEnumerable();

            var decSentWithoutResult = DecisionDomain.GetAll()
                //.Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "") // есть гуид - проверка отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Where(x => DocumentGjiChildrenDomain.GetAll().Where(y => y.Parent == x)
                            .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.ActCheck)
                            .Where(y => y.Children.State.FinalState) // в конечном статусе
                            .Any(y => y.Children.GisGkhGuid == null || y.Children.GisGkhGuid == "")) // есть документ результата без гуида
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.sentWithoutResult,
                    TypeDocumentGji = TypeDocumentGji.Decision
                })
                .Filter(loadParams, Container).AsEnumerable();

            var decSentWithoutPrescr = DecisionDomain.GetAll()
                //.Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "") // есть гуид - проверка отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Where(x => PrescriptionDomain.GetAll()
                            .Where(y => y.Inspection == x.Inspection && y.DocumentNumber.Contains(x.DocumentNumber))
                            .Where(y => y.GisGkhTransportGuid == null || y.GisGkhTransportGuid == "") // предписания без транспорт гуидов
                            .Where(y => y.GisGkhGuid == null || y.GisGkhGuid == "") // и без гуидов
                            .Where(y => y.State.FinalState) // в конечном статусе
                            .Any())
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.sentWithoutDocs,
                    TypeDocumentGji = TypeDocumentGji.Decision
                })
                .Filter(loadParams, Container).AsEnumerable();

            var decSentWithoutProt = DecisionDomain.GetAll()
                //.Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "") // есть гуид - проверка отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Where(x => ProtocolDomain.GetAll()
                            .Where(y => y.Inspection == x.Inspection && y.DocumentNumber.Contains(x.DocumentNumber))
                            .Where(y => y.GisGkhTransportGuid == null || y.GisGkhTransportGuid == "") // протоколы без транспорт гуидов
                            .Where(y => y.GisGkhGuid == null || y.GisGkhGuid == "") // и без гуидов
                            .Where(y => y.State.FinalState) // в конечном статусе
                            .Any())
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.sentWithoutDocs,
                    TypeDocumentGji = TypeDocumentGji.Decision
                })
                .Filter(loadParams, Container).AsEnumerable();

            //Объединяем все
            var data = dispNotSent
                .Union(dispSentWithoutResult)
                .Union(dispSentWithoutPrescr)
                .Union(dispSentWithoutProt)
                .Union(decNotSent)
                .Union(decSentWithoutResult)
                .Union(decSentWithoutPrescr)
                .Union(decSentWithoutProt)
                .AsQueryable();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListResolutionForGisGkh(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            //var data = DisposalDomain.GetAll()
            //    .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "")
            //    .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // пока только новые проверки
            //    .Where(x => x.State.FinalState) // в конечном статусе
            //    .Select(x => new
            //    {
            //        x.Id,
            //        x.DocumentNumber,
            //        State = GisGkhExaminationState.notSent
            //    }).Filter(loadParams, Container);


            //var inProc = DisposalDomain.GetAll()
            //    .Where(x => x.GisGkhTransportGuid != null && x.GisGkhTransportGuid != "")
            //    .Where(x => x.State.FinalState) // в конечном статусе
            //    .Select(x => new
            //    {
            //        x.Id,
            //        x.DocumentNumber,
            //        State = GisGkhExaminationState.inProcess
            //    })
            //    .Filter(loadParams, Container).AsEnumerable();

            var data = ResolutionDomain.GetAll()
                .Where(x => x.GisGkhTransportGuid == null || x.GisGkhTransportGuid == "") // отсекаем ожидающие ответа
                .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "") // нет гуида - не отправлена
                .Where(x => x.State.FinalState) // в конечном статусе
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentDate,
                    State = GisGkhExaminationState.notSent
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListROForGisGkhExport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = RealityObjectRepo.GetAll()
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments || x.TypeHouse == TypeHouse.SocialBehavior)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable)
                .Select(x => new
                {
                    x.Id,
                    Address = x.FiasAddress.AddressName,
                    //x.Address,
                    x.NumberGisGkhMatchedApartments,
                    x.NumberGisGkhMatchedNonResidental,
                    x.NumberApartments,
                    x.NumberNonResidentialPremises,
                    MatchedRooms = (!x.NumberApartments.HasValue || (x.NumberApartments == x.NumberGisGkhMatchedApartments)) && (!x.NumberNonResidentialPremises.HasValue || (x.NumberNonResidentialPremises == x.NumberGisGkhMatchedNonResidental)) ? YesNo.Yes : YesNo.No
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListMOForGisGkhExport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = MunicipalityDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Oktmo
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListProgramForGisGkhExport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = ProgramVersionDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.GisGkhGuid
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListContragentForGisGkhExport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = ContragentDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Inn
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListRooms(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("RealityObjectId", 0L);
            var preData = RoomDomain.GetAll()
                .Where(x => x.RealityObject != null && x.RealityObject.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.Area,
                    x.CadastralNumber,
                    x.ChamberNum,
                    x.CommunalArea,
                    x.Description,
                    x.EntranceNum,
                    x.Floor,
                    x.GisGkhPremisesGUID,
                    x.IsCommunal,
                    x.IsRoomCommonPropertyInMcd,
                    x.IsRoomHasNoNumber,
                    x.LivingArea,
                    x.Notation,
                    x.OwnershipType,
                    x.PrevAssignedRegNumber,
                    x.RealityObject,
                    x.RoomNum,
                    x.RoomsCount,
                    x.Type,
                    Matched = x.GisGkhPremisesGUID != null && x.GisGkhPremisesGUID != ""
                }).AsEnumerable();
                
            var data = preData.Select(x => new
            {
                x.Id,
                x.Area,
                x.CadastralNumber,
                x.ChamberNum,
                x.CommunalArea,
                x.Description,
                x.EntranceNum,
                x.Floor,
                x.GisGkhPremisesGUID,
                x.IsCommunal,
                x.IsRoomCommonPropertyInMcd,
                x.IsRoomHasNoNumber,
                x.LivingArea,
                x.Notation,
                x.OwnershipType,
                x.PrevAssignedRegNumber,
                x.RealityObject,
                x.RoomNum,
                x.RoomsCount,
                x.Type,
                x.Matched
            }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListPremises(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("RealityObjectId", 0L);
            var data = GisGkhPremisesDomain.GetAll()
                .Where(x => x.RealityObject != null && x.RealityObject.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.CadastralNumber,
                    x.EntranceNum,
                    x.Floor,
                    x.RealityObject,
                    x.PremisesNum,
                    x.RoomType,
                    x.TotalArea,
                    x.GrossArea,
                    x.PremisesGUID,
                    Matched = RoomDomain.GetAll().Select(y => y.GisGkhPremisesGUID).Contains(x.PremisesGUID)
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListTasksForMassSign(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var FileDomain = Container.ResolveDomain<GisGkhRequestsFile>();

            Dictionary<long, long> reqFilesDict = FileDomain.GetAll().Where(x => x.GisGkhFileType == GisGkhFileType.request)
                .GroupBy(x => x.GisGkhRequests.Id, x => x.FileInfo.Id).ToDictionary(x => x.Key, x => x.FirstOrDefault());
            Dictionary<long, long> respFilesDict = FileDomain.GetAll().Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                .GroupBy(x => x.GisGkhRequests.Id, x => x.FileInfo.Id).ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var data = GisGkhRequestsDomain.GetAll()
                .Where(x => x.RequestState == GisGkhRequestState.Formed)
                .Select(x => new
                {
                    x.Id,
                    x.MessageGUID,
                    //x.RequesterMessageGUID,
                    x.ObjectCreateDate,
                    OperatorName = x.Operator != null ? x.Operator.User.Name : "",
                    //x.ReqDate,
                    //x.RequestState,
                    x.TypeRequest,
                    x.Answer,
                    //x.IsExport
                }).AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.MessageGUID,
                    //x.RequesterMessageGUID,
                    x.ObjectCreateDate,
                    x.OperatorName,
                    //x.ReqDate,
                    //x.RequestState,
                    x.TypeRequest,
                    x.Answer,
                    //x.IsExport,
                    ReqFile = reqFilesDict.ContainsKey(x.Id) ? (long?)reqFilesDict[x.Id] : null,
                    RespFile = respFilesDict.ContainsKey(x.Id) ? (long?)respFilesDict[x.Id] : null
                })
                .AsQueryable()
                   .Filter(loadParams, Container);
            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListProgramImport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = ProgramDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PeriodName = x.Period.Name,
                    x.TypeVisibilityProgramCr,
                    x.TypeProgramStateCr
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListObjectCrImport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var programId = baseParams.Params.GetAs<long>("programId");

            var data = ObjectCrDomain.GetAll()
                .Where(x => x.ProgramCr.Id == programId)
                .Select(x => new
                {
                    x.Id,
                    Address = x.RealityObject.FiasAddress.AddressName
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListBuildContractImport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var objCrId = baseParams.Params.GetAs<long>("objCrId");

            var data = ContractDomain.GetAll()
                .Where(x => x.ObjectCr.Id == objCrId)
                .Where(x => x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    DocumentDateFrom = x.DocumentDateFrom ?? x.ObjectCreateDate,
                    Sum = x.Sum ?? 0
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListBuildContractForActImport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var objCrId = baseParams.Params.GetAs<long>("objCrId");

            var data = ContractDomain.GetAll()
                .Where(x => x.ObjectCr.Id == objCrId)
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    DocumentDateFrom = x.DocumentDateFrom ?? x.ObjectCreateDate,
                    Sum = x.Sum ?? 0
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListPerfWorkActImport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var buildContrId = baseParams.Params.GetAs<long>("buildContrId");

            var works = TypeWorkCrDomain.GetAll()
                .Where(x => x.BuildContract.Id == buildContrId)
                .Select(x => x.TypeWork)
                .ToList();

            var data = WorkActDomain.GetAll()
                .Where(x => works.Contains(x.TypeWorkCr))
                .Where(x => x.State.FinalState)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    DateFrom = x.DateFrom ?? x.ObjectCreateDate,
                    Sum = x.Sum ?? 0
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult ListDownloads(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = GisGkhDownloadsDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.EntityT,
                    x.FileField,
                    x.RecordId,
                    x.Guid
                }).Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public bool MatchRoom(BaseParams baseParams, long roomId, long premisesId)
        {
            var room = RoomDomain.Get(roomId);
            var premises = GisGkhPremisesDomain.Get(premisesId);
            if (room != null && premises != null)
            {
                room.GisGkhPremisesGUID = premises.PremisesGUID;
                RoomDomain.Update(room);
                var RO = RealityObjectRepo.Get(room.RealityObject.Id);
                if (RO != null)
                {
                    if (room.Type == RoomType.Living)
                    {
                        RO.NumberGisGkhMatchedApartments += 1;
                    }
                    else
                    {
                        RO.NumberGisGkhMatchedNonResidental += 1;
                    }
                    RealityObjectRepo.Update(RO);
                }
            }
            return true;
        }

        public bool UnMatchRoom(BaseParams baseParams, long roomId)
        {
            var room = RoomDomain.Get(roomId);
            if (room != null)
            {
                var oldGUID = room.GisGkhPremisesGUID;
                room.GisGkhPremisesGUID = null;
                RoomDomain.Update(room);
                var RO = RealityObjectRepo.Get(room.RealityObject.Id);
                if (RO != null & !string.IsNullOrEmpty(oldGUID))
                {
                    if (room.Type == RoomType.Living)
                    {
                        RO.NumberGisGkhMatchedApartments -= 1;
                    }
                    else
                    {
                        RO.NumberGisGkhMatchedNonResidental -= 1;
                    }
                    RealityObjectRepo.Update(RO);
                }
            }
            return true;
        }

        public bool DownloadFiles(BaseParams baseParams)
        {
            var dwlds = GisGkhDownloadsDomain.GetAll().ToList();
            List<long> makeArchiveAppeals = new List<long>();
            foreach(var dwld in dwlds)
            {
                if ((dwld.orgPpaGuid != null) && (dwld.orgPpaGuid != ""))
                {
                    var downloadResult = _fileService.DownloadFile(dwld.Guid, dwld.orgPpaGuid);

                    if (downloadResult.Success)
                    {
                        bool delete = false;
                        switch (dwld.EntityT)
                        {
                            case "AttachmentField":
                                try
                                {
                                    var attachmentField = AttachmentFieldDomain.Get(dwld.RecordId);
                                    if (dwld.FileField == "Attachment")
                                    {
                                        attachmentField.Attachment = downloadResult.File;
                                        AttachmentFieldDomain.Update(attachmentField);
                                        delete = true;
                                    }
                                }
                                catch { }
                                break;
                            case "AppealCitsAttachment":
                                AppealCitsAttachment appealCitsAttachment = AppealCitsAttachmentDomain.Get(dwld.RecordId);
                                if (appealCitsAttachment == null)
                                {
                                    delete = true;
                                    break;
                                }
                                try
                                {
                                    if (dwld.FileField == "FileInfo")
                                    {
                                        appealCitsAttachment.FileInfo = downloadResult.File;
                                        AppealCitsAttachmentDomain.Update(appealCitsAttachment);
                                        delete = true;
                                        if (!makeArchiveAppeals.Contains(appealCitsAttachment.AppealCits.Id))
                                        {
                                            makeArchiveAppeals.Add(appealCitsAttachment.AppealCits.Id);
                                        }
                                    }
                                }
                                catch { }
                                break;
                            case "CitizenSuggestionFiles":
                                CitizenSuggestionFiles citizenSuggestionFile = CitizenSuggestionFilesDomain.Get(dwld.RecordId);
                                if (citizenSuggestionFile == null)
                                {
                                    delete = true;
                                    break;
                                }
                                try
                                {
                                    if (dwld.FileField == "FileInfo")
                                    {
                                        citizenSuggestionFile.DocumentFile = downloadResult.File;
                                        CitizenSuggestionFilesDomain.Update(citizenSuggestionFile);
                                        delete = true;
                                    }
                                }
                                catch { }
                                break;
                        }
                        if (delete)
                        {
                            GisGkhDownloadsDomain.Delete(dwld.Id);
                        }
                    }
                    else
                    {
                        // Не скачалось
                    }
                }
            }
            if (makeArchiveAppeals.Count > 0)
            {
                try
                {
                    makeArchiveAppeals.ForEach(id =>
                    {
                        Bars.B4.Modules.FileStorage.FileInfo archiveFile = GetArchive(id);
                        if (archiveFile != null)
                        {
                            var appeal = AppealCitsRepo.Get(id);
                            if (appeal != null)
                            {
                                appeal.File = archiveFile;
                                AppealCitsRepo.Update(appeal);
                            }
                        }


                    });
                }
                catch
                { }
            }
            return true;
        }
        #endregion

        #region Private methods

        private Bars.B4.Modules.FileStorage.FileInfo GetArchive(long id)
        {
            
            var fileManager = this.Container.Resolve<IFileManager>();
            var archive = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level9,
                AlternateEncoding = Encoding.GetEncoding("cp866"),
                AlternateEncodingUsage = ZipOption.AsNecessary
            };
            var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
            bool skip = true;
            AppealCitsAttachmentDomain.GetAll()
                .Where(x => x.AppealCits.Id == id && x.FileInfo != null).ToList().ForEach(x =>
                 {
                     System.IO.File.WriteAllBytes(
                     Path.Combine(tempDir.FullName, $"{x.FileInfo.Name}.{x.FileInfo.Extention}"),
                     fileManager.GetFile(x.FileInfo).ReadAllBytes());
                     skip = false;
                 });
            archive.AddDirectory(tempDir.FullName);
            if (!skip)
            {
                var appealCits = AppealCitsRepo.Get(id);
                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);
                    string from = appealCits.DateFrom.HasValue ? appealCits.DateFrom.Value.ToShortDateString() : "";
                    var file = fileManager.SaveFile(ms, $"Обращение {appealCits.DocumentNumber} от {from}.zip");
                    return file;
                }
            }
           
            return null;
        }

            /// <summary>
            /// Сериаилазация запроса
            /// </summary>
            /// <param name="data">Запрос</param>
            /// <returns>Xml-документ</returns>
        protected XmlDocument SerializeRequest(object data)
            {
            var type = data.GetType();
            XmlDocument result;

            var attr = (XmlTypeAttribute)type.GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(type, attr.Namespace);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, data);

                    result = new XmlDocument();
                    result.LoadXml(stringWriter.ToString());
                }
            }

            //var prefixer = new XmlNsPrefixer();
            //prefixer.Process(result);

            return result;
        }

        protected TDataType DeserializeData<TDataType>(string data)
        {
            TDataType result;

            var attr = (XmlTypeAttribute)typeof(TDataType).GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(typeof(TDataType), attr.Namespace);

            using (var reader = XmlReader.Create(new StringReader(data)))
            {
                result = (TDataType)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        private Bars.B4.Modules.FileStorage.FileInfo SaveFile(byte[] data, string fileName)
        {
            if (data == null)
                return null;
            try
            {
                //сохраняем пакет
                return _fileManager.SaveFile(fileName, data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void SaveFile(GisGkhRequests req, GisGkhFileType fileType, XmlDocument data, string fileName)
        {
            if (data == null)
                throw new Exception("Пустой документ для сохранения");

            MemoryStream stream = new MemoryStream();
            data.PreserveWhitespace = true;
            data.Save(stream);
            try
            {
                //сохраняем
                GisGkhRequestsFileDomain.Save(new GisGkhRequestsFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    GisGkhRequests = req,
                    GisGkhFileType = fileType,
                    FileInfo = _fileManager.SaveFile(stream, fileName)
                });
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при сохранении файла: " + e.Message);
            }
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
