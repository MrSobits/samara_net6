namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Regions.Tatarstan.DomainService;

    using Castle.Windsor;

    public class RealObjectStructElementService : IRealObjectStructElementService
    {
        public IWindsorContainer Container { get; set; }

        public IDictionary<long, decimal> GetRealityObjectWearoutDictionary(IQueryable<RealityObject> roQuery, string code)
        {
            var domainService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            try
            {
                return domainService.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Where(x => x.StructuralElement.Group.CommonEstateObject.Code == code)
                    .Select(
                        x => new
                        {
                            x.RealityObject.Id,
                            x.Wearout
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First().Wearout);
            }
            finally
            {
                this.Container.Release(domainService);
            }
            
        }
    }
}