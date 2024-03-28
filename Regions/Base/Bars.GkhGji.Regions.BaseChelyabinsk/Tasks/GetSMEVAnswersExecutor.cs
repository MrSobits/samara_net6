using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Tasks
{
    /// <summary>
    /// Задача на опрос и обработку ответов из смэв
    /// </summary>
    public class GetSMEVAnswersExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<LogOperation> LogOperationDomainService { get; set; }
        public IDomainService<SMEVCertInfo> SMEVCertInfoDomain { get; set; }

        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }

        private ISMEV3Service _SMEV3Service;
        private ISMEVCertInfoService _SMEVCertInfoService;
        private IFileManager _fileManager;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetSMEVAnswersExecutor(ISMEV3Service SMEV3Service, ISMEVCertInfoService SMEVCertInfoService, IFileManager fileManager)
        {            
            _SMEV3Service = SMEV3Service;
            _SMEVCertInfoService = SMEVCertInfoService;
            _fileManager = fileManager;
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
                do
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");

                    responseResult = _SMEV3Service.GetResponseAsync(null, null, true).GetAwaiter().GetResult();

                    //Если сервер прислал ошибку, возвращаем как есть
                    if (responseResult.FaultXML != null)
                        return new BaseDataResult(false, responseResult.FaultXML.ToString());

                    //если результатов пока нет, возврат
                    if (!responseResult.isAnswerPresent)
                        return new BaseDataResult(processLog);

                    indicator?.Report(null, 0, $"Обработка ответа {number}");

                    string processedResult = null;

                    if (TryCertInfoAnswerProcessed(responseResult, ref processedResult))
                    {
                        processLog.Add($"Сообщение {responseResult.OriginalMessageId} - обработано - {processedResult}");
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
        
        private bool TryCertInfoAnswerProcessed(GetResponseResponse responseResult, ref string result)
        {

            var requestData = SMEVCertInfoDomain.GetAll().Where(x => x.MessageId == responseResult.OriginalMessageId).FirstOrDefault();

            if (requestData == null)
                return false; //это не сообщение ЕГРН

            bool success = _SMEVCertInfoService.TryProcessResponse(requestData, responseResult);
            result = (success ? "успешно" : "ошибка") + ": " + requestData.Answer;

            return true;
        }

        #endregion
    }
}
