namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.Suggestion;

    public class CategoryPostsInterceptor : EmptyDomainInterceptor<CategoryPosts>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<CategoryPosts> service, CategoryPosts entity)
        {
            if (Container.Resolve<IDomainService<MessageSubject>>().GetAll().Any(x => x.CategoryPosts.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Сообщения;");
            }

            return Success();
        }
    }
}
