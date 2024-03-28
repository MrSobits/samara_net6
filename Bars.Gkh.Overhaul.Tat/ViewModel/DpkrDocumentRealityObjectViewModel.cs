namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    public class DpkrDocumentRealityObjectViewModel : BaseViewModel<DpkrDocumentRealityObject>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<DpkrDocumentRealityObject> domainService, BaseParams baseParams)
        {
            var isIncluded = baseParams.Params.GetAs<bool>("isIncluded");
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
            
            return domainService.GetAll()
                .Where(x=> x.DpkrDocument.Id == dpkrDocumentId && x.IsIncluded == isIncluded)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name,
                }).ToListDataResult(GetLoadParam(baseParams), this.Container);
        }
    }
}