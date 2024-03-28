namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationViolationViewModel : BaseViewModel<MotivatedPresentationViolation>
    {
        public IDataResult RealityObjectsListForMotivatedPresentationActionToWarningDocRule(IDomainService<MotivatedPresentationViolation> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.MotivatedPresentationRealityObject.MotivatedPresentation.Id == documentId)
                .GroupBy(x => new
                {
                    x.MotivatedPresentationRealityObject.RealityObject.Id,
                    x.MotivatedPresentationRealityObject.RealityObject.Address,
                    x.MotivatedPresentationRealityObject.RealityObject.Municipality.Name
                })
                .Select(x => new
                {
                    RealityObjectId = x.Key.Id,
                    x.Key.Address,
                    Municipality = x.Key.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}