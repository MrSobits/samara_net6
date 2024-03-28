namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Domain.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Entities;
    using Gkh.Utils;
    using GkhCr.Entities;
    using Overhaul.Domain.ProxyEntity;
    using Overhaul.Domain.RealityObjectServices;

    public class ObjectCrDpkrDataService : IObjectCrDpkrDataService
    {
        public ObjectCrDpkrDataService(
            IDomainService<RealityObjectStructuralElementInProgrammStage3> dpkrDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IWindsorContainer container)
        {
            _dpkrDomain = dpkrDomain;
            _objectCrDomain = objectCrDomain;
            _container = container;
        }

        public IQueryable<ObjectCrDpkrProxy> GetShortProgramRecordsProgramAndMunicipality(ProgramCr programCr = null, Municipality municipality = null)
        {
            var config = _container.GetGkhConfig<OverhaulHmaoConfig>();
            var leftEdgeYear = config.ProgrammPeriodStart;
            var shortPeriod = config.ShortTermProgPeriod;
            var rightEdgeYear = leftEdgeYear + shortPeriod;

            var roFilter =
                _objectCrDomain.GetAll()
                    .WhereIf(programCr != null, x => x.ProgramCr == programCr)
                    .WhereIf(municipality != null, x => x.RealityObject.Municipality == municipality);

            return
                _dpkrDomain.GetAll()
                    .WhereIf(programCr != null || municipality != null, x => roFilter.Any(r => r.RealityObject == x.RealityObject))
                    .Where(x => x.Year >= leftEdgeYear)
                    .Where(x => x.Year <= rightEdgeYear)
                    .Select(x => new ObjectCrDpkrProxy
                    {
                        RealityObject = x.RealityObject,
                        PlanYear = x.Year,
                        Sum = x.Sum,
                        CommonEstateObjectName = x.CommonEstateObjects
                    });
        }

        private readonly IDomainService<RealityObjectStructuralElementInProgrammStage3> _dpkrDomain;
        private readonly IDomainService<ObjectCr> _objectCrDomain;
        private readonly IWindsorContainer _container;
    }
}