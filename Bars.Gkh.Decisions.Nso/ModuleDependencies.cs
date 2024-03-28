namespace Bars.Gkh.Decisions.Nso
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
            References.Add(new EntityReference
            {
                ReferenceName = "Решение о выборе управления",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IDomainService<MkdManagementDecision>>();
                        var list = service.GetAll().Where(x => x.Decision.Id == id).Select(x => x.Id).ToArray();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Протокол решений собственников",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealityObjectDecisionProtocol>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "GovDecision",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<GovDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение о выборе кредитной организации",
                BaseEntity = typeof(CreditOrg),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<CreditOrgDecision>>();
                    var list = service.GetAll().Where(x => x.Decision.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение о выборе кредитной организации",
                BaseEntity = typeof(CreditOrg),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<CreditOrgDecisionNotification>>();
                    var list = service.GetAll().Where(x => x.CreditOrg.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });
        }
    }
}