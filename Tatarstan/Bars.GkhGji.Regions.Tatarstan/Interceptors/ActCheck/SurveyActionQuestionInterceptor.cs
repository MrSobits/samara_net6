namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    using Castle.Core.Internal;

    public class SurveyActionQuestionInterceptor : EmptyDomainInterceptor<SurveyActionQuestion>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<SurveyActionQuestion> service, SurveyActionQuestion entity)
        {
            return ValidateEntity(entity);
        }
        
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<SurveyActionQuestion> service, SurveyActionQuestion entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(SurveyActionQuestion entity)
        {
            if (entity.Question.IsNullOrEmpty() && entity.Answer.IsNullOrEmpty())
            {
                return this.Failure("Не заполнены поля \"Вопрос\" и \"Ответ\"");
            }
            
            return this.Success();
        }
    }
}