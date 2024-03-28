namespace Bars.Gkh.DataResult
{
    using System.Collections;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Utils;

    using Newtonsoft.Json;

    public class LoggingDataResult : BaseDataResult
    {
        public LoggingDataResult()
        {
            this.Success = true;
        }
        
        public LoggingDataResult(bool success, object data)
        {
            this.Success = success;
            this.Data = data;
        }
        
        public LoggingDataResult(bool success, string message, object data, string stackTrace, ErrorAggregator error)
        {
            this.Success = success;
            this.Data = data;
            this.Message = message;
            this.StackTrace = stackTrace;
            this.Error = error;
        }

        /// <summary>
        /// Иерархичный объект ошибки
        /// </summary>
        [JsonProperty("Error")]
        private ErrorAggregator Error { get; set; }

        /// <summary>
        /// Трассировка ошибки
        /// </summary>
        [JsonProperty("StackTrace")]
        public string StackTrace { get; set; }
    }
}