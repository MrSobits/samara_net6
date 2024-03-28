namespace Bars.GkhRf
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        public override IModuleDependencies Init()
        {
            References.Add(
                        new EntityReference
                        {
                            ReferenceName = "Реестр договоров с управляющей организацией",
                            BaseEntity = typeof(ManagingOrganization),
                            DeleteAnyDependences = id =>
                            {
                                var service = this.Container.Resolve<IDomainService<ContractRf>>();
                                var itemCollection = service
                                        .GetAll()
                                        .Where(x => x.ManagingOrganization.Id == id)
                                        .Select(x => x.Id)
                                        .ToArray();

                                foreach (var item in itemCollection)
                                {
                                    service.Delete(item);
                                }
                            }
                        });

            References.Add(
                        new EntityReference
                            {
                                ReferenceName = "Дом в реестре договоров с управляющими организациями",
                                BaseEntity = typeof(RealityObject),
                                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll().Any(x => x.RealityObject.Id == id)
                            });

            References.Add(
                           new EntityReference
                           {
                               ReferenceName = "Заявка на перечисление денежных средств",
                               BaseEntity = typeof(ProgramCr),
                               CheckAnyDependences = id => this.Container.Resolve<IDomainService<RequestTransferRf>>().GetAll().Any(x => x.ProgramCr.Id == id)
                           });

            References.Add(
                           new EntityReference
                           {
                               ReferenceName = "Заявка на перечисление денежных средств",
                               BaseEntity = typeof(ContragentBank),
                               DeleteAnyDependences = id =>
                               {
                                   var service = Container.Resolve<IDomainService<RequestTransferRf>>();
                                   service.GetAll().Where(x => x.ContragentBank.Id == id)
                                       .Select(x => x.Id).ForEach(x => service.Delete(x));
                               }
                           });

            References.Add(
                           new EntityReference
                           {
                               ReferenceName = "Заявка на перечисление денежных средств",
                               BaseEntity = typeof(ManagingOrganization),
                               DeleteAnyDependences = id =>
                               {
                                   var service = this.Container.Resolve<IDomainService<RequestTransferRf>>();
                                   var itemCollection = service
                                           .GetAll()
                                           .Where(x => x.ManagingOrganization.Id == id)
                                           .Select(x => x.Id)
                                           .ToArray();

                                   foreach (var item in itemCollection)
                                   {
                                       service.Delete(item);
                                   }
                               }
                           });

            References.Add(
                          new EntityReference
                          {
                              ReferenceName = "Заявка на перечисление денежных средств",
                              BaseEntity = typeof(State),
                              CheckAnyDependences = id => this.Container.Resolve<IDomainService<RequestTransferRf>>().GetAll().Any(x => x.State.Id == id)
                          });

            References.Add(
                           new EntityReference
                           {
                               ReferenceName = "Дома, включенные в заявку",
                               BaseEntity = typeof(RealityObject),
                               CheckAnyDependences = id => this.Container.Resolve<IDomainService<TransferFundsRf>>().GetAll().Any(x => x.RealityObject.Id == id)
                           });

            References.Add(
                            new EntityReference
                            {
                                ReferenceName = "Объект записи перечисления средств рег. фонда",
                                BaseEntity = typeof(RealityObject),
                                CheckAnyDependences = id => this.Container.Resolve<IDomainService<TransferRfRecObj>>().GetAll().Any(x => x.RealityObject.Id == id)
                            });

            References.Add(
                               new EntityReference
                               {
                                   ReferenceName = "Сведения о перечислениях по договору",
                                   BaseEntity = typeof(State),
                                   CheckAnyDependences = id => this.Container.Resolve<IDomainService<TransferRfRecord>>().GetAll().Any(x => x.State.Id == id)
                               });

            References.Add(
                             new EntityReference
                             {
                                 ReferenceName = "Реестр оплат капитального ремонта",
                                 BaseEntity = typeof(RealityObject),
                                 CheckAnyDependences = id => this.Container.Resolve<IDomainService<Payment>>().GetAll().Any(x => x.RealityObject.Id == id)
                             });

            References.Add(
                             new EntityReference
                             {
                                 ReferenceName = "Оплата капитального ремонта",
                                 BaseEntity = typeof(ManagingOrganization),
                                 DeleteAnyDependences = id =>
                                 {
                                     var service = this.Container.Resolve<IDomainService<PaymentItem>>();
                                     var itemCollection = service
                                             .GetAll()
                                             .Where(x => x.ManagingOrganization.Id == id)
                                             .Select(x => x.Id)
                                             .ToArray();

                                     foreach (var item in itemCollection)
                                     {
                                         service.Delete(item);
                                     }
                                 }
                             });

            References.Add(
                             new EntityReference
                             {
                                 ReferenceName = "Разрезы финансирования настройки проверки на наличие лимитов",
                                 BaseEntity = typeof(FinanceSource),
                                 CheckAnyDependences = id => this.Container.Resolve<IDomainService<LimitCheckFinSource>>().GetAll().Any(x => x.FinanceSource.Id == id)
                             });

            return this;
        }
    }
}
