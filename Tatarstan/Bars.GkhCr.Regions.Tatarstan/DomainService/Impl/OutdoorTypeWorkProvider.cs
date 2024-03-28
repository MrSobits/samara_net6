namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    using Castle.Windsor;

    public class OutdoorTypeWorkProvider : IOutdoorTypeWorkProvider
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<WorkRealityObjectOutdoor> GetWorks(long outdoorId, long periodId)
        {
            var typeWorkDomain = this.Container.Resolve<IDomainService<TypeWorkRealityObjectOutdoor>>();
            var workRealityObjectOutdoorkDomain = this.Container.Resolve<IDomainService<WorkRealityObjectOutdoor>>();

            using (this.Container.Using(typeWorkDomain, workRealityObjectOutdoorkDomain))
            {
                return workRealityObjectOutdoorkDomain.GetAll()
                    .Where(x => typeWorkDomain.GetAll()
                        .Where(z => z.ObjectOutdoorCr.RealityObjectOutdoor.Id == outdoorId && z.IsActive)
                        .Where(z => z.ObjectOutdoorCr.RealityObjectOutdoorProgram.Period.Id == periodId &&
                            z.ObjectOutdoorCr.RealityObjectOutdoorProgram.TypeVisibilityProgram == TypeVisibilityProgramCr.Full)
                        .Any(z => z.WorkRealityObjectOutdoor == x));
            }
        }
    }
}