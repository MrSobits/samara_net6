using Bars.B4.Utils;
using Bars.Gkh.Overhaul.DomainService;

namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.Windsor;
    using Gkh.Utils;
    using Overhaul.Entities;

    public class RealityObjectStructElementService : IRealityObjectStructElementService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<EmergencyObject> EmergencyDomainService { get; set; }
        public IQueryable<RealityObjectStructuralElement> GetElementsForLongProgram()
        {
            var domainService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            return GetElementsForLongProgram(domainService);
        }

        public IDomainService<EmergencyObject> EmergencyDomain { get; set; }

        public IQueryable<RealityObjectStructuralElement> GetElementsForLongProgram(IDomainService<RealityObjectStructuralElement> domainService)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var minCountApartments = config.HouseAddInProgramConfig.MinimumCountApartments;
            var usePhysicalWear = config.HouseAddInProgramConfig.UsePhysicalWearout;
            var physicalWear = config.HouseAddInProgramConfig.PhysicalWear;

            return domainService.GetAll()
                .Where(x => !EmergencyDomain.GetAll()
                        .Any(e => e.RealityObject.Id == x.RealityObject.Id) || EmergencyDomain.GetAll()
                            .Any(e => e.RealityObject.Id == x.RealityObject.Id
                                      && (e.ConditionHouse == ConditionHouse.Serviceable || e.ConditionHouse == ConditionHouse.Dilapidated)))
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding
                                 || x.RealityObject.TypeHouse == TypeHouse.ManyApartments
                                 || x.RealityObject.TypeHouse == TypeHouse.SocialBehavior)
                    .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated)
                    .Where(x => !x.RealityObject.IsNotInvolvedCr)
                    .WhereIf(minCountApartments > 0, x => x.RealityObject.NumberApartments >= minCountApartments)
                    .WhereIf(usePhysicalWear == TypeUsage.Used, x => !x.RealityObject.PhysicalWear.HasValue || x.RealityObject.PhysicalWear < physicalWear)
                    .Where(x => x.State.StartState);
        }
    }
}