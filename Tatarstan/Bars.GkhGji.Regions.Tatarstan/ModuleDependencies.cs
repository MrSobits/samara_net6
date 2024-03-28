namespace Bars.GkhGji.Regions.Tatarstan
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        /// <inheritdoc />
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        /// <inheritdoc />
        public override IModuleDependencies Init()
        {
            References.Add(new EntityReference
            {
                ReferenceName = "BaseTatarstanProtocolGji",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseTatProtocolGji>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            return this;
        }
    }
}
