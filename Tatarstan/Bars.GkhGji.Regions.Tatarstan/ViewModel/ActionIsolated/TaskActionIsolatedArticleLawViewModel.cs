namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedArticleLawViewModel : BaseViewModel<TaskActionIsolatedArticleLaw>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskActionIsolatedArticleLaw> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.Task.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.ArticleLaw.Name,
                    x.ArticleLaw.Description
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}