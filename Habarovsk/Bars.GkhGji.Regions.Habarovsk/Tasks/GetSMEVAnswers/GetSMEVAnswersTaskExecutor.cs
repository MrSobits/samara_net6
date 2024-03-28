using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.DomainService;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVPremises;
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
    public class GetSMEVAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GISERP> GISERPDomain { get; set; }
        public IDomainService<ERKNM> ERKNMDomain { get; set; }
        public IDomainService<SMEVERULReqNumber> SMEVERULReqNumberDomain { get; set; }
        public IDomainService<LogOperation> LogOperationDomainService { get; set; }
        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }
        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }
        public IDomainService<SMEVExploitResolution> SMEVExploitResolutionDomain { get; set; }
        public IDomainService<SMEVChangePremisesState> SMEVChangePremisesStateDomain { get; set; }
        public IDomainService<SMEVNDFL> SMEVNDFLDomain { get; set; }
        public IDomainService<GASU> GASUDomain { get; set; }
        public IDomainService<SMEVDISKVLIC> SMEVDISKVLICDomain { get; set; }
        public IDomainService<SMEVFNSLicRequest> SMEVFNSLicRequestDomain { get; set; }
        public IDomainService<SMEVPremises> SMEVPremisesDomain { get; set; }
        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }
        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }
        public IDomainService<SMEVEDO> SMEVEDODomain { get; set; }
        public IDomainService<MVDPassport> MVDPassportDomain { get; set; }
        public IDomainService<MVDLivingPlaceRegistration> MVDLivingPlaceRegistrationDomain { get; set; }
        public IDomainService<MVDStayingPlaceRegistration> MVDStayingPlaceRegistrationDomain { get; set; }
        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }
        public IDomainService<GisGmp> GisGmpDomain { get; set; }
        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }
        public IDomainService<SMEVSNILS> SMEVSNILSDomain { get; set; }

        private ISMEV3Service _SMEV3Service;
        private ISMEVNDFLService _SMEVNDFLService;
        private ISMEVDISKVLICService _SMEVDISKVLICService;
        private ISMEVFNSLicRequestService _SMEVFNSLicRequestService;
        private ISMEVPremisesService _SMEVPremisesService;
        private IGISERPService _GISERPService;
        private ISMEVEGRNService _SMEVEGRNService;
        private ISMEVEGRULService _SMEVEGRULService;
        private ISMEVEGRIPService _SMEVEGRIPService;
        private ISMEVEDOService _SMEVEDOService;
        private IMVDPassportService _MVDPassportService;
        private IMVDLivingPlaceRegistrationService _MVDLivingPlaceRegistrationService;
        private IMVDStayingPlaceRegistrationService _MVDStayingPlaceRegistrationService;
        private ISMEVMVDService _SMEVMVDService;
        private IGISGMPService _GISGMPService;
        private IPAYREGService _PAYREGService;
        private ISMEVSNILSService _SMEVSNILSService;
        private IGASUService _GASUService;
        private IERKNMService _ERKNMService;
        private IComplaintsService _ComplaintsService;
        private IFileManager _fileManager;
        private ISMEVERULService _SMEVERULService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetSMEVAnswersTaskExecutor(ISMEV3Service SMEV3Service, IGISERPService GISERPService, ISMEVEGRNService SMEVEGRNService, IFileManager fileManager, 
            ISMEVEGRIPService egrip, ISMEVEGRULService egrul, ISMEVMVDService mvd, ISMEVFNSLicRequestService fns, IGISGMPService gmp, IPAYREGService payreg,
            ISMEVDISKVLICService diskvlic, ISMEVPremisesService premises, ISMEVNDFLService ndfl, ISMEVSNILSService snils, IMVDLivingPlaceRegistrationService MVDLivingPlaceRegistrationService, IMVDStayingPlaceRegistrationService MVDStayingPlaceRegistrationService,
            IGASUService gasu, ISMEVEDOService doServ, IComplaintsService complServ, IERKNMService erknm, ISMEVERULService SMEVERULService, IMVDPassportService MVDPassportService)
        {            
            _SMEV3Service = SMEV3Service;
            _GISERPService = GISERPService;
            _SMEVEGRNService = SMEVEGRNService;
            _fileManager = fileManager;
            _SMEVEGRIPService = egrip;
            _SMEVEGRULService = egrul;
            _SMEVMVDService = mvd;
            _MVDLivingPlaceRegistrationService = MVDLivingPlaceRegistrationService;
            _MVDStayingPlaceRegistrationService = MVDStayingPlaceRegistrationService;
            _SMEVFNSLicRequestService = fns;
            _GISGMPService = gmp;
            _PAYREGService = payreg;
            _SMEVNDFLService = ndfl;
            _SMEVSNILSService = snils;
            _SMEVDISKVLICService = diskvlic;
            _SMEVPremisesService = premises;
            _GASUService = gasu;
            _SMEVEDOService = doServ;
            _ComplaintsService = complServ;
            _SMEVERULService = SMEVERULService;
            _ERKNMService = erknm;
            _MVDPassportService = MVDPassportService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var processLog = new List<string>();
            GetResponseResponse responseResult;
            uint number = 0;

            try
            {
              //  _SMEV3Service.GetAckAsync("0c9cf186-01df-11ed-bc46-52540000001e", true).GetAwaiter().GetResult();
                //_SMEV3Service.GetAckAsync("895e9822-fdf8-11ec-bf50-e44629100c7d", true).GetAwaiter().GetResult();
                //_SMEV3Service.GetAckAsync("50f1590b-0063-11ed-99e3-fa4d34ee99dd", true).GetAwaiter().GetResult();
                //_SMEV3Service.GetAckAsync("cf72a89c-01de-11ed-83ec-fbb9ca154716", true).GetAwaiter().GetResult();
                //ЕГРН
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    responseResult = _SMEV3Service.GetResponseAsync(@"urn://x-artefacts-rosreestr-gov-ru/virtual-services/egrn-statement/1.1.2", @"Request", true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                    {
                        processLog.Add($"Запрос ЕГРН {number} - ошибка: {responseResult.FaultXML}; ");
                        break;
                    }
                    //   return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                    {
                        processLog.Add($"Запрос ЕГРН {number} - ответ пуст; ");
                        break;
                    }
                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryEGRNAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРН - {processedResult}");
                    }
                    else if (TryDOAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как тест - {processedResult}");
                    }
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }
                }
                while (true);
                // остальное
                do
                {
                    // МВД
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    processLog.Add($"Запрос МВД {number}; ");
                    responseResult = _SMEV3Service.GetResponseAsync(@"urn://ru/mvd/ibd-m/convictions/search/1.0.2", @"request", true).GetAwaiter().GetResult();
                    //if (responseResult != null && responseResult.MessagePrimaryContent != null)
                    //{
                    //    processLog.Add("2 трай " + number + "респонс не нулл");
                    //    mpc = responseResult.MessagePrimaryContent.ToString();
                    //}
                    //else
                    //{
                    //    processLog.Add("2 трай " + number + "респонс нулл");
                    //    break;
                    //}
                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                    {
                        processLog.Add($"Запрос МВД {number} - ошибка: {responseResult.FaultXML}; ");
                        break;
                    }
                    //   return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                    {
                        processLog.Add($"Запрос МВД {number} - ответ пуст; ");
                        break;
                    }
                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;
                    processLog.Add("Обработка " + responseResult.OriginalMessageId);
                    if (TryMVDAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как МВД - {processedResult}");
                    }
                    else if (TryDOAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как тест - {processedResult}");
                    }
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }
                }
                while (true);
                do
                {
                    // МВД
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    processLog.Add($"Запрос МВД {number}; ");
                    responseResult = _SMEV3Service.GetResponseAsync(@"urn://mvd/guvm/staying-place-registration/1.1.0", @"stayingPlaceRegistrationRequest", true).GetAwaiter().GetResult();
                    //if (responseResult != null && responseResult.MessagePrimaryContent != null)
                    //{
                    //    processLog.Add("2 трай " + number + "респонс не нулл");
                    //    mpc = responseResult.MessagePrimaryContent.ToString();
                    //}
                    //else
                    //{
                    //    processLog.Add("2 трай " + number + "респонс нулл");
                    //    break;
                    //}
                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                    {
                        processLog.Add($"Запрос МВД {number} - ошибка: {responseResult.FaultXML}; ");
                        break;
                    }
                    //   return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                    {
                        processLog.Add($"Запрос МВД {number} - ответ пуст; ");
                        break;
                    }
                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;
                    processLog.Add("Обработка " + responseResult.OriginalMessageId);
                    if (TryMVDStayingPlaceRegistrationAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как МВД - {processedResult}");
                    }
                    else if (TryDOAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как тест - {processedResult}");
                    }
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }
                }
                while (true);
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    processLog.Add($"Все запросы {number}; ");
                    responseResult = _SMEV3Service.GetResponseAsync(null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                        return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                        return new BaseDataResult(processLog);

                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;
                    if (TryGetComplaintsProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как запрос досудебного обжалования - {processedResult}");
                    }
                    if (TryGetProcecutorOfficeAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как Справочник отделов прокуратур - {processedResult}");
                    }
                    else if (TryGISERPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как проверка ГИС ЕРП - {processedResult}");
                    }
                    else if (TryERKNMAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как проверка ЕРКНМ - {processedResult}");
                    }
                    else if (TryFNSLicRequestAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как лицензии ФНС - {processedResult}");
                    }
                    else if (TryEGRULAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРЮЛ - {processedResult}");
                    }
                    else if (TryERULAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕРУЛ - {processedResult}");
                    }
                    else if (TryEGRIPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРИП - {processedResult}");
                    }
                    else if (TryDOAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как тест - {processedResult}");
                    }
                    else if (TryMVDPasspostAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как запрос сведений о паспорте РФ - {processedResult}");
                    }
                    else if (TryMVDLivingPlaceRegistrationAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как запрос сведений о месте регистрации - {processedResult}");
                    }
                    else if (TryMVDStayingPlaceRegistrationAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как запрос сведений о месте пребывания - {processedResult}");
                    }
                    //else if (TryMVDAnswerProcessed(responseResult, ref processedResult))
                    //{
                    //    processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как МВД - {processedResult}");
                    //}
                    else if(TryGISGMPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ГИС ГМП - {processedResult}");
                    }
                    else if (TryPAYREGAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как оплаты из ГИС ГМП - {processedResult}");
                    }
                    else if (TryPremisesProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ВС по помещениям - {processedResult}");
                    }
                    else if (TryDISKVLICProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ВС дисквал. лиц - {processedResult}");
                    }
                    else if (TryNDFLProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ВС 2-НДФЛ - {processedResult}");
                    }
                    else if (TrySNILSProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ВС СНИЛС - {processedResult}");
                    }
                    else if (TryGASUProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ВС разрешений на ввод в эксплуатацию  - {processedResult}");
                    }
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }

                }
                while (true);
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

        /// <summary>
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryGISERPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            GISERP requestData = GISERPDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _GISERPService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryERKNMAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            try
            {
                ERKNM requestData = ERKNMDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение ГИС ГМП

                var success = _ERKNMService.TryProcessResponse(requestData, responseResult, null);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;
                return true;
               
            }
            catch
            {
                return false;
            }
        }

        private bool TryGetProcecutorOfficeAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = ExternalExchangeTestingFilesDomain.GetAll()
                .Where(x=> x.ClassName == "ProsecutorOffice")
                .Where(x => x.ClassDescription == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение с офисами прокуратуры

            bool success = _GISERPService.TryProcessGetProsecutorOfficeResponse(responseResult);
            result = (success ? "успешно" : "ошибка");

            return true;
        }

        private bool TryFNSLicRequestAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            SMEVFNSLicRequest requestData = SMEVFNSLicRequestDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не ФНС

            var success = _SMEVFNSLicRequestService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryEGRNAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRNDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРН

            bool success = _SMEVEGRNService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryEGRULAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRULDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVEGRULService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно ЕРУЛ, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryERULAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVERULReqNumberDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVERULService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGASUProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = GASUDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _GASUService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryEGRIPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            string log = responseResult.OriginalMessageId;
            try
            {
                if (SMEVEGRIPDomain == null)
                {
                    log += "Домен сервис нулл";
                }
                var requestData = SMEVEGRIPDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение ЕГРИП

                bool success = _SMEVEGRIPService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch (Exception e)
            {
                var memStream = new MemoryStream();
                var streamWriter = new StreamWriter(memStream);

               
                streamWriter.WriteLine("TEST");
                streamWriter.WriteLine(e.StackTrace);
                streamWriter.WriteLine(e.Message);

                streamWriter.Flush();                                   
                memStream.Seek(0, SeekOrigin.Begin);
                this.LogOperationDomainService.Save(new LogOperation
                {
                    OperationType = Gkh.Enums.LogOperationType.FormatDataExport,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Comment = log + " " + e.Message,
                    LogFile = _fileManager.SaveFile(memStream, "log.txt")
                });
                return false;
            }
        }

        private bool TryDOAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            string log = responseResult.OriginalMessageId;
            try
            {
                if (SMEVEDODomain == null)
                {
                    log += "Домен сервис нулл";
                }
                var requestData = SMEVEDODomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение ЕГРИП

                bool success = _SMEVEDOService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch (Exception e)
            {
                var memStream = new MemoryStream();
                var streamWriter = new StreamWriter(memStream);


                streamWriter.WriteLine("TEST");
                streamWriter.WriteLine(e.StackTrace);
                streamWriter.WriteLine(e.Message);

                streamWriter.Flush();
                memStream.Seek(0, SeekOrigin.Begin);
                this.LogOperationDomainService.Save(new LogOperation
                {
                    OperationType = Gkh.Enums.LogOperationType.FormatDataExport,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Comment = log + " " + e.Message,
                    LogFile = _fileManager.SaveFile(memStream, "log.txt")
                });
                return false;
            }
        }

        private bool TryMVDAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            try
            {
                var requestData = SMEVMVDDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение МВД
                bool success = _SMEVMVDService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryMVDPasspostAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            try
            {
                var requestData = MVDPassportDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение МВД
                bool success = _MVDPassportService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool TryMVDLivingPlaceRegistrationAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            try
            {
                var requestData = MVDLivingPlaceRegistrationDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение МВД
                bool success = _MVDLivingPlaceRegistrationService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch
            {
                return false;
            }
        }
        private bool TryMVDStayingPlaceRegistrationAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            try
            {
                var requestData = MVDStayingPlaceRegistrationDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение МВД
                bool success = _MVDStayingPlaceRegistrationService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool TryGetComplaintsProcessed(GetResponseResponse responseResult, ref string result)
        {
            try
            {
                var requestData = SMEVComplaintsRequestDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

                if (requestData == null)
                    return false; //это не сообщение МВД
                bool success = _ComplaintsService.TryProcessResponse(requestData, responseResult);
                result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryGISGMPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            GisGmp requestData = GisGmpDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _GISGMPService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно - оплаты из ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryPAYREGAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            PayRegRequests requestData = PayRegRequestsDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение с оплатами из ГИС ГМП

            var success = _PAYREGService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryPremisesProcessed(GetResponseResponse responseResult, ref string result)
        {
            SMEVPremises requestData = SMEVPremisesDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false;

            var success = _SMEVPremisesService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryDISKVLICProcessed(GetResponseResponse responseResult, ref string result)
        {
            SMEVDISKVLIC requestData = SMEVDISKVLICDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false;

            var success = _SMEVDISKVLICService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryNDFLProcessed(GetResponseResponse responseResult, ref string result)
        {
            SMEVNDFL requestData = SMEVNDFLDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false;

            var success = _SMEVNDFLService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TrySNILSProcessed(GetResponseResponse responseResult, ref string result)
        {
            SMEVSNILS requestData = SMEVSNILSDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false;

            var success = _SMEVSNILSService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }   
     
        #endregion
    }
}
