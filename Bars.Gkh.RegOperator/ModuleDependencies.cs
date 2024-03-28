namespace Bars.Gkh.RegOperator
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Реализация зависимостей сущностей
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
        /// Инициализировать
        /// </summary>
        /// <returns></returns>
        public override IModuleDependencies Init()
        {
            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Cчет невыясненных сумм",
                    BaseEntity = typeof(FinanceSourceResource),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<SuspenseAccountFinSourceResource>>().GetAll().Any(x => x.FinSourceResource.Id == id)
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Отложенный перевод статуса счета в \"Неактивно\"",
                    BaseEntity = typeof(GovDecision),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<DefferedUnactivation>>();
                        service.GetAll().Where(x => x.GovDecision.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Жилой дом записи перечисления средств рег. фонда",
                    BaseEntity = typeof(TransferRfRecord),
                    DeleteAnyDependences = id => 
                    {
                        var service = this.Container.Resolve<IDomainService<TransferObject>>();
                        service.GetAll().Where(x => x.TransferRecord.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "RegopCalcAccount",
                    BaseEntity = typeof(ContragentBankCreditOrg),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<RegopCalcAccount>>();
                        var list = service.GetAll().Where(x => x.ContragentCreditOrg.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Расчетные счета Рег оператора",
                    BaseEntity = typeof(ContragentBankCreditOrg),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<RegOpCalcAccount>>();
                        var list = service.GetAll().Where(x => x.ContragentBankCrOrg.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Расчетный счет",
                    BaseEntity = typeof(CreditOrg),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<CalcAccount>>();
                        var list = service.GetAll().Where(x => x.CreditOrg.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Специальный расчетный счет",
                    BaseEntity = typeof(CreditOrg),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<SpecialCalcAccount>>();
                        var list = service.GetAll().Where(x => x.CreditOrg.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "RegopCalcAccount",
                    BaseEntity = typeof(CreditOrg),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<RegopCalcAccount>>();
                        var list = service.GetAll().Where(x => x.CreditOrg.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Расчетные счета Рег оператора(устаревшее)",
                    BaseEntity = typeof(RealityObject),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<RegOpCalcAccount>>();
                        var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Условия обслуживания кредитными организациями",
                    BaseEntity = typeof(CreditOrg),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<CreditOrgServiceCondition>>();
                        var list = service.GetAll().Where(x => x.CreditOrg.Id == id).Select(x => x.Id).ToList();
                        foreach (var value in list)
                        {
                            service.Delete(value);
                        }
                    }
                });

            this.References.Add(
               new EntityReference
               {
                   ReferenceName = "Заявка на перечисление средств подрядчикам",
                   BaseEntity = typeof(ContragentBank),
                   DeleteAnyDependences = id =>
                   {
                       var service = this.Container.Resolve<IDomainService<TransferCtr>>();
                       service.GetAll().Where(x => x.ContragentBank.Id == id)
                           .Select(x => x.Id).ForEach(x => service.Delete(x));
                   }
               });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Связь лицевого счета с группой",
                    BaseEntity = typeof(PersAccGroup),
                    CheckAnyDependences = id => this.Container.ResolveDomain<PersAccGroupRelation>().GetAll().Any(x => x.Group.Id == id)
                });

            return this;
        }
    }
}
