namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    public class SurveyActionInterceptor : InheritedActCheckActionBaseInterceptor<SurveyAction>
    {
        /// <inheritdoc />
        protected override void DeleteInheritedActionAdditionalEntities(SurveyAction entity)
        {
            var surveyActionQuestionService = this.Container.Resolve<IDomainService<SurveyActionQuestion>>();

            using (this.Container.Using(surveyActionQuestionService))
            {
                surveyActionQuestionService
                    .GetAll()
                    .Where(x => x.SurveyAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => surveyActionQuestionService.Delete(x.Id));
            }
        }
    }
}