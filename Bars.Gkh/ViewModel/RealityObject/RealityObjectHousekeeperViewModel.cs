namespace Bars.Gkh.ViewModel.RealityObject
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class RealityObjectHousekeeperViewModel : BaseViewModel<RealityObjectHousekeeper>
    {
        public override IDataResult List(IDomainService<RealityObjectHousekeeper> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectId = baseParams.Params.GetAs<long>("objectId");

            return domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.IsActive,
                    x.FIO,
                    x.Login,
                    x.PhoneNumber
                }).Filter(loadParams, Container)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}