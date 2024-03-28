namespace Bars.Gkh.StateChanges
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Suggestion;
    using Castle.Windsor;
    using Domain.Suggestions;
    using DomainService;
    using Enums;

    public class CitizenSuggestionStateRule : IRuleChangeStatus
    {
        private IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var suggestion = statefulEntity as CitizenSuggestion;

            if (suggestion == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }

            var container = ApplicationContext.Current.Container;
            var suggestionCommentDomain = container.ResolveDomain<SuggestionComment>();
            var transitionDomain = container.ResolveDomain<Transition>();

            try
            {
                var comment = suggestionCommentDomain
                    .GetAll().First(x => suggestion.Id == x.CitizenSuggestion.Id && x.IsFirst);

                var executorType = comment.GetCurrentExecutorType();

                if (executorType == ExecutorType.None)
                {
                    return ValidateResult.No("Необходимо указать исполнителя");
                }

                var transition = transitionDomain
                    .GetAll()
                    .Where(x => x.Rubric.Id == comment.CitizenSuggestion.Rubric.Id)
                    .FirstOrDefault(x => x.InitialExecutorType == executorType);

                comment.CreationDate = DateTime.UtcNow;

                comment.CitizenSuggestion.Deadline = DateTime.Now.AddDays(transition.Return(x => x.ExecutionDeadline));

                if (Container.Kernel.HasComponent(typeof(ISuggestionChangeHandler)))
                {
                    var handler = Container.Resolve<ISuggestionChangeHandler>();
                    handler.ApplyChange(comment, transition);
                }
            }
            finally
            {
                container.Release(suggestionCommentDomain);
                container.Release(transitionDomain);
            }

            return ValidateResult.Yes();
        }

        public string Id
        {
            get { return "CitizenSuggestionStateRule"; }
        }

        public string Name { get { return "Проставление контрольного срока и начальной даты обращения для исполниеля"; } }
        public string TypeId { get { return "gkh_citizen_suggestion"; } }
        public string Description
        {
            get
            {
                return "Проставление контрольного срока и начальной даты обращения для исполниеля";
            }
        }
    }
}
