namespace Bars.Gkh.Regions.Tyumen.Controllers
{
    using B4;
    using B4.Modules.FileStorage;
    using DomainServices.Suggestions;
    using Entities.Suggestion;

    using Microsoft.AspNetCore.Mvc;

    public class SuggestionCommentController : FileStorageDataController<SuggestionComment>
    {
        public ActionResult ChangeExecutorType(BaseParams baseParams)
        {
            var service = this.Resolve<ISuggestionCommentService>();
            try
            {
                var result = service.ApplyExecutor(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}