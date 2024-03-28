namespace Bars.Gkh.StateChanges
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Suggestion;
    using Domain.Suggestions;
    using Enums;

    public class CitizenSuggestionExecutorState : IRuleChangeStatus
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

            try
            {
                var comment = suggestionCommentDomain
                    .GetAll().First(x => suggestion.Id == x.CitizenSuggestion.Id && x.IsFirst);

                var executorType = comment.GetCurrentExecutorType();

                if (executorType != ExecutorType.None)
                {
                    return ValidateResult.No("Изменение статуса запрещено. Назначен исполнитель");
                }
            }
            finally
            {
                container.Release(suggestionCommentDomain);
            }

            return ValidateResult.Yes();
        }

        public string Id
        {
            get { return "CitizenSuggestionExecutorState"; }
        }

        public string Name { get { return "Изменение статуса с назначенным исполнителем"; } }
        public string TypeId { get { return "gkh_citizen_suggestion"; } }
        public string Description
        {
            get
            {
                return "Изменение статуса с назначенным исполнителем";
            }
        }
    }
}
