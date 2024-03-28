namespace Bars.Gkh.Controllers.Dict
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Entities.Suggestion;

    public class MessageSubjectController : B4.Alt.DataController<MessageSubject>
    {

        public ActionResult ListMessages(BaseParams baseParams)
        {
            var categoryId = baseParams.Params.GetAs<long>("categoryId");
            var loadParam = baseParams.GetLoadParam();

            var messages = DomainService.GetAll().Where(x => x.CategoryPosts.Id == categoryId)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    CategotyPostsId = x.CategoryPosts.Id
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            var totalCount = messages.Count();

            return new JsonListResult(messages.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}