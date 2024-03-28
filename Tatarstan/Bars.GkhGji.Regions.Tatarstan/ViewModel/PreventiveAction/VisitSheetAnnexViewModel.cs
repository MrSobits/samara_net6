namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class VisitSheetAnnexViewModel : BaseViewModel<VisitSheetAnnex>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<VisitSheetAnnex> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            return domainService
                .GetAll()
                .Where(x => x.VisitSheet.Id == id)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}