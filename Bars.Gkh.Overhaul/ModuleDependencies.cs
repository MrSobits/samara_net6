namespace Bars.Gkh.Overhaul
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Castle.Windsor;
    using Entities;
    using Gkh.Entities;
    using Bars.B4.Utils;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container) : base(container)
        {
            References.Add(new EntityReference
            {
                ReferenceName = "Конструктивный элемент дома",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
                    service.GetAll().Where(x => x.RealityObject.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "RealityObjectMissingCeo",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<RealityObjectMissingCeo>>();
                    service.GetAll().Where(x => x.RealityObject.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Базовый класс Счет",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<BankAccount>>();
                    service.GetAll().Where(x => x.RealityObject.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Работы",
                BaseEntity = typeof(UnitMeasure),
                CheckAnyDependences = id => Container.Resolve<IDomainService<Job>>().GetAll().Any(x => x.UnitMeasure.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Работы",
                BaseEntity = typeof(Work),
                CheckAnyDependences = id => Container.Resolve<IDomainService<Job>>().GetAll().Any(x => x.Work.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Источники финансирования вида работ",
                BaseEntity = typeof(Work),
                CheckAnyDependences = id => Container.Resolve<IDomainService<WorkTypeFinSource>>().GetAll().Any(x => x.Work.Id == id)
            });

            var roSeIdHistoryDomain = Container.Resolve<IDomainService<RealObjStructElementIdHistory>>();
            References.Add(new EntityReference
            {
                ReferenceName = "История констуктивных элементов",
                BaseEntity = typeof(RealityObject),
                DeleteAnyDependences = id => roSeIdHistoryDomain
                    .GetAll()
                    .Where(x => x.RealityObject.Id == id)
                    .Select(x => x.Id).AsEnumerable()
                    .ForEach(x => roSeIdHistoryDomain.Delete(x))
            });

            References.Add(new EntityReference
            {
                ReferenceName = "ContragentBankCreditOrg",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = Container.Resolve<IDomainService<ContragentBankCreditOrg>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });
        }
    }
}