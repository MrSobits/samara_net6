namespace Bars.Gkh.StateChanges
{
    using System.Linq;
    using B4.Utils;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Suggestion;

    public class CitizenSuggestionHasAnswerRule : IRuleChangeStatus
    {

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var suggestion = statefulEntity as CitizenSuggestion;

            if (suggestion == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }

            var container = ApplicationContext.Current.Container;
            var suggestionCommentDomain = container.ResolveDomain<SuggestionComment>();

            var suggestionAnswers = suggestionCommentDomain.GetAll()
                .Where(x => suggestion.Id == x.CitizenSuggestion.Id)
                .Select(x => new
                {
                    x.Id,
                    sugId = x.CitizenSuggestion.Id,
                    x.Answer
                }).AsEnumerable()
                .GroupBy(x => x.sugId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Id).Select(y => y.Answer).FirstOrDefault());

            var hasAnswers = !string.IsNullOrEmpty(suggestionAnswers.Get(suggestion.Id));

            if (!hasAnswers)
            {
                return ValidateResult.No("Для закрытия обращения необходимо заполнить поле Дата ответа и поле Ответ.");
            }
            return ValidateResult.Yes();
        }

        public string Id
        {
            get { return "CitizenSuggestionHasAnswerRule"; }
        }

        public string Name { get { return "Проверка на завершенность обработки обращения"; } }
        public string TypeId { get { return "gkh_citizen_suggestion"; } }
        public string Description
        {
            get
            {
                return "При переводе обращения в статус с признаком \"конечный\"," +
                       " система должна проверять на завершенность обработки обращения," +
                       " т.е заполнена дата ответа и сам ответ";
            }
        }
    }
}
