namespace Bars.Gkh.Regions.Nso
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Nso.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container) : base(container)
        {
            this.References.Add(new EntityReference
            {
                ReferenceName = "Документы жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectDocument>>().GetAll().Count(x => x.RealityObject.Id == id) > 0
            });
        }
    }
}