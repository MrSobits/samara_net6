namespace Bars.GkhGji.Regions.Tyumen.Interceptors.Suggestion
{
    using B4;

    using Gkh.Entities.Suggestion;

    public class SuggestionCommentInterceptor : EmptyDomainInterceptor<SuggestionComment>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SuggestionComment> service, SuggestionComment entity)
        {
            return ValidationAnswerDate(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SuggestionComment> service, SuggestionComment entity)
        {
            var result = ValidationAnswerDate(service, entity);
            if (!result.Success)
            {
                return result;
            }

            return Success();
        }

        private IDataResult ValidationAnswerDate(IDomainService<SuggestionComment> service, SuggestionComment entity)
        {
            if (entity.AnswerDate.HasValue && entity.AnswerDate.Value.Date < entity.CreationDate.GetValueOrDefault().Date && entity.CreationDate.HasValue)
            {
                return Failure(string.Format("Дата ответа не может быть меньше даты обращения = '{0}'", entity.CreationDate.Value.ToShortDateString()));
            }

            if (entity.AnswerDate.HasValue && entity.CitizenSuggestion.Deadline.HasValue && entity.AnswerDate.Value > entity.CitizenSuggestion.Deadline.Value)
            {
                return
                    Failure(string.Format("Дата ответа должна входить в диапазон между 'Датой обращения'='{0}' и 'Контрольным сроком'='{1}' ", entity.CreationDate.Value.ToShortDateString(),
                        entity.CitizenSuggestion.Deadline.Value.ToShortDateString()));
            }

            return Success();
        }
    }
}