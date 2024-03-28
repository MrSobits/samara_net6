namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskRegulationViewModel : BaseViewModel<PreventiveActionTaskRegulation>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PreventiveActionTaskRegulation> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            
            return domainService.GetAll()
                .Where(x => x.Task.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    NormativeDoc = x.NormativeDoc.Name,
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}