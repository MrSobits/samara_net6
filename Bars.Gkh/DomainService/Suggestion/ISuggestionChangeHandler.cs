namespace Bars.Gkh.DomainService
{
    using Entities.Suggestion;

    public interface ISuggestionChangeHandler
    {
        /// <summary>
        /// Применить изменение состояния (отправить уведомление, сохранить историю, проставить статус)
        /// </summary>
        void ApplyChange(SuggestionComment comment, Transition transition);

        void SendEmailApplicant(SuggestionComment comment);
    }
}