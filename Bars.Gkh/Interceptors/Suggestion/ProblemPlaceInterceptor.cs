namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Suggestion;

    public class ProblemPlaceInterceptor : EmptyDomainInterceptor<ProblemPlace>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ProblemPlace> service, ProblemPlace entity)
        {
            return CheckProblemPlaceForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ProblemPlace> service, ProblemPlace entity)
        {
            return CheckProblemPlaceForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ProblemPlace> service, ProblemPlace entity)
        {
            var hasSuggestion =
                Container.Resolve<IDomainService<CitizenSuggestion>>()
                    .GetAll()
                    .Any(x => x.ProblemPlace.Id == entity.Id);

            if (hasSuggestion)
            {
                return Failure("Невозможно удалить это место, так как есть обращения граждан, связанные с ним!");
            }

            return base.BeforeDeleteAction(service, entity);
        }

        private IDataResult CheckProblemPlaceForm(ProblemPlace entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнено обязательное поле Наименование");
            }

            if (entity.Name.Length > 250)
            {
                return Failure("Количество знаков в поле Наименование  не должно превышать 250 символов");
            }

            return Success();
        }
    }
}