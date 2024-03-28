namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class MeteringDeviceViewModel : BaseViewModel<NonResidentialPlacementMeteringDevice>
    {
        public override IDataResult List(IDomainService<NonResidentialPlacementMeteringDevice> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var nonResidentialPlacementId = baseParams.Params.GetAs<long>("nonResidentialPlacementId");

            var data = domainService
                .GetAll()
                .Where(x => x.NonResidentialPlacement.Id == nonResidentialPlacementId)
                .Select(x => new
                {
                    x.Id,
                    x.MeteringDevice.Name,
                    x.MeteringDevice.AccuracyClass,
                    x.MeteringDevice.TypeAccounting
                })
                .Filter(loadParams, this.Container);
            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);
            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}