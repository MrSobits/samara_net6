namespace Bars.Gkh.Interceptors
{
    using B4;
    using B4.Modules.States;

    using Bars.B4.Utils;

    using Entities.Suggestion;
    using System.Linq;

    public class CitizenSuggestionServiceInterceptor : EmptyDomainInterceptor<CitizenSuggestion>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CitizenSuggestion> service, CitizenSuggestion entity)
        {
            entity.Year = entity.CreationDate.Year;

            entity.Num = service.GetAll()
                .Where(x => x.Year == entity.Year)
                .GroupBy(x => x.Year)
                .Select(x => x.Max(y => y.Num))
                .FirstOrDefault() + 1;

            entity.Number = string.Format("{0}-{1}", entity.Year.ToString().Substring(2, 2), entity.Num);
            
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CitizenSuggestion> service, CitizenSuggestion entity)
        {
            var filesService = Container.Resolve<IDomainService<CitizenSuggestionFiles>>();
            var commentService = Container.Resolve<IDomainService<SuggestionComment>>();
            var commentFilesService = Container.Resolve<IDomainService<SuggestionCommentFiles>>();
            var sugHistoryService = Container.Resolve<IDomainService<CitizenSuggestionHistory>>();

            filesService.GetAll()
                        .Where(x => x.CitizenSuggestion.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => filesService.Delete(x));

            commentFilesService.GetAll()
                        .Where(x => x.SuggestionComment.CitizenSuggestion.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => commentService.Delete(x));

            commentService.GetAll()
                        .Where(x => x.CitizenSuggestion.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => commentService.Delete(x));

            sugHistoryService.GetAll()
                        .Where(x => x.CitizenSuggestion.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => sugHistoryService.Delete(x));

            return this.Success();
        }
    }
}