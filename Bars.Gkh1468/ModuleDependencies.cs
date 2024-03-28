namespace Bars.Gkh1468
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        public override IModuleDependencies Init()
        {
            #region MetaAttribute

            References.Add(new EntityReference
            {
                ReferenceName = "Реестр паспортов. Раздел структуры",
                BaseEntity = typeof(PassportStruct),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Part>>()
                        .GetAll()
                        .Any(x => x.Struct.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Реестр паспортов. Паспорт дома",
                BaseEntity = typeof(PassportStruct),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<BaseProviderPassport>>()
                        .GetAll()
                        .Any(x => x.PassportStruct.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Паспорт жилого дома",
                BaseEntity = typeof(MetaAttribute),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<HouseProviderPassportRow>>()
                        .GetAll()
                        .Any(x => x.MetaAttribute.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Паспорт ОКИ",
                BaseEntity = typeof(MetaAttribute),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<OkiProviderPassportRow>>()
                    .GetAll()
                    .Any(x => x.MetaAttribute.Id == id)
            });

            References.Add(new EntityReference
                {
                    ReferenceName = "Существуют связанные записи в следующих таблицах: Атрибут структуры паспорта",
                    BaseEntity = typeof(UnitMeasure),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<MetaAttribute>>()
                        .GetAll()
                        .Any(x => x.UnitMeasure.Id == id)
                });

            #endregion MetaAttribute

            #region Contragent

            References.Add(new EntityReference
            {
                ReferenceName = "Паспорт ОКИ",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<OkiProviderPassport>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Паспорт дома (1468)",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<HouseProviderPassport>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Поставщик ресурсов",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<PublicServiceOrg>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            #endregion Contragent

            return this;
        }
    }
}
