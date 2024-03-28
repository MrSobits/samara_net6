using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Задача на опрос и обработку ответов из смэв
    /// </summary>
    public class GetSMEVAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGmp> GisGmpDomain { get; set; }
        public IDomainService<GISERP> GISERPDomain { get; set; }
        public IDomainService<SMEVERULReqNumber> SMEVERULReqNumberDomain { get; set; }
        public IDomainService<SMEVCertInfo> SMEVCertInfoDomain { get; set; }
        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }
        public IDomainService<SMEVEGRUL> SMEVEGRULDomain { get; set; }
        public IDomainService<SMEVEGRIP> SMEVEGRIPDomain { get; set; }
        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }
        public IDomainService<SMEVEDO> SMEVEDODomain { get; set; }
        public IDomainService<ERKNM> ERKNMDomain { get; set; }
        public IDomainService<ERKNMDict> ERKNMDictDomain { get; set; }
        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }
        public IDomainService<MVDPassport> MVDPassportDomain { get; set; }
        public IDomainService<MVDLivingPlaceRegistration> MVDLivingPlaceRegistrationDomain { get; set; }
        public IDomainService<MVDStayingPlaceRegistration> MVDStayingPlaceRegistrationDomain { get; set; }

        private ISMEV3Service _SMEV3Service;
        private IGISGMPService _GISGMPService;
        private IPAYREGService _PAYREGService;
        private ISMEVEGRULService _SMEVEGRULService;
        private ISMEVEGRIPService _SMEVEGRIPService;
        private ISMEVMVDService _SMEVMVDService;
        private IGISERPService _GISERPService;
        private IERKNMService _ERKNMService;
        private ISMEVEGRNService _SMEVEGRNService;
        private ISMEVCertInfoService _SMEVCertInfoService;
        private ISMEVERULService _SMEVERULService;
        private ISMEVEDOService _SMEVEDOService;
        private IMVDPassportService _MVDPassportService;
        private IMVDLivingPlaceRegistrationService _MVDLivingPlaceRegistrationService;
        private IMVDStayingPlaceRegistrationService _MVDStayingPlaceRegistrationService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetSMEVAnswersTaskExecutor(IGISGMPService GISGMPService, IPAYREGService PAYREGService, ISMEVEGRULService SMEVEGRULService, ISMEV3Service SMEV3Service,
            IMVDLivingPlaceRegistrationService MVDLivingPlaceRegistrationService, IMVDStayingPlaceRegistrationService MVDStayingPlaceRegistrationService, IMVDPassportService MVDPassportService,
            ISMEVEGRIPService SMEVEGRIPService, ISMEVMVDService SMEVMVDService, IGISERPService GISERPService, ISMEVEGRNService SMEVEGRNService,
            ISMEVCertInfoService SMEVCertInfoService, ISMEVEDOService doServ, ISMEVERULService SMEVERULService, IERKNMService ERKNMService)
        {
            _GISGMPService = GISGMPService;
            _PAYREGService = PAYREGService;
            _SMEVEGRULService = SMEVEGRULService;
            _SMEVEGRIPService = SMEVEGRIPService;
            _SMEVMVDService = SMEVMVDService;
            _SMEV3Service = SMEV3Service;
            _ERKNMService = ERKNMService;
            _GISERPService = GISERPService;
            _SMEVEGRNService = SMEVEGRNService;
            _SMEVCertInfoService = SMEVCertInfoService;
            _SMEVERULService = SMEVERULService;
            _MVDLivingPlaceRegistrationService = MVDLivingPlaceRegistrationService;
            _MVDStayingPlaceRegistrationService = MVDStayingPlaceRegistrationService;
            _MVDPassportService = MVDPassportService;
            _SMEVEDOService = doServ;
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
                    else
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработчик не найден");
                    }
                }
                while (true);
                // остальное
                do
                {
                    processLog.Add($"Общая очередь; ");
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");

                    responseResult = _SMEV3Service.GetResponseAsync(null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                        return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                    {
                        processLog.Add($"Ответов нет; ");
                        return new BaseDataResult(processLog);
                    }

                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryERKNMAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕРКНМ - {processedResult}");
                    }
                    if (TryERKNMDictAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано справочник из ЕРКНМ - {processedResult}");
                    }
                    if (TryGISGMPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ГИС ГМП - {processedResult}");
                    }
                    else if (TryPAYREGAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как оплаты из ГИС ГМП - {processedResult}");
                    }
                    else if (TryERULAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕРУЛ - {processedResult}");
                    }
                    else if (TryEGRULAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРЮЛ - {processedResult}");
                    }
                    else if (TryEGRIPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как ЕГРИП - {processedResult}");
                    }
                    else if (TryMVDAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как МВД - {processedResult}");
                    }
                    else if (TryGetProcecutorOfficeAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как Справочник отделов прокуратур - {processedResult}");
                    }
                    else if (TryGISERPAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как проверка ГИС ЕРП - {processedResult}");
                    }
                    else if (TrySmevCertAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано как отправка сертификата - {processedResult}");
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

        /// <summary>
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
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
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryERKNMAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            ERKNM requestData = ERKNMDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _ERKNMService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        /// <summary>
        /// Проверяет сообщение, что оно справочник из ЕРКНМ, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryERKNMDictAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            ERKNMDict requestData = ERKNMDictDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _ERKNMService.TryProcessGetDictResponse(requestData, responseResult, null);
            result = (success ? "Успешно" : "Ошибка") + ": " + requestData.Answer;

            return true;
        }

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

        /// <summary>
        /// Проверяет сообщение, что оно ГИС ГМП, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TrySmevCertAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            var requestData = SMEVCertInfoDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ГИС ГМП

            var success = _SMEVCertInfoService.TryProcessResponse(requestData, responseResult, null);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
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
                return false;
            }
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

        /// <summary>
        /// Проверяет сообщение, что оно ЕГРЮЛ, и обрабатывает его, если так
        /// </summary>
        /// <returns>true, если обработано</returns>
        private bool TryEGRULAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRULDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРЮЛ

            bool success = _SMEVEGRULService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryEGRIPAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVEGRIPDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРИП

            bool success = _SMEVEGRIPService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryMVDAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVMVDDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение МВД

            bool success = _SMEVMVDService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        private bool TryGetProcecutorOfficeAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {
            return false;
            var requestData = ExternalExchangeTestingFilesDomain.GetAll()
                .Where(x=> x.ClassName == "ProsecutorOffice")
                .Where(x => x.ClassDescription == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение с офисами прокуратуры

            bool success = _GISERPService.TryProcessGetProsecutorOfficeResponse(responseResult);
            result = (success ? "успешно" : "ошибка");

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

        #endregion
    }
}
