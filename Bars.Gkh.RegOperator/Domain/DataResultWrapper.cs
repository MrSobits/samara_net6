namespace Bars.Gkh.RegOperator.Domain
{
    using B4;

    public class DataResultWrapper<TResult> : IDataResult<TResult> where TResult : class
    {
        public DataResultWrapper(TResult result, bool success = true, string message = null)
        {
            Data = result;
            Success = success;
            Message = message;
        }

        /// <summary>
        /// Успешность.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Данные.
        /// </summary>
        public TResult Data { get; set; }

        /// <summary>
        /// Данные.
        /// </summary>
        object IDataResult.Data
        {
            get { return Data; }
            set { Data = (TResult)value; }
        }
    }
}