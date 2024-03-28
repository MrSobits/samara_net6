namespace Bars.Gkh.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;

    /// <inheritdoc />
    public class JurInstitutionRealObjViewModel : BaseViewModel<JurInstitutionRealObj>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<JurInstitutionRealObj> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var jurInstId = loadParams.Filter.GetAs("jurInstId", 0L);

            var data = domain.GetAll()
                .Where(x => x.JurInstitution.Id == jurInstId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    Settlement = x.RealityObject.MoSettlement.Name,
                    x.RealityObject.FiasAddress.PlaceName,
                    x.RealityObject.FiasAddress.StreetName,
                    x.RealityObject.FiasAddress.House,
                    x.RealityObject.FiasAddress.Letter,
                    x.RealityObject.FiasAddress.Housing,
                    x.RealityObject.FiasAddress.Building
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}