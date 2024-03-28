namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectMeteringDeviceViewModel : BaseViewModel<RealityObjectMeteringDevice>
    {
        public override IDataResult List(IDomainService<RealityObjectMeteringDevice> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    MeteringDevice = x.MeteringDevice.Name,
                    x.MeteringDevice.AccuracyClass,
                    x.MeteringDevice.TypeAccounting,
                    x.DateRegistration,
                    x.DateInstallation,
                    x.PersonalAccountNum
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}