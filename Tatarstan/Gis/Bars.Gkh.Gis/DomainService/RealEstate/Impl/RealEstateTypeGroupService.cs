namespace Bars.Gkh.Gis.DomainService.RealEstate.Impl
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities.RealEstate.GisRealEstateType;

    public class RealEstateTypeGroupService : IRealEstateTypeGroupService
    {
        protected IWindsorContainer Container;
        protected IRepository<GisRealEstateTypeGroup> RealEstateTypeGroupRepository;

        public RealEstateTypeGroupService(IWindsorContainer container, IRepository<GisRealEstateTypeGroup> realEstateTypeGroupRepository)
        {
            Container = container;
            RealEstateTypeGroupRepository = realEstateTypeGroupRepository;
        }

        /// <summary>
        /// Список групп без пейджинга
        /// </summary>
        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var data = RealEstateTypeGroupRepository
                .GetAll();

            return new ListDataResult(data, data.Count());
        }
    }
}