using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.DomainService;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVEmergencyHouse;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVOwnershipProperty;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVPremises;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVRedevelopment;
using Bars.GkhGji.Regions.Habarovsk.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Habarovsk.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Задача на опрос и обработку ответов из смэв
    /// </summary>
    public class GetRPGUAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GISERP> GISERPDomain { get; set; }

        public IDomainService<SMEVEmergencyHouse> SMEVEmergencyHouseDomain { get; set; }
        public IDomainService<SMEVChangePremisesState> SMEVChangePremisesStateDomain { get; set; }
        public IDomainService<SMEVSocialHire> SMEVSocialHireDomain { get; set; }
        public IDomainService<SMEVExploitResolution> SMEVExploitResolutionDomain { get; set; }
        public IDomainService<SMEVRedevelopment> SMEVRedevelopmentDomain { get; set; }
        public IDomainService<SMEVOwnershipProperty> SMEVOwnershipPropertyDomain { get; set; }

        public IDomainService<LogOperation> LogOperationDomainService { get; set; }
        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }

        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }

        public IDomainService<SMEVDISKVLIC> SMEVDISKVLICDomain { get; set; }
        public IDomainService<SMEVFNSLicRequest> SMEVFNSLicRequestDomain { get; set; }
        public IDomainService<SMEVPremises> SMEVPremisesDomain { get; set; }
        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }
        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }
        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }
        public IDomainService<GisGmp> GisGmpDomain { get; set; }
        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }

        private ISMEV3Service _SMEV3Service;
        private ISMEVSocialHireService _SMEVSocialHireService;
        private ISMEVRedevelopmentService _SMEVRedevelopmentService;
        private ISMEVOwnershipPropertyService _SMEVOwnershipPropertyService;
        private ISMEVEmergencyHouseService _SMEVEmergencyHouseService;
        private ISMEVExploitResolutionService _SMEVExploitResolutionService;
        private ISMEVChangePremisesStateService _SMEVChangePremisesStateService;
        private IRPGUService _RPGUService;
        private IFileManager _fileManager;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetRPGUAnswersTaskExecutor(ISMEV3Service SMEV3Service, ISMEVExploitResolutionService SMEVExploitResolutionService, IFileManager fileManager, 
            IRPGUService rpgu, ISMEVChangePremisesStateService prem, ISMEVSocialHireService hire, ISMEVEmergencyHouseService ehouse,
            ISMEVRedevelopmentService redev, ISMEVOwnershipPropertyService owserv)
        {            
            _SMEV3Service = SMEV3Service;
            _SMEVExploitResolutionService = SMEVExploitResolutionService;
             _fileManager = fileManager;
            _RPGUService = rpgu;
            _SMEVChangePremisesStateService = prem;
            _SMEVSocialHireService = hire;
            _SMEVEmergencyHouseService = ehouse;
            _SMEVRedevelopmentService = redev;
            _SMEVOwnershipPropertyService = owserv;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var processLog = new List<string>();
            GetRequestRequest requestResult;
            SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult;
            uint number = 0;
            int cnt = 0;
            try
            {
                //ищем запросы к нам
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    cnt++;
                    if (cnt > 20)
                    {
                        break;
                    }
                    requestResult = _SMEV3Service.GetRequestAsyncSGIO("urn:GetRequest", null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (requestResult.FaultXML != null)
                        break;

                    //если результатов пока нет, возврат
                    if (!requestResult.isAnswerPresent)
                        break;

                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryGetLicRequestRequestProcessed(requestResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {requestResult.OriginalMessageId} - обработано как запрос на получение лицензии - {processedResult}");
                      //  break;

                    }
                    else
                    {
                        processLog.Add($"Сообщение {requestResult.OriginalMessageId} - обработчик не найден");
                     //   break;
                    }

                }
                while (true);
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");

                    responseResult = _SMEV3Service.GetResponseAsyncSGIO("urn:GetResponse", null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                        return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                        return new BaseDataResult(processLog);

                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryGetSMEVExploitResolutionAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как разрешение на ввод в эксплуатацию - {processedResult}");
                    }
                    if (TryGetSChangePremisesStateAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как разрешение на ввод в эксплуатацию - {processedResult}");
                    }
                    if (TryGetSMEVSocialHireAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как SocialHire - {processedResult}");
                    }
                    if (TryGetSMEVEHouseAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как EmergHouse - {processedResult}");
                    }
                    if (TryGetSMEVOwnerPropertyAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как EmergHouse - {processedResult}");
                    }
                    if (TryGetSMEVRedevelopmentAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как EmergHouse - {processedResult}");
                    }

                }
                while (true);

                //return new BaseDataResult(processLog);
            }
            catch (HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
        }

        #endregion

        #region Private methods             
        private bool TryGetSMEVExploitResolutionAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVExploitResolutionDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVExploitResolutionService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetSChangePremisesStateAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVChangePremisesStateDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVChangePremisesStateService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetSMEVSocialHireAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVSocialHireDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVSocialHireService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetSMEVEHouseAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEmergencyHouseDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVEmergencyHouseService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetSMEVRedevelopmentAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVRedevelopmentDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVRedevelopmentService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetSMEVOwnerPropertyAnswerProcessed(SMEV3Library.Entities.GetResponseResponse.GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVOwnershipPropertyDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVOwnershipPropertyService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }
        private bool TryGetLicRequestRequestProcessed(GetRequestRequest responseResult, ref string result)
        {           

            bool success = _RPGUService.TryProcessResponse(responseResult);
            result = (success ? "успешно" : "ошибка");

            return true;
        }
        #endregion
    }
}
