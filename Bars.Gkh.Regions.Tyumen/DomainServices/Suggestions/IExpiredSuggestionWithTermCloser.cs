namespace Bars.Gkh.Regions.Tyumen.DomainServices.Suggestions
{
    public interface IExpiredSuggestionWithTermCloser
    {
        /// <summary>
        /// Закрыть обращения с истекшим сроком
        /// </summary>
        void Close();
    }
}
