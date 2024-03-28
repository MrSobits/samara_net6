namespace Bars.GkhDi.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class CapRepairServiceInterceptor : EmptyDomainInterceptor<CapRepairService>
    {
        public override IDataResult AfterCreateAction(IDomainService<CapRepairService> service, CapRepairService entity)
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

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<CapRepairService> service, CapRepairService entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                   id => this.Container.Resolve<IDomainService<InfoAboutReductionPayment>>().GetAll().Any(x => x.BaseService.Id == id) ? "Сведения о случаях снижения платы" : null,
                                   id => this.Container.Resolve<IDomainService<PlanReductionExpense>>().GetAll().Any(x => x.BaseService.Id == id) ? "План мер по снижению расходов" : null
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
                return Failure(message);
            }

            var providerServiceRep = Container.Resolve<IDomainService<ProviderService>>();
            var providerServiceIds = providerServiceRep.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in providerServiceIds)
            {
                providerServiceRep.Delete(value);
            }

            var costItemService = Container.Resolve<IDomainService<CostItem>>();
            var costItemIds = costItemService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in costItemIds)
            {
                costItemService.Delete(value);
            }

            var tariffForConsumersService = Container.Resolve<IDomainService<TariffForConsumers>>();
            var tariffForConsumersIds = tariffForConsumersService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in tariffForConsumersIds)
            {
                tariffForConsumersService.Delete(value);
            }

            var tariffForRsoService = Container.Resolve<IDomainService<TariffForRso>>();
            var tariffForRsoIds = tariffForRsoService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in tariffForRsoIds)
            {
                tariffForRsoService.Delete(value);
            }

            var percentService = Container.Resolve<IDomainService<ServicePercent>>();
            var servicePercentIds = percentService.GetAll().Where(x => x.Service.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in servicePercentIds)
            {
                percentService.Delete(value);
            }

            var infoAboutPaymentHousingService = Container.Resolve<IDomainService<InfoAboutPaymentHousing>>();
            var infoAboutPaymentHousingIds = infoAboutPaymentHousingService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in infoAboutPaymentHousingIds)
            {
                infoAboutPaymentHousingService.Delete(value);
            }

            var workCapRepairService = Container.Resolve<IDomainService<WorkCapRepair>>();
            var workCapRepairIds = workCapRepairService.GetAll().Where(x => x.BaseService.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in workCapRepairIds)
            {
                workCapRepairService.Delete(value);
            }

            return this.Success();
        }
    }
}
