namespace Bars.Gkh.ModuleDependencies
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    using Castle.Windsor;

    using NHibernate.Util;

    /// <summary>
    /// Зависимости модулей
    /// </summary>
    public class ModuleDependencies : BaseModuleDependencies
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Метод инициализации
        /// </summary>
        /// <returns>Интерфейс зависимости модуля</returns>
        public override IModuleDependencies Init()
        {
            // Operator
            this.References.Add(new EntityReference
            {
                ReferenceName = "Удаление невозможно, пока имеются активные связи с \"Операторы\"",
                BaseEntity = typeof(Role),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<UserRole>>().GetAll().Any(x => x.Role.Id == id)
            });

            this.References.Add(new EntityReference
            {
                BaseEntity = typeof(Role),
                ReferenceName = "Права доступа роли по статусам",
                DeleteAnyDependences = id =>
                {
                    var domain = this.Container.Resolve<IDomainService<StateRolePermission>>();
                    domain.GetAll().Where(x => x.Role.Id == id).Select(x => x.Id).ToArray().ForEach(x => domain.Delete(x));
                }
            });

            this.References.Add(new EntityReference
            {
                BaseEntity = typeof(Role),
                ReferenceName = "Переходы статусов",
                DeleteAnyDependences = id =>
                {
                    var domain = this.Container.Resolve<IDomainService<StateTransfer>>();
                    domain.GetAll().Where(x => x.Role.Id == id).Select(x => x.Id).ToArray().ForEach(x => domain.Delete(x));
                }
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Инспекторы оператора",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<OperatorInspector>>().GetAll().Any(x => x.Inspector.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Муниципальные образования оператора",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<OperatorMunicipality>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            // BelayManOrgActivity
            this.References.Add(new EntityReference
            {
                ReferenceName = "МКД, включенные в договор страхового полиса",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<BelayPolicyMkd>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            // Builder
            this.References.Add(new EntityReference
            {
                ReferenceName = "Документы подрядной организации",
                BaseEntity = typeof(Period),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<BuilderDocument>>().GetAll().Any(x => x.Period.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Сведения об участии в СРО подрядной организации",
                BaseEntity = typeof(Work),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<BuilderSroInfo>>().GetAll().Any(x => x.Work.Id == id)
            });

            // Contragent
            this.References.Add(new EntityReference
            {
                ReferenceName = "Контрагенты",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Contragent>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            #region Dict
            this.References.Add(new EntityReference
            {
                ReferenceName = "Виды оснащений",
                BaseEntity = typeof(UnitMeasure),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<KindEquipment>>().GetAll().Any(x => x.UnitMeasure.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Разрезы финансирования муниципального образования",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<MunicipalitySourceFinancing>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Программы переселения",
                BaseEntity = typeof(Period),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ResettlementProgram>>().GetAll().Any(x => x.Period.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Виды работ",
                BaseEntity = typeof(UnitMeasure),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Work>>().GetAll().Any(x => x.UnitMeasure.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Виды работ текущего ремонта",
                BaseEntity = typeof(UnitMeasure),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<WorkKindCurrentRepair>>().GetAll().Any(x => x.UnitMeasure.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Инспекторы зональной жилищной инспекции",
                BaseEntity = typeof(ZonalInspection),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll().Any(x => x.ZonalInspection.Id == id)
            });
            this.References.Add(new EntityReference
            {
                ReferenceName = "Инспекторы зональной жилищной инспекции",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll().Any(x => x.Inspector.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Муниципальные образования зональной жилищной инспекции",
                BaseEntity = typeof(ZonalInspection),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll().Any(x => x.ZonalInspection.Id == id)
            });
            this.References.Add(new EntityReference
            {
                ReferenceName = "Муниципальные образования зональной жилищной инспекции",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll().Any(x => x.Municipality.Id == id)
            });
            #endregion Dict

            // EmergencyObject
            this.References.Add(new EntityReference
            {
                ReferenceName = "Реестр аварийных домов",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<EmergencyObject>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Реестр аварийных домов",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<EmergencyObject>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Документы аварийного дома",
                BaseEntity = typeof(EmergencyObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<EmergencyObjectDocuments>>().GetAll().Any(x => x.EmergencyObject.Id == id)
            });

            // LocalGovernment
            this.References.Add(new EntityReference
            {
                ReferenceName = "Муниципальные образования органа местного самоуправления",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<LocalGovernmentMunicipality>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            // PoliticAuthority
            this.References.Add(new EntityReference
            {
                ReferenceName = "Муниципальные образования органа государственной власти",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<PoliticAuthorityMunicipality>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            // ManagingOrganization
            this.References.Add(new EntityReference
            {
                ReferenceName = "Жилые дома управляющей организации",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ManagingOrgRealityObject>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Жилые дома управления дома УО",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            #region RealityObject

            this.References.Add(new EntityReference
            {
                ReferenceName = "Реестр жилых домов",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Сведения о квартирах жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectApartInfo>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Конструктивные элементы жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectConstructiveElement>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Члены совета МКД жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectCouncillors>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Текущий ремонт жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectCurentRepair>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Управление жилого дома",
                BaseEntity = typeof(ManagingOrganization),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectDirectManagContract>>().GetAll().Any(x => x.ManagingOrganization.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Сведения о помещениях жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectHouseInfo>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Сведения о помещениях жилого дома",
                BaseEntity = typeof(UnitMeasure),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectHouseInfo>>().GetAll().Any(x => x.UnitMeasure.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Фото-архив жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectImage>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Фото-архив жилого дома",
                BaseEntity = typeof(Period),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectImage>>().GetAll().Any(x => x.Period.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Фото-архив жилого дома",
                BaseEntity = typeof(Work),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectImage>>().GetAll().Any(x => x.WorkCr.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Земельные участки жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectLand>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Приборы учета и узлы регулирования жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectMeteringDevice>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Протокол собрания жильцов жилого дома",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealityObjectProtocol>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToList();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Поставщики услуг жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<RealityObjectServiceOrg>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            #endregion RealityObject

            // InspectorSubscription
            this.References.Add(new EntityReference
            {
                ReferenceName = "Подписки на инспектора",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<InspectorSubscription>>().GetAll().Any(x => x.SignedInspector.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Подписки на инспектора",
                BaseEntity = typeof(Inspector),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<InspectorSubscription>>();
                    var list = service.GetAll().Where(x => x.Inspector.Id == id).Select(x => x.Id).ToList();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Настройки ограничений по статусам",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<StateRolePermission>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Сведения о помещениях",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Room>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            #region Договор поставщика ресурсов с домом
            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Договор поставщика ресурсов с домом (Услуга по договору поставщика ресурсов с жилым домом)",
                    BaseEntity = typeof(PublicServiceOrgContract),
                    DeleteAnyDependences = id =>
                    {
                        var domain = this.Container.ResolveDomain<PublicServiceOrgContractService>();
                        using (this.Container.Using(domain))
                        {
                            domain.GetAll().Where(x => x.ResOrgContract.Id == id).ForEach(x => domain.Delete(x.Id));
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Услуга по договору поставщика ресурсов с жилым домом (показатели качества)",
                    BaseEntity = typeof(PublicServiceOrgContractService),
                    DeleteAnyDependences = id =>
                    {
                        var domain = this.Container.ResolveDomain<PublicOrgServiceQualityLevel>();
                        using (this.Container.Using(domain))
                        {
                            domain.GetAll().Where(x => x.ServiceOrg.Id == id).ForEach(x => domain.Delete(x.Id));
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Договор поставщика ресурсов с домом (сторона договора)",
                    BaseEntity = typeof(PublicServiceOrgContract),
                    DeleteAnyDependences = id =>
                    {
                        var domain = this.Container.ResolveDomain<BaseContractPart>();
                        using (this.Container.Using(domain))
                        {
                            domain.GetAll().Where(x => x.PublicServiceOrgContract.Id == id).ForEach(x => domain.Delete(x.Id));
                        }
                    }
                });

            this.References.Add(
               new EntityReference
               {
                   ReferenceName = "Договор поставщика ресурсов с домом (информация о температурном графике)",
                   BaseEntity = typeof(PublicServiceOrgContract),
                   DeleteAnyDependences = id =>
                   {
                       var domain = this.Container.ResolveDomain<PublicServiceOrgTemperatureInfo>();
                       using (this.Container.Using(domain))
                       {
                           domain.GetAll().Where(x => x.Contract.Id == id).ForEach(x => domain.Delete(x.Id));
                       }
                   }
               });
            #endregion

            return this;
        }
    }
}
