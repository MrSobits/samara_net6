namespace Bars.Gkh.Gku
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Gku.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
            References.Add(new EntityReference
            {
                ReferenceName = "Тарифы ЖКХ",
                BaseEntity = typeof(SupplyResourceOrg),
                DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<GkuTariffGji>>();
                        var list = service.GetAll().Where(x => x.ResourceOrg.Id == id).Select(x => x.Id).ToArray();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Тарифы ЖКХ",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<GkuTariffGji>>();
                    var list = service.GetAll().Where(x => x.ManOrg.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });
        }
    }
}