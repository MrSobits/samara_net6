namespace Bars.GkhGji.Regions.Tula.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalTypeSurveyInterceptor : EmptyDomainInterceptor<DisposalTypeSurvey>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DisposalTypeSurvey> service, DisposalTypeSurvey entity)
        {
            return service.GetAll().Count(x => x.Disposal.Id == entity.Disposal.Id) > 0
                ? Failure("Добавить можно не больше одной записи.")
                : Success();
        }
    }
}