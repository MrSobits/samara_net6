namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class RealityObjectVidecamViewModel : BaseViewModel<RealityObjectVidecam>
    {
        public override IDataResult List(IDomainService<RealityObjectVidecam> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            return domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.UnicalNumber,
                    x.Workability,
                    x.InstallPlace
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}