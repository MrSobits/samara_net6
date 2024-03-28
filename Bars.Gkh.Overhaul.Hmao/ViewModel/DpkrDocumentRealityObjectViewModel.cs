namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// ViewModel for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public class DpkrDocumentRealityObjectViewModel : BaseViewModel<DpkrDocumentRealityObject>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<DpkrDocumentRealityObject> domainService, BaseParams baseParams)
        {
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
            var isExcluded = baseParams.Params.GetAs<bool?>("isExcluded");

            return domainService.GetAll()
                .Where(x => x.DpkrDocument.Id == dpkrDocumentId)
                .WhereIf(isExcluded.HasValue, x => x.IsExcluded == isExcluded)
                .Select(x => new
                {
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}