namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Gkh.Entities;
    using Bars.GkhCr.DomainService;

    using Castle.Windsor;

    public class ProgramCrRealityObjectService : IProgramCrRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<RealityObject> GetObjectsInMainProgram()
        {
            var realityObjectDomain = Container.Resolve<IDomainService<RealityObject>>();
            var versionRecordDomain = Container.ResolveDomain<VersionRecord>();

            var realityObjects =
                realityObjectDomain.GetAll()
                .Where(x => versionRecordDomain.GetAll().Any(z => z.ProgramVersion.IsMain && z.RealityObject.Id == x.Id));

            return realityObjects;
        }
    }
}
