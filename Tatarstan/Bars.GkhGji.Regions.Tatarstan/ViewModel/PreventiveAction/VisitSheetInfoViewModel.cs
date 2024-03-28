namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class VisitSheetInfoViewModel : BaseViewModel<VisitSheetInfo>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<VisitSheetInfo> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var result = domainService.GetAll()
                .Where(x => x.VisitSheet.Id == id);

            return new BaseDataResult(result);
        }
    }
}