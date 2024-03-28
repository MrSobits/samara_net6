namespace Bars.GkhCr
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        public override IModuleDependencies Init()
        {
            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Банковские выписки",
                    BaseEntity = typeof(ManagingOrganization),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<BankStatement>>();
                        var list = service.GetAll().Where(x => x.ManagingOrganization.Id == id);
                        foreach (var value in list)
                        {
                            value.ManagingOrganization = null;
                            service.Update(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Банковские выписки",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IRepository<BankStatement>>();
                        var list = service.GetAll().Where(x => x.Contragent.Id == id);
                        foreach (var value in list)
                        {
                            value.Contragent = null;
                            service.Update(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Договор КР",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IRepository<ContractCr>>();
                        var list = service.GetAll().Where(x => x.Contragent.Id == id);
                        foreach (var value in list)
                        {
                            value.Contragent = null;
                            service.Update(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Документ работы объекта КР",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IRepository<DocumentWorkCr>>();
                        var list = service.GetAll().Where(x => x.Contragent.Id == id);
                        foreach (var value in list)
                        {
                            value.Contragent = null;
                            service.Update(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Протокол(акт)",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IRepository<ProtocolCr>>();
                        var list = service.GetAll().Where(x => x.Contragent.Id == id);
                        foreach (var value in list)
                        {
                            value.Contragent = null;
                            service.Update(value);
                        }
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Cущность платежного поручения приход",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<PaymentOrderIn>>();
                        service.GetAll().Where(x => x.PayerContragent.Id == id || x.ReceiverContragent.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Cущность платежного поручения расход",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<PaymentOrderOut>>();
                        service.GetAll().Where(x => x.PayerContragent.Id == id || x.ReceiverContragent.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Платежные поручения",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<BasePaymentOrder>>();
                        service.GetAll().Where(x => x.PayerContragent.Id == id || x.ReceiverContragent.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Банковские выписки",
                    BaseEntity = typeof(Period),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<BankStatement>>().GetAll().Any(x => x.Period.Id == id)
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Виды работ источников финансирования",
                    BaseEntity = typeof(Work),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<FinanceSourceWork>>().GetAll().Any(x => x.Work.Id == id)
                });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Виды работ источников финансирования",
                    BaseEntity = typeof(FinanceSource),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<FinanceSourceWork>>().GetAll().Any(x => x.FinanceSource.Id == id)
                });

            this.References.Add(
              new EntityReference
              {
                  ReferenceName = "Программы КР",
                  BaseEntity = typeof(Period),
                  CheckAnyDependences = id => this.Container.Resolve<IDomainService<ProgramCr>>().GetAll().Any(x => x.Period.Id == id)
              });

            this.References.Add(
               new EntityReference
               {
                   ReferenceName = "Разрезы финансирования по программе КР",
                   BaseEntity = typeof(ProgramCr),
                   CheckAnyDependences = id => this.Container.Resolve<IDomainService<ProgramCrFinSource>>().GetAll().Any(x => x.ProgramCr.Id == id)
               });

            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Разрезы финансирования по программе КР",
                    BaseEntity = typeof(FinanceSource),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<ProgramCrFinSource>>().GetAll().Any(x => x.FinanceSource.Id == id)
                });

            this.References.Add(
               new EntityReference
               {
                   ReferenceName = "Квалификационный отбор",
                   BaseEntity = typeof(Period),
                   CheckAnyDependences = id => this.Container.Resolve<IDomainService<QualificationMember>>().GetAll().Any(x => x.Period.Id == id)
               });

            this.References.Add(
             new EntityReference
             {
                 ReferenceName = "Cметный расчет по работе",
                 BaseEntity = typeof(State),
                 CheckAnyDependences = id => this.Container.Resolve<IDomainService<EstimateCalculation>>().GetAll().Any(x => x.State.Id == id)
             });

            this.References.Add(
            new EntityReference
            {
                ReferenceName = "Акт выполненных работ",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Участники квалификационного отбора",
                BaseEntity = typeof(Builder),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<Qualification>>();
                    var list = service.GetAll().Where(x => x.Builder.Id == id).Select(x => x.Id).ToArray();
                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Договор подряда КР",
                BaseEntity = typeof(Builder),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BuildContract>>();
                    var list = service.GetAll().Where(x => x.Builder.Id == id).Select(x => x.Id).ToArray();

                    foreach (var value in list)
                    {
                        service.Delete(value);
                    }
                }
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Договоры подряда объекта КР",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<BuildContract>>().GetAll().Any(x => x.Inspector.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Договоры подряда объекта КР",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<BuildContract>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Договоры объекта КР",
                BaseEntity = typeof(FinanceSource),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ContractCr>>().GetAll().Any(x => x.FinanceSource.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Договоры объекта КР",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ContractCr>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Дефектная ведомость объекта КР",
                BaseEntity = typeof(Work),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<DefectList>>().GetAll().Any(x => x.Work.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Дефектная ведомость объекта КР",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<DefectList>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Средства источника финансирования объекта КР",
                BaseEntity = typeof(FinanceSource),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll().Any(x => x.FinanceSource.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Мониторинг СМР",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<MonitoringSmr>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Объекты КР",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ObjectCr>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Объекты КР",
                BaseEntity = typeof(ProgramCr),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ObjectCr>>().GetAll().Any(x => x.ProgramCr.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Объекты КР",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ObjectCr>>().GetAll().Any(x => x.State.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Виды работ объекта КР",
                BaseEntity = typeof(FinanceSource),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll().Any(x => x.FinanceSource.Id == id)
            });
            this.References.Add(new EntityReference
            {
                ReferenceName = "Виды работ объекта КР",
                BaseEntity = typeof(Work),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll().Any(x => x.Work.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Контрольные сроки",
                BaseEntity = typeof(ProgramCr),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ControlDate>>().GetAll().Any(x => x.ProgramCr.Id == id)
            });

            this.References.Add(new EntityReference
            {
                ReferenceName = "Виды работ",
                BaseEntity = typeof(DesignAssignment),
                DeleteAnyDependences = id =>
                    {
                        var domain = this.Container.Resolve<IDomainService<DesignAssignmentTypeWorkCr>>();

                        using (this.Container.Using(domain))
                        {
                            var records = this.Container.Resolve<IDomainService<DesignAssignmentTypeWorkCr>>().GetAll()
                                .Where(x => x.DesignAssignment.Id == id)
                                .Select(x => x.Id)
                                .ToList();

                            records.ForEach(x => domain.Delete(x));
                        }
                    }
            });

            return this;
        }
    }
}
