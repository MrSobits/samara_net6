namespace Bars.GkhDi.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерцептор для коммунальных услуг
    /// </summary>
    public class CommunalServiceInterceptor : EmptyDomainInterceptor<CommunalService>
    {
        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<CommunalService> service, CommunalService entity)
        {
            if (entity.Provider != null)
            {
                var providerService = new ProviderService
                {
                    Id = 0,
                    BaseService = entity,
                    DateStartContract = DateTime.Now,
                    Provider = entity.Provider
                };

                this.Container.Resolve<IDomainService<ProviderService>>().Save(providerService);
            }

            return this.Success();
        }

        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<CommunalService> service, CommunalService entity)
        {
            var providerServiceRep = this.Container.Resolve<IDomainService<ProviderService>>();
            var costItemService = this.Container.Resolve<IDomainService<CostItem>>();
            var tariffForConsumersService = this.Container.Resolve<IDomainService<TariffForConsumers>>();
            var tariffForRsoService = this.Container.Resolve<IDomainService<TariffForRso>>();
            var percentService = this.Container.Resolve<IDomainService<ServicePercent>>();
            var infoAboutPaymentCommunalService = this.Container.Resolve<IDomainService<InfoAboutPaymentCommunal>>();
            var consumptionNormsNpaService = this.Container.Resolve<IDomainService<ConsumptionNormsNpa>>();
            var infoAboutReductionPaymentService = this.Container.Resolve<IDomainService<InfoAboutReductionPayment>>();
            var planReductionExpenseService = this.Container.Resolve<IDomainService<PlanReductionExpense>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => infoAboutReductionPaymentService.GetAll().Any(x => x.BaseService.Id == id) ? "Сведения о случаях снижения платы" : null,
                    id => planReductionExpenseService.GetAll().Any(x => x.BaseService.Id == id) ? "План мер по снижению расходов" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                }

                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return this.Failure(message);
                }

                
                var providerServiceIds = providerServiceRep.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in providerServiceIds)
                {
                    providerServiceRep.Delete(value);
                }

                
                var costItemIds = costItemService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in costItemIds)
                {
                    costItemService.Delete(value);
                }

               
                var tariffForConsumersIds = tariffForConsumersService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in tariffForConsumersIds)
                {
                    tariffForConsumersService.Delete(value);
                }

               
                var tariffForRsoIds = tariffForRsoService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in tariffForRsoIds)
                {
                    tariffForRsoService.Delete(value);
                }

                
                var servicePercentIds = percentService.GetAll().Where(x => x.Service.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in servicePercentIds)
                {
                    percentService.Delete(value);
                }

              
                var infoAboutPaymentCommunalIds = infoAboutPaymentCommunalService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in infoAboutPaymentCommunalIds)
                {
                    infoAboutPaymentCommunalService.Delete(value);
                }

               
                var consumptionNormsNpalIds = consumptionNormsNpaService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in consumptionNormsNpalIds)
                {
                    consumptionNormsNpaService.Delete(value);
                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(providerServiceRep);
                this.Container.Release(costItemService);
                this.Container.Release(tariffForConsumersService);
                this.Container.Release(tariffForRsoService);
                this.Container.Release(percentService);
                this.Container.Release(infoAboutPaymentCommunalService);
                this.Container.Release(consumptionNormsNpaService);
                this.Container.Release(infoAboutReductionPaymentService);
                this.Container.Release(planReductionExpenseService);
            }
        }
    }
}
