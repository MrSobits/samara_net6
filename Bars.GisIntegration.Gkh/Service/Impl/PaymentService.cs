namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.UI.Service;

    using Castle.Windsor;

    /// <summary>
    /// Сервис работы оплатами
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Вернуть список распоряжений, помеченных к удалению из ГИСа
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetNotificationsToDelete(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var dataSelector = this.Container
                .Resolve<IDataSelector<NotificationOfOrderExecution>>("NotificationOfOrderExecutionCancellationDataSelector");

            var accRelationDomain = this.Container.ResolveDomain<RisAccountRelations>();
            baseParams.Params.Add("Contragent", this.GetCurrentContragent());

            try
            {
                var data = dataSelector.GetExternalEntities(baseParams.Params)
                .Select(x => new
                {
                    x.Id,
                    x.OrderDate,
                    x.AccountNumber,
                    Address = accRelationDomain.GetAll().Where(y => y.Account.Id == x.RisPaymentDocument.Account.Id).Select(y => y.House.Adress).FirstOrDefault(),
                    x.OrderNum
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

                var count = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
            }
            finally
            {
                this.Container.Release(dataSelector);
                this.Container.Release(accRelationDomain);
            }
        }

        /// <summary>
        /// Получить список распоряжений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetNotificationsToAdd(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var extractor = this.Container.Resolve<IDataExtractor<NotificationOfOrderExecution>>("NotificationOfOrderExecutionExtractor");

            var accRelationDomain = this.Container.ResolveDomain<RisAccountRelations>();
            extractor.Contragent = this.GetCurrentContragent();

            try
            {
                var data = extractor.Extract(baseParams.Params)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.OrderDate,
                            x.AccountNumber,
                            Address =
                                accRelationDomain.GetAll()
                                    .Where(y => y.Account.Id == x.RisPaymentDocument.Account.Id)
                                    .Select(y => y.House.Adress)
                                    .FirstOrDefault(),
                            x.OrderNum
                        })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                var count = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
            }
            finally
            {
                this.Container.Release(extractor);
                this.Container.Release(accRelationDomain);
            }
        }

        private RisContragent GetCurrentContragent()
        {
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();

            try
            {
                return dataSupplierProvider.GetCurrentDataSupplier();
            }
            finally
            {
                this.Container.Release(dataSupplierProvider);
            }
        }
    }
}