namespace Bars.GkhDi
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        public override IModuleDependencies Init()
        {
            #region Услуги

            References.Add(
                new EntityReference
                {
                    ReferenceName = "Базовая услуга",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<BaseService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "AdditionalService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<AdditionalService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "CapRepairService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<CapRepairService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "CommunalService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<CommunalService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "CommunalService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<CommunalService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "ControlService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<ControlService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "HousingService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<HousingService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "RepairService",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<RepairService>>();
                        var list = service.GetAll().Where(x => x.Provider.Id == id);
                        foreach (var value in list)
                        {
                            value.Provider = null;
                            service.Update(value);
                        }
                    }
                });

            #endregion

            References.Add(new EntityReference
            {
                ReferenceName = "Поставщики услуги",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<ProviderService>>();
                    service.GetAll().Where(x => x.Provider.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Раскрытие информации по управляющей организации",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<DisclosureInfo>>();
                    service.GetAll().Where(x => x.ManagingOrganization.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Аудиторские проверки фин деятельности",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<FinActivityAudit>>();
                    service.GetAll().Where(x => x.ManagingOrganization.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });
            References.Add(new EntityReference
            {
                ReferenceName = "Документы по годам фин деятельности",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<FinActivityDocByYear>>();
                    service.GetAll().Where(x => x.ManagingOrganization.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Управление домом фин деятельности",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<FinActivityManagRealityObj>>();
                    service.GetAll().Where(x => x.RealityObject.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Дом в раскрытии информации",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<FinActivityManagRealityObj>>();
                    service.GetAll().Where(x => x.RealityObject.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Дом в раскрытии информации",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
                    service.GetAll().Where(x => x.RealityObject.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Виды работ",
                BaseEntity = typeof(UnitMeasure),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<TemplateService>>().GetAll().Any(x => x.UnitMeasure.Id == id)
            });

            return this;
        }
    }
}
