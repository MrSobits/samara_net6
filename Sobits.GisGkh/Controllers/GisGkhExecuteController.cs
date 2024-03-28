namespace Sobits.GisGkh.Controllers
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using GisGkhLibrary.NsiServiceAsync;
    using Sobits.GisGkh.DomainService;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Enums;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    using Microsoft.AspNetCore.Mvc;

    public class GisGkhExecuteController : BaseController
    {
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        private IFileManager _fileManager;
        public IGkhUserManager UserManager { get; set; }
        private IGISGKHService _GISGKHService;
        private IExportOrgRegistryService _ExportOrgRegistryService;
        private IExportHouseDataService _ExportHouseDataService;
        private IExportNsiListService _ExportNsiListService;
        private IExportNsiItemsService _ExportNsiItemsService;
        private IExportNsiPagingItemsService _ExportNsiPagingItemsService;
        private IExportDataProviderNsiItemsService _ExportDataProviderNsiItemsService;
        private IExportAccountDataService _ExportAccountDataService;
        private IExportRegionalProgramService _ExportRegionalProgramService;
        private IExportRegionalProgramWorkService _ExportRegionalProgramWorkService;
        private IExportPlanService _ExportPlanService;
        private IExportPlanWorkService _ExportPlanWorkService;
        private IExportBriefApartmentHouseService _ExportBriefApartmentHouseService;
        private IImportAccountDataService _ImportAccountDataService;
        private IExportDecisionsFormingFundService _ExportDecisionsFormingFundService;
        private IImportDecisionsFormingFundService _ImportDecisionsFormingFundService;
        private IImportPlanService _ImportPlanService;
        private IImportPlanWorkService _ImportPlanWorkService;
        private IImportRegionalProgramService _ImportRegionalProgramService;
        private IImportRegionalProgramWorkService _ImportRegionalProgramWorkService;
        private IExportPaymentDocumentDataService _ExportPaymentDocumentDataService;
        private IImportPaymentDocumentDataService _ImportPaymentDocumentDataService;
        private IExportAppealService _ExportAppealService;
        private IImportAnswerService _ImportAnswerService;
        private IExportAppealCRService _ExportAppealCRService;
        private IExportDRsService _ExportDRsService;
        private IImportAnswerCRService _ImportAnswerCRService;
        private IExportExaminationsService _ExportExaminationsService;
        private IImportExaminationsService _ImportExaminationsService;
        private IExportInspectionPlansService _ExportInspectionPlansService;
        private IImportInspectionPlanService _ImportInspectionPlanService;
        private IExportDecreesAndDocumentsDataService _ExportDecreesAndDocumentsDataService;
        private IImportDecreesAndDocumentsDataService _ImportDecreesAndDocumentsDataService;
        private IExportLicenseService _ExportLicenseService;
        private IImportBuildContractService _ImportBuildContractService;
        private IImportPerfWorkActService _ImportPerfWorkActService;

        public GisGkhExecuteController(IFileManager fileManager, IGISGKHService GISGKHService, IExportOrgRegistryService ExportOrgRegistryService,
            IExportHouseDataService ExportHouseDataService, IExportNsiListService ExportNsiListService, IExportNsiItemsService ExportNsiItemsService,
            IExportNsiPagingItemsService ExportNsiPagingItemsService, IExportDataProviderNsiItemsService ExportDataProviderNsiItemsService,
            IExportAccountDataService ExportAccountDataService, IExportRegionalProgramService ExportRegionalProgramService,
            IExportRegionalProgramWorkService ExportRegionalProgramWorkService, IExportPlanService ExportPlanService, IImportPlanWorkService ImportPlanWorkService,
            IExportBriefApartmentHouseService ExportBriefApartmentHouseService, IImportAccountDataService ImportAccountDataService, IExportDecisionsFormingFundService ExportDecisionsFormingFundService,
            IImportDecisionsFormingFundService ImportDecisionsFormingFundService, IImportPlanService ImportPlanService, IExportPlanWorkService ExportPlanWorkService,
            IExportPaymentDocumentDataService ExportPaymentDocumentDataService, IImportPaymentDocumentDataService ImportPaymentDocumentDataService,
            IImportRegionalProgramService ImportRegionalProgramService, IImportRegionalProgramWorkService ImportRegionalProgramWorkService,
            IExportAppealService ExportAppealService, IImportAnswerService ImportAnswerService, IExportAppealCRService ExportAppealCRService, IImportAnswerCRService ImportAnswerCRService,
            IExportExaminationsService ExportExaminationsService, IImportExaminationsService ImportExaminationsService,
            IExportInspectionPlansService ExportInspectionPlansService, IImportInspectionPlanService ImportInspectionPlanService,
            IExportDecreesAndDocumentsDataService ExportDecreesAndDocumentsDataService, IImportDecreesAndDocumentsDataService ImportDecreesAndDocumentsDataService,
            IExportLicenseService ExportLicenseService, IExportDRsService expDrs, IImportBuildContractService ImportBuildContractService, IImportPerfWorkActService ImportPerfWorkActService
            )
        {
            _fileManager = fileManager;
            _GISGKHService = GISGKHService;
            _ExportOrgRegistryService = ExportOrgRegistryService;
            _ExportHouseDataService = ExportHouseDataService;
            _ExportNsiListService = ExportNsiListService;
            _ExportNsiItemsService = ExportNsiItemsService;
            _ExportNsiPagingItemsService = ExportNsiPagingItemsService;
            _ExportDataProviderNsiItemsService = ExportDataProviderNsiItemsService;
            _ExportAccountDataService = ExportAccountDataService;
            _ExportRegionalProgramService = ExportRegionalProgramService;
            _ExportRegionalProgramWorkService = ExportRegionalProgramWorkService;
            _ExportPlanService = ExportPlanService;
            _ExportPlanWorkService = ExportPlanWorkService;
            _ExportBriefApartmentHouseService = ExportBriefApartmentHouseService;
            _ImportAccountDataService = ImportAccountDataService;
            _ExportDecisionsFormingFundService = ExportDecisionsFormingFundService;
            _ImportDecisionsFormingFundService = ImportDecisionsFormingFundService;
            _ImportPlanService = ImportPlanService;
            _ImportPlanWorkService = ImportPlanWorkService;
            _ExportPaymentDocumentDataService = ExportPaymentDocumentDataService;
            _ImportPaymentDocumentDataService = ImportPaymentDocumentDataService;
            _ExportAppealService = ExportAppealService;
            _ImportAnswerService = ImportAnswerService;
            _ExportAppealCRService = ExportAppealCRService;
            _ImportAnswerCRService = ImportAnswerCRService;
            _ImportRegionalProgramService = ImportRegionalProgramService;
            _ImportRegionalProgramWorkService = ImportRegionalProgramWorkService;
            _ExportExaminationsService = ExportExaminationsService;
            _ImportExaminationsService = ImportExaminationsService;
            _ExportInspectionPlansService = ExportInspectionPlansService;
            _ImportInspectionPlanService = ImportInspectionPlanService;
            _ExportDecreesAndDocumentsDataService = ExportDecreesAndDocumentsDataService;
            _ImportDecreesAndDocumentsDataService = ImportDecreesAndDocumentsDataService;
            _ExportLicenseService = ExportLicenseService;
            _ExportDRsService = expDrs;
            _ImportBuildContractService = ImportBuildContractService;
            _ImportPerfWorkActService = ImportPerfWorkActService;
        }

        /// <summary>
        /// Сохранить запрос
        /// </summary>
        public ActionResult SaveRequest(BaseParams baseParams, long? reqId, GisGkhTypeRequest reqType, string[] reqParams)
        {
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.GisGkhContragent == null)
            {
                return JsFailure("К учётной записи текущего пользователя не привязана организация для работы с ГИС ЖКХ");
            }
            GisGkhRequests req = null;
            if (reqId != null)
            {
                req = GisGkhRequestsDomain.GetAll()
                .Where(x => x.Id == reqId).FirstOrDefault();
            }
            if (req == null)
            {
                req = new GisGkhRequests();
                req.TypeRequest = reqType;
                //req.ReqDate = DateTime.Now;
                req.RequestState = GisGkhRequestState.NotFormed;

                GisGkhRequestsDomain.Save(req);

                switch (req.TypeRequest)
                {
                    case GisGkhTypeRequest.exportRegionalProgram:
                        try
                        {
                            _ExportRegionalProgramService.SaveRequest(req, reqParams[0]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы региональной программы и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос региональной программы сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос региональной программы сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportRegionalProgramWork:
                        try
                        {
                            _ExportRegionalProgramWorkService.SaveRequest(req, reqParams[0]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы работ региональной программы и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос работ региональной программы сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос работ региональной программы сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportPlan:
                        try
                        {
                            _ExportPlanService.SaveRequest(req, reqParams);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы КПР и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос КПР сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос КПР сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportPlanWork:
                        try
                        {
                            _ExportPlanWorkService.SaveRequest(req, reqParams);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы работ КПР и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос работ КПР сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос работ КПР сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportBriefApartmentHouse:
                        try
                        {
                            _ExportBriefApartmentHouseService.SaveRequest(req, reqParams[0]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы краткой информации о МКД и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос краткой информации о МКД сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос краткой информации о МКД сформирован, отправить запрос может только уполномоченный сотрудник");
                        }
                    case GisGkhTypeRequest.exportHouseData:
                        try
                        {
                            _ExportHouseDataService.SaveRequest(req, reqParams[0]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации о доме и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос информации о доме сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос информации о доме сформирован, отправить запрос может только уполномоченный сотрудник");
                        }
                    case GisGkhTypeRequest.exportAccountData:

                        try
                        {
                            _ExportAccountDataService.SaveRequest(req, reqParams[0]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации о счетах по дому и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос информации о счетах по дому сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос информации о счетах по дому сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importAccountData:

                        try
                        {
                            _ImportAccountDataService.SaveRequest(req, reqParams[0]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы импорта ЛС и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос(ы) импорта ЛС сформирован(ы), ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос(ы) сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос(ы) импорта ЛС сформирован(ы), отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportDecisionsFormingFund:

                        try
                        {
                            _ExportDecisionsFormingFundService.SaveRequest(req, new List<long> { long.Parse(reqParams[0]) });
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы экспорта решений о выборе способа формирования ФКР и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос экспорта решений о выборе способа формирования ФКР сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос экспорта решений о выборе способа формирования ФКР сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importDecisionsFormingFund:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации об организации сформирован, запрос импорта решений о выборе способа формирования ФКР будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportDecisionsFormingFundService.SaveRequest(req, reqParams[0], thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importPlan:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации об организации сформирован, запрос импорта КПР будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportPlanService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importPlanWork:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации об организации сформирован, запрос импорта работ КПР будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportPlanWorkService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importBuildContract:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации об организации сформирован, запрос импорта договоров КПР будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportBuildContractService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importPerfWorkAct:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации об организации сформирован, запрос импорта актов КПР будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportPerfWorkActService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importRegionalProgram:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запрос информации об организации сформирован, запрос импорта регпрограммы будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportRegionalProgramService.SaveRequest(req, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importRegionalProgramWork:

                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации об организации сформирован, запрос импорта работ регпрограммы будет доступен после получения информации об организации");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    _ImportRegionalProgramWorkService.SaveRequest(req, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                    return JsSuccess("Запрос сформирован");
                                }
                                catch (Exception e)
                                {
                                    req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    return JsFailure("Ошибка: " + e.Message);
                                }
                            }
                        }
                        else
                        {
                            return JsSuccess("Отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportPaymentDocumentData:

                        try
                        {
                            _ExportPaymentDocumentDataService.SaveRequest(req, reqParams[0], reqParams[1]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы информации о начислениях по дому и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос информации о начислениях по дому сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос информации о начислениях по дому сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importPaymentDocumentData:

                        try
                        {
                            _ImportPaymentDocumentDataService.SaveRequest(req, reqParams[0], reqParams[1], reqParams[2]);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы выгрузки начислений и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос выгрузки начислений сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос выгрузки начислений сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportExaminations:

                        try
                        {
                            _ExportExaminationsService.SaveRequest(req, reqParams);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы экспорта проверок и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос экспорта проверок сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос экспорта проверок сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importExaminations:
                        if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                        {
                            try
                            {
                                _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                return JsSuccess("Запрос информации об организации сформирован, запрос импорта проверок будет доступен после получения информации об организации");
                            }
                            catch (Exception e)
                            {
                                //req.RequestState = GisGkhRequestState.Error;
                                //GisGkhRequestsDomain.Update(req);
                                //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                _ImportExaminationsService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                return JsSuccess("Запрос сформирован");
                            }
                            catch (Exception e)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                //GisGkhRequestsDomain.Update(req);
                                return JsFailure("Ошибка: " + e.Message);
                            }
                        }

                    case GisGkhTypeRequest.importCurrentExaminations:
                        if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                        {
                            try
                            {
                                _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                return JsSuccess("Запрос информации об организации сформирован, запрос импорта проверки будет доступен после получения информации об организации");
                            }
                            catch (Exception e)
                            {
                                //req.RequestState = GisGkhRequestState.Error;
                                //GisGkhRequestsDomain.Update(req);
                                //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                _ImportExaminationsService.SaveCurrentRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                return JsSuccess("Запрос сформирован");
                            }
                            catch (Exception e)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                //GisGkhRequestsDomain.Update(req);
                                return JsFailure("Ошибка: " + e.Message);
                            }
                        }

                    case GisGkhTypeRequest.exportInspectionPlans:

                        try
                        {
                            _ExportInspectionPlansService.SaveRequest(req, reqParams);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы экспорта плана проверок и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос экспорта плана проверок сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос экспорта плана проверок сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importInspectionPlan:

                        try
                        {
                            _ImportInspectionPlanService.SaveRequest(req, reqParams);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы импорта плана проверок и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос импорта плана проверок сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос импорта плана проверок сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.importDecreesAndDocumentsData:
                        if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                        {
                            try
                            {
                                _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                return JsSuccess("Запрос информации об организации сформирован, запрос импорта постановлений будет доступен после получения информации об организации");
                            }
                            catch (Exception e)
                            {
                                //req.RequestState = GisGkhRequestState.Error;
                                //GisGkhRequestsDomain.Update(req);
                                //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                return JsSuccess("Ошибка при запросе информации об организации: " + e.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                _ImportDecreesAndDocumentsDataService.SaveCurrentRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                                return JsSuccess("Запрос сформирован");
                            }
                            catch (Exception e)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                //GisGkhRequestsDomain.Update(req);
                                return JsFailure("Ошибка: " + e.Message);
                            }
                        }

                    case GisGkhTypeRequest.exportDecreesAndDocumentsData:

                        try
                        {
                            _ExportDecreesAndDocumentsDataService.SaveRequest(req, reqParams);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы экспорта постановлений и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос экспорта постановлений сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос экспорта постановлений сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportLicense:

                        try
                        {
                            _ExportLicenseService.SaveRequest(req, reqParams.Select(long.Parse).ToList());
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            //GisGkhRequestsDomain.Update(req);
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы экспорта лицензий и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    //req.RequestState = GisGkhRequestState.Error;
                                    //GisGkhRequestsDomain.Update(req);
                                    //return JsFailure("Ошибка при запросе информации об организациях: " + e.Message);
                                    return JsSuccess("Запрос экспорта лицензий сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован(ы)");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос экспорта лицензий сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportOrgRegistry:
                        try
                        {
                            _ExportOrgRegistryService.SaveRequest(req, new List<long> { long.Parse(reqParams[0]) });
                            return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            return JsFailure("Ошибка: " + e.Message);
                        }

                    case GisGkhTypeRequest.exportNsiList:
                        try
                        {
                            _ExportNsiListService.SaveRequest(req, GisGkhLibrary.NsiCommonAsync.ListGroup.NSI);
                            return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }

                    case GisGkhTypeRequest.exportNsiRaoList:
                        try
                        {
                            _ExportNsiListService.SaveRequest(req, GisGkhLibrary.NsiCommonAsync.ListGroup.NSIRAO);
                            return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }

                    case GisGkhTypeRequest.exportNsiItems:
                        try
                        {
                            _ExportNsiItemsService.SaveRequest(req, (GisGkhLibrary.NsiCommonAsync.ListGroup)Enum.GetValues(typeof(GisGkhLibrary.NsiCommonAsync.ListGroup)).GetValue(int.Parse(reqParams[0])), reqParams[1]);
                            return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }

                    case GisGkhTypeRequest.exportNsiPagingItems:
                        try
                        {
                            _ExportNsiPagingItemsService.SaveRequest(req, (GisGkhLibrary.NsiCommonAsync.ListGroup)Enum.GetValues(typeof(GisGkhLibrary.NsiCommonAsync.ListGroup)).GetValue(int.Parse(reqParams[0])), reqParams[1], int.Parse(reqParams[2]));
                            return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }

                    case GisGkhTypeRequest.exportDataProviderNsiItem:
                        try
                        {
                            _ExportDataProviderNsiItemsService.SaveRequest(req, (exportDataProviderNsiItemRequestRegistryNumber)Enum.GetValues(typeof(exportDataProviderNsiItemRequestRegistryNumber)).GetValue(int.Parse(reqParams[0])));
                            //return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы пунктов справочника поставщика и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    return JsSuccess("Запрос пунктов справочника поставщика сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос пунктов справочника поставщика сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportAppeal:
                        try
                        {
                            _ExportAppealService.SaveRequest(req, reqParams[0].ToDateTimeNullable(), reqParams[1].ToDateTimeNullable());
                            //return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы обращений и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    return JsSuccess("Запрос обращений сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос обращений сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportAppealCR:
                        try
                        {
                            _ExportAppealCRService.SaveRequest(req, reqParams[0].ToDateTimeNullable(), reqParams[1].ToDateTimeNullable());
                            //return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы обращений и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    return JsSuccess("Запрос обращений ФКР сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос обращений ФКР сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    case GisGkhTypeRequest.exportDebtRequests:
                        try
                        {
                            _ExportDRsService.SaveRequest(req, reqParams[0].ToDateTimeNullable(), reqParams[1].ToDateTimeNullable());
                            //return JsSuccess("Запрос сформирован");
                        }
                        catch (Exception e)
                        {
                            return JsFailure("Ошибка: " + e.Message);
                        }
                        if (thisOperator.GisGkhContragent != null)
                        {
                            if ((thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == null) || (thisOperator.GisGkhContragent.GisGkhOrgPPAGUID == ""))
                            {
                                try
                                {
                                    _ExportOrgRegistryService.SaveRequest(null, new List<long> { thisOperator.GisGkhContragent.Id });
                                    return JsSuccess("Запросы обращений и информации об организации сформированы");
                                }
                                catch (Exception e)
                                {
                                    return JsSuccess("Запрос обращений ФКР сформирован, ошибка при запросе информации об организации: " + e.Message);
                                }
                            }
                            else
                            {
                                return JsSuccess("Запрос сформирован");
                            }
                        }
                        else
                        {
                            return JsSuccess("Запрос обращений ФКР сформирован, отправить запрос может только уполномоченный сотрудник");
                        }

                    default:
                        return JsFailure("Неизвестный тип запроса");
                }
            }
            else return JsFailure("Уже есть");
        }

        ///// <summary>
        ///// Получить словари
        ///// </summary>
        //public ActionResult GetDictionaries(BaseParams baseParams)
        //{
        //    GisGkhRequests req = new GisGkhRequests();
        //    req.TypeRequest = GisGkhTypeRequest.exportNsiList;
        //    //req.ReqDate = DateTime.Now;
        //    req.RequestState = GisGkhRequestState.NotFormed;

        //    GisGkhRequestsDomain.Save(req);

        //    try
        //    {
        //        _ExportNsiListService.SaveRequest(req, ListGroup.NSI);
        //        return JsSuccess("Запрос сформирован");
        //    }
        //    catch (Exception e)
        //    {
        //        return JsFailure("Ошибка: " + e.Message);
        //    }
        //}

        /// <summary>
        /// Получить XML
        /// </summary>
        public ActionResult GetXML(BaseParams baseParams, long reqId)
        {
            try
            {
                string xml = _GISGKHService.GetXML(reqId);
                return new JsonGetResult(new BaseDataResult(xml).Data);
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.Message);
            }
        }

        /// <summary>
        /// Сохранить и отправить подписанный XML
        /// </summary>
        public ActionResult SaveAndSendRequest(BaseParams baseParams, long reqId, string signedData)
        {
            try
            {
                _GISGKHService.SaveSignedXML(reqId, signedData);
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.ToString() + e.StackTrace);
            }
            return SendRequest(baseParams, reqId);
        }

        /// <summary>
        /// Отправить запрос
        /// </summary>
        public ActionResult SendRequest(BaseParams baseParams, long reqId)
        {
            try
            {
                _GISGKHService.SendRequest(reqId);
                return JsSuccess("Запрос отправлен");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.Message);
            }
        }

        public ActionResult CheckAnswers(BaseParams baseParams)
        {
            var orgPPAGUID = "";
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator.GisGkhContragent != null)
            {
                orgPPAGUID = thisOperator.GisGkhContragent.GisGkhOrgPPAGUID ?? "";
            }
            // берём только запросы, отправленные ГИС ЖКХ контрагентом текущего оператора, если у него есть orgPPAGUID
            var requests = GisGkhRequestsDomain.GetAll()
                .WhereIf(orgPPAGUID != "", x => x.RequestState == GisGkhRequestState.Queued && x.Operator.GisGkhContragent.GisGkhOrgPPAGUID == orgPPAGUID);
            foreach (var req in requests)
            {
                try
                {
                    switch (req.TypeRequest)
                    {
                        case GisGkhTypeRequest.exportNsiItems:
                            _ExportNsiItemsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportNsiPagingItems:
                            _ExportNsiPagingItemsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportDataProviderNsiItem:
                            _ExportDataProviderNsiItemsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportNsiList:
                            _ExportNsiListService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportNsiRaoList:
                            _ExportNsiListService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportRegionalProgram:
                            _ExportRegionalProgramService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportRegionalProgramWork:
                            _ExportRegionalProgramWorkService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportPlan:
                            _ExportPlanService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportPlanWork:
                            _ExportPlanWorkService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportBriefApartmentHouse:
                            _ExportBriefApartmentHouseService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportHouseData:
                            _ExportHouseDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportOrgRegistry:
                            _ExportOrgRegistryService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportAccountData:
                            _ExportAccountDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importAccountData:
                            _ImportAccountDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportDecisionsFormingFund:
                            _ExportDecisionsFormingFundService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importDecisionsFormingFund:
                            _ImportDecisionsFormingFundService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importPlan:
                            _ImportPlanService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importPlanWork:
                            _ImportPlanWorkService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importRegionalProgram:
                            _ImportRegionalProgramService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importRegionalProgramWork:
                            _ImportRegionalProgramWorkService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportPaymentDocumentData:
                            _ExportPaymentDocumentDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importPaymentDocumentData:
                            _ImportPaymentDocumentDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportExaminations:
                            _ExportExaminationsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importExaminations:
                        case GisGkhTypeRequest.importCurrentExaminations:
                            _ImportExaminationsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportInspectionPlans:
                            _ExportInspectionPlansService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importInspectionPlan:
                            _ImportInspectionPlanService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportDecreesAndDocumentsData:
                            _ExportDecreesAndDocumentsDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importDecreesAndDocumentsData:
                            _ImportDecreesAndDocumentsDataService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportAppeal:
                            _ExportAppealService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importAnswer:
                            _ImportAnswerService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportAppealCR:
                            _ExportAppealCRService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportDebtRequests:
                            _ExportDRsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importDebtRequests:
                            _ExportDRsService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importAnswerCR:
                            _ImportAnswerCRService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.exportLicense:
                            _ExportLicenseService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importBuildContract:
                            _ImportBuildContractService.CheckAnswer(req, orgPPAGUID);
                            break;
                        case GisGkhTypeRequest.importPerfWorkAct:
                            _ImportPerfWorkActService.CheckAnswer(req, orgPPAGUID);
                            break;
                    }
                }
                catch (Exception e)
                {
                    req.Answer += e.Message;
                    req.RequestState = GisGkhRequestState.Error;
                    GisGkhRequestsDomain.Update(req);
                }
            }
            return JsSuccess("Ответы проверены, подробнее в статусах запросов");
        }

        public ActionResult DisposalAndDecisionForGisGkh(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListDisposalAndDecisionForGisGkh(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ResolutionForGisGkh(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListResolutionForGisGkh(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ROForGisGkhExport(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListROForGisGkhExport(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult MOForGisGkhExport(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListMOForGisGkhExport(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ListRooms(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListRooms(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ListPremises(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListPremises(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ContragentForGisGkhExport(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListContragentForGisGkhExport(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ProgramForGisGkhExport(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListProgramForGisGkhExport(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ListTasksForMassSign(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListTasksForMassSign(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ListDownloads(BaseParams baseParams)
        {
            //var gisGkhService = Container.Resolve<IGISGKHService>();
            try
            {
                return _GISGKHService.ListDownloads(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult ProgramImport(BaseParams baseParams)
        {
            return _GISGKHService.ListProgramImport(baseParams).ToJsonResult();
        }

        public ActionResult ObjectCrImport(BaseParams baseParams)
        {
            return _GISGKHService.ListObjectCrImport(baseParams).ToJsonResult();
        }

        public ActionResult BuildContractImport(BaseParams baseParams)
        {
            return _GISGKHService.ListBuildContractImport(baseParams).ToJsonResult();
        }

        public ActionResult BuildContractForActImport(BaseParams baseParams)
        {
            return _GISGKHService.ListBuildContractForActImport(baseParams).ToJsonResult();
        }

        public ActionResult PerfWorkActImport(BaseParams baseParams)
        {
            return _GISGKHService.ListPerfWorkActImport(baseParams).ToJsonResult();
        }

        public ActionResult MatchRoom(BaseParams baseParams, long roomId, long premisesId)
        {
            ////var gisGkhService = Container.Resolve<IGISGKHService>();
            //try
            //{
            //    return _GISGKHService.MatchRoom(baseParams, roomId, premisesId).ToJsonResult();
            //}
            //finally
            //{
            //    //  Container.Release(service);
            //}
            try
            {
                if (_GISGKHService.MatchRoom(baseParams, roomId, premisesId) == true)
                {
                    return JsSuccess("Помещение сопоставлено");
                }
                else return JsFailure("Ошибка сопоставления");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.Message);
            }
        }

        public ActionResult UnMatchRoom(BaseParams baseParams, long roomId)
        {
            ////var gisGkhService = Container.Resolve<IGISGKHService>();
            //try
            //{
            //    return _GISGKHService.MatchRoom(baseParams, roomId, premisesId).ToJsonResult();
            //}
            //finally
            //{
            //    //  Container.Release(service);
            //}
            try
            {
                if (_GISGKHService.UnMatchRoom(baseParams, roomId) == true)
                {
                    return JsSuccess("Сопоставление отменено");
                }
                else return JsFailure("Ошибка отмены сопоставления");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.Message);
            }
        }

        public ActionResult DownloadFiles(BaseParams baseParams)
        {
            try
            {
                if (_GISGKHService.DownloadFiles(baseParams) == true)
                {
                    return JsSuccess("Файлы загружены");
                }
                else return JsFailure("Ошибка при загрузке файлов");
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка: " + e.Message);
            }
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
            catch (Exception eeeeeeee)
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

        //private void exportThisOpertorOrganisation(Operator thisOperator)
        //{
        //    var subreq = new GisGkhRequests();
        //    subreq.TypeRequest = GisGkhTypeRequest.exportOrgRegistry;
        //    //subreq.ReqDate = DateTime.Now;
        //    subreq.RequestState = GisGkhRequestState.NotFormed;
        //    GisGkhRequestsDomain.Save(subreq);
        //    try
        //    {
        //        var RegOrgReq = RegOrgCommonAsync.exportOrgRegistryReq(null, new Dictionary<GisGkhLibrary.RegOrgCommonAsync.ItemsChoiceType3, string>{
        //                                    { GisGkhLibrary.RegOrgCommonAsync.ItemsChoiceType3.OGRN, thisOperator.GisGkhContragent.Ogrn },
        //                                    { GisGkhLibrary.RegOrgCommonAsync.ItemsChoiceType3.KPP, thisOperator.GisGkhContragent.Kpp }
        //                                });
        //        var subPrefixer = new XmlNsPrefixer();
        //        XmlDocument subDocument = SerializeRequest(RegOrgReq);
        //        subPrefixer.Process(subDocument);// Удаляем старые файлы ответов, если они, вдруг, имеются
        //        SaveFile(subreq, GisGkhFileType.request, subDocument, "request.xml");
        //        //subreq.ReqFileInfo = SaveFile(subDocument, "Request.dat");
        //        subreq.RequestState = GisGkhRequestState.Formed;
        //        GisGkhRequestsDomain.Update(subreq);
        //        //return JsSuccess("Сформирован запрос идентификатора организации. После получения ответа повторите запрос в ГИС ЖКХ");
        //    }
        //    catch (Exception e)
        //    {
        //        subreq.RequestState = GisGkhRequestState.Error;
        //        GisGkhRequestsDomain.Update(subreq);
        //        throw e;
        //    }
        //}
    }
}