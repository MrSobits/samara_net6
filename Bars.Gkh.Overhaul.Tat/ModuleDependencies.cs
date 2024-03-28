namespace Bars.Gkh.Overhaul.Tat
{
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;
    using Gkh.Entities.RealEstateType;
    using Overhaul.Entities;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        public override IModuleDependencies Init()
        {
            References.Add(new EntityReference
            {
                ReferenceName = "Базовая сущность решения собственников помещений МКД",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<BasePropertyOwnerDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Банковская выписка",
                BaseEntity = typeof(BankAccount),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<AccBankStatement>>();
                    var list = service.GetAll().Where(x => x.BankAccount.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Счет начислений",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<AccrualsAccount>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "PaymentAccount",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<PaymentAccount>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "RealAccount",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealAccount>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Специальный счет",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccount>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (Перечень услуг и(или) работ по Капитальному ремонту)",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<ListServicesDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (Установление минимального размера фонда кап.ремонта)",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<MinAmountDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (Минимальный размер фонда КР)",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<MinFundSizeDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (при формирования фонда КР на счете Рег.оператора)",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RegOpAccountDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (при формирования фонда КР на спец.счете)",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccountDecision>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Протоколы собственников помещений МКД",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<PropertyOwnerProtocols>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Связка типа дома - дом",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealEstateTypeRealityObject>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Сущность, содержащая данные, необходимые при учете корректировки ДПКР",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "DpkrGroupedYear",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<DpkrGroupedYear>>();
                    var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Протокол Краткосрочной программы",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<ShortProgramProtocol>>();
                    var list = service.GetAll().Where(x => x.Contragent.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "RealAccount",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealAccount>>();
                    var list = service.GetAll().Where(x => x.AccountOwner.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "PaymentAccount",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<PaymentAccount>>();
                    var list = service.GetAll().Where(x => x.AccountOwner.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Специальный счет",
                BaseEntity = typeof(CreditOrg),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccount>>();
                    var list = service.GetAll().Where(x => x.CreditOrganization.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Специальный счет",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccount>>();
                    var list = service.GetAll().Where(x => x.AccountOwner.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (при формирования фонда КР на спец.счете)",
                BaseEntity = typeof(CreditOrg),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccountDecision>>();
                    var list = service.GetAll().Where(x => x.CreditOrg.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (при формирования фонда КР на спец.счете)",
                BaseEntity = typeof(ManagingOrganization),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccountDecision>>();
                    var list = service.GetAll().Where(x => x.ManagingOrganization.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (при формирования фонда КР на счете Рег.оператора)",
                BaseEntity = typeof(RegOperator),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RegOpAccountDecision>>();
                    var list = service.GetAll().Where(x => x.RegOperator.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решение собственников помещений МКД (при формирования фонда КР на спец.счете)",
                BaseEntity = typeof(RegOperator),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<SpecialAccountDecision>>();
                    var list = service.GetAll().Where(x => x.RegOperator.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Муниципальное образование регионального оператора",
                BaseEntity = typeof(RegOperator),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RegOperatorMunicipality>>();
                    var list = service.GetAll().Where(x => x.RegOperator.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Существует зависимые записи в разделе \"Конструктивные характеристики\" жилого дома",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            var typeWorkCrVersionDomain = Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();
            References.Add(new EntityReference
            {
                BaseEntity = typeof(TypeWorkCr),
                DeleteAnyDependences = id => typeWorkCrVersionDomain.GetAll()
                    .Where(x => x.TypeWorkCr.Id == id)
                    .Select(x => x.Id).AsEnumerable()
                    .ForEach(x => typeWorkCrVersionDomain.Delete(x))
            });

            var realEstateTypeRealityObjectDomain = Container.Resolve<IDomainService<RealEstateTypeRealityObject>>();
            References.Add(new EntityReference
            {
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id => realEstateTypeRealityObjectDomain.GetAll()
                    .Where(x => x.RealityObject.Id == id)
                    .Select(x => x.Id).AsEnumerable()
                    .ForEach(x => realEstateTypeRealityObjectDomain.Delete(x))
            });

            return this;
        }
    }
}