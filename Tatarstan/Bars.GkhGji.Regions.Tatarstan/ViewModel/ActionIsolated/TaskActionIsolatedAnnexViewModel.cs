namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedAnnexViewModel : BaseViewModel<TaskActionIsolatedAnnex>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskActionIsolatedAnnex> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.Task.Id == documentId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}