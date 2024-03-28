namespace Bars.Gkh.Overhaul.Nso.Domain.Impl
{
    using System.Linq;
    using B4.DataAccess;
    using Castle.Windsor;
    using Enums;
    using Gkh.Entities;
    using Overhaul.Domain;

    public class NsoDpkrRealityObjectService : IDpkrRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<RealityObject> GetObjectsInDpkr()
        {
            var emergencyDomain = Container.ResolveDomain<EmergencyObject>();

            return Container.ResolveRepository<RealityObject>().GetAll()
                .Where(x => x.TypeHouse != TypeHouse.NotSet)
                .Where(x => x.ConditionHouse == ConditionHouse.Serviceable
                            || x.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(x => !emergencyDomain.GetAll().Any(e => e.RealityObject.Id == x.Id)
                            || emergencyDomain.GetAll()
                                .Where(e => e.ConditionHouse == ConditionHouse.Serviceable
                                            || e.ConditionHouse == ConditionHouse.Dilapidated)
                                .Any(e => e.RealityObject.Id == x.Id))
                .Where(x => !x.IsNotInvolvedCr);
        }
    }
}
