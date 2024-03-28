using Bars.B4.Utils;
using Bars.Gkh.Overhaul.DomainService;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

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

        public IQueryable<RealityObjectStructuralElement> GetElementsForLongProgram(IDomainService<RealityObjectStructuralElement> domainService)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();

            var minCountApartments = config.HouseAddInProgramConfig.MinimumCountApartments;
            var usePhysicalWear = config.HouseAddInProgramConfig.UsePhysicalWearout;
            var physicalWear = config.HouseAddInProgramConfig.PhysicalWear;
            var useManyApartments = config.HouseTypesConfig.UseManyApartments;
            var useBlockedBuilding = config.HouseTypesConfig.UseBlockedBuilding;
            var useIndividual = config.HouseTypesConfig.UseIndividual;
            var useSocialBehavior = config.HouseTypesConfig.UseSocialBehavior;

            return domainService.GetAll()
                .Where(y =>
                    //не берем дома, которые есть в реестре аварийных объектов
                    !EmergencyDomainService.GetAll()
                        .Any(x => x.RealityObject.Id == y.RealityObject.Id)
                    // либо они есть и у них состояние ветхий || исправный
                    || EmergencyDomainService.GetAll()
                        .Where(x => x.ConditionHouse == ConditionHouse.Serviceable
                            || x.ConditionHouse == ConditionHouse.Dilapidated)
                        .Any(x => x.RealityObject.Id == y.RealityObject.Id))
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable
                    || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.NotSet)
                .Where(x => !x.RealityObject.IsNotInvolvedCr)
                .Where(x => x.StructuralElement.Group.CommonEstateObject.IncludedInSubjectProgramm)
                .Where(x => x.StructuralElement.Group.UseInCalc)
                .Where(x => x.State.StartState)
                .WhereIf(minCountApartments > 0, x => x.RealityObject.NumberApartments >= minCountApartments)
                .WhereIf(usePhysicalWear == TypeUsage.Used, x => !x.RealityObject.PhysicalWear.HasValue || x.RealityObject.PhysicalWear < physicalWear)
                .WhereIf(!useManyApartments, x => x.RealityObject.TypeHouse != TypeHouse.ManyApartments)
                .WhereIf(!useBlockedBuilding, x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding)
                .WhereIf(!useIndividual, x => x.RealityObject.TypeHouse != TypeHouse.Individual)
                .WhereIf(!useSocialBehavior, x => x.RealityObject.TypeHouse != TypeHouse.SocialBehavior);
        }
    }
}