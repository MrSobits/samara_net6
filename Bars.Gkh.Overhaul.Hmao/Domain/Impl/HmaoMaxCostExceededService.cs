namespace Bars.Gkh.Overhaul.Hmao.Domain.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;

    public class HmaoMaxCostExceededService : IMaxCostExceededService
    {
        private readonly IWindsorContainer _container;

        public HmaoMaxCostExceededService(IWindsorContainer container)
        {
            _container = container;
        }

        public IQueryable<MaxCostExeededRealty> GetAll()
        {
            var missingDomain = _container.Resolve<IDomainService<MissingByMargCostDpkrRec>>();
            using (_container.Using(missingDomain))
            {
                return missingDomain.GetAll().Select(x => new MaxCostExeededRealty
                {
                    Area = x.Area,
                    CommonEstateObjects = x.CommonEstateObjects,
                    MaxSum = x.MargSum,
                    RealEstateTypeName = x.RealEstateTypeName,
                    RealityObject = x.RealityObject,
                    Sum = x.Sum,
                    Year = x.Year
                });
            }
        }
    }
}
