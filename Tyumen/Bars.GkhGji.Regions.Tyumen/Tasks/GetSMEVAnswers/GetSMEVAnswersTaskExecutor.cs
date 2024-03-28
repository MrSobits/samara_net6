using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Tyumen.DomainService;
using Bars.GkhGji.Regions.Tyumen.Entities;
using Bars.GkhGji.Regions.Tyumen.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Tyumen.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Задача на опрос и обработку ответов из смэв
    /// </summary>
    public class GetSMEVAnswersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

     
        public IDomainService<ExternalExchangeTestingFiles> ExternalExchangeTestingFilesDomain { get; set; }
        public IDomainService<SMEVEGRN> SMEVEGRNDomain { get; set; }

        private ISMEV3Service _SMEV3Service;      
        private ISMEVEGRNService _SMEVEGRNService;       

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public GetSMEVAnswersTaskExecutor(ISMEV3Service SMEV3Service,  ISMEVEGRNService SMEVEGRNService)
        {          
            _SMEV3Service = SMEV3Service;         
            _SMEVEGRNService = SMEVEGRNService;          
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
                // остальное
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
