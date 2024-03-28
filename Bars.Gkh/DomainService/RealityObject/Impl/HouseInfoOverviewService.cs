namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using Entities.Hcs;

    public class HouseInfoOverviewService : IHouseInfoOverviewService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetHouseInfoOverviewByRealityObjectId(BaseParams baseParams)
        {
            var realtyObjectId = baseParams.Params["realtyObjectId"].ToInt();

            var id = this.Container.Resolve<IDomainService<HouseInfoOverview>>()
                .GetAll()
                .Where(x => x.RealityObject.Id == realtyObjectId)
                .Select(x => x.Id)
                .FirstOrDefault();
            
            return new BaseDataResult(new { houseInfoOverviewId = id })
            {
                Success = true
            };
        }
    }
}