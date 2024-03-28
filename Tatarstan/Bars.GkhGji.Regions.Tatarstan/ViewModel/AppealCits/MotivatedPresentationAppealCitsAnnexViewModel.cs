namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.AppealCits
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    /// <summary>
    /// View-модель для <see cref="MotivatedPresentationAppealCitsAnnex"/>
    /// </summary>
    public class MotivatedPresentationAppealCitsAnnexViewModel : BaseViewModel<MotivatedPresentationAppealCitsAnnex>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<MotivatedPresentationAppealCitsAnnex> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.MotivatedPresentation.Id == documentId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}