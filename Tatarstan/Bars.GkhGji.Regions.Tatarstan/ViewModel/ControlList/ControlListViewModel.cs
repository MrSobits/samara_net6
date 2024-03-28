namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ControlList
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    public class ControlListViewModel : BaseViewModel<TatarstanControlList>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TatarstanControlList> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            return domainService.GetAll()
                .Where(x => x.Disposal.Id == documentId)
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}
