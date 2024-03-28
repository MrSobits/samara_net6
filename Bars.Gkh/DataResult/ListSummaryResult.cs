namespace Bars.Gkh.DataResult
{
    using Bars.B4;

    using Newtonsoft.Json;

    /// <summary>
    /// Временный класс для результата списка данных и строки с Итогами
    /// </summary>
    public class ListSummaryResult : ListDataResult
    {
        public ListSummaryResult(object data, int totalCount)
            : base(data, totalCount)
        {
        }
        
        public ListSummaryResult(object data, int totalCount, object summaryData)
            : this(data, totalCount)
        {
            this.SummaryData = summaryData;
        }

        [JsonProperty("summaryData")]
        public object SummaryData { get; set; }
    }
}
