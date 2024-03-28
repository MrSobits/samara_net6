namespace Bars.Gkh.Domain.Suggestions
{
    /// <summary>
    /// Контракт закрытия обращений с истекшим сроком
    /// </summary>
    public interface IExpiredSuggestionCloser
    {
        /// <summary>
        /// Закрыть обращения с истекшим сроком
        /// </summary>
        /// <param name="waitDays">Срок обращения в днях</param>
        void Close(int waitDays);
    }
}