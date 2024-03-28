namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class DisposalServiceInterceptor : EmptyDomainInterceptor<Disposal>
    {
        public IDomainService<DisposalSurveySubject> DisposalSurveySubjectDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<Disposal> service, Disposal entity)
        {
            return DisposalTypeSurveyDomain.GetAll().Count(x => x.Disposal.Id == entity.Id) != 1
                ? Failure("Не добавлен Тип обследования.")
                : Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Disposal> service, Disposal entity)
        {
            DisposalSurveySubjectDomain.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => DisposalSurveySubjectDomain.Delete(x));

            return Success();
        }
    }
}