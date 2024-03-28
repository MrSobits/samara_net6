namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationAnnexViewModel : BaseViewModel<MotivatedPresentationAnnex>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<MotivatedPresentationAnnex> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.MotivatedPresentation.Id == documentId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}