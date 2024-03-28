namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Entities.Suggestion;

    public class SuggestionCommentServiceInterceptor : EmptyDomainInterceptor<SuggestionComment>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<SuggestionComment> service, SuggestionComment entity)
        {
            var filesService = Container.Resolve<IDomainService<SuggestionCommentFiles>>();

            filesService.GetAll()
                        .Where(x => x.SuggestionComment.Id == entity.Id)
                        .Select(x => x.Id)
                        .ForEach(x => filesService.Delete(x));

            return this.Success();
        }
    }
}