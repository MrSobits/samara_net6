namespace Bars.GisIntegration.Gkh.Service.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения объектов при выполнении импорта/экспорта данных через сервис Bills
    /// </summary>
    public class BillsService : IBillsService
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <summary>
        /// Метод возвращает запросы на проведения квитирования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список запросов</returns>
        public IDataResult GetAcknowledgments(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var dataExtractor = this.Container.Resolve<IDataExtractor<RisAcknowledgment>>("AcknowledgmentDataExtractor");
            var houseRelationDomain = this.Container.ResolveDomain<RisAccountRelations>();

            dataExtractor.Contragent = this.GetContragent();

            using (this.Container.Using(dataExtractor, houseRelationDomain))
            {
                var data = dataExtractor.Extract(new DynamicDictionary())
                    .Select(x => new
                    {
                        x.Id,
                        Period = x.Notification.OrderDate ?? new DateTime(x.PaymentDocument.PeriodYear, x.PaymentDocument.PeriodMonth, 1),
                        Address = houseRelationDomain.GetAll().Where(y => y.Account.Id == x.PaymentDocument.Account.Id).Select(y => y.House.Adress).FirstOrDefault(),
                        x.PaymentDocument.PaymentDocumentNumber,
                        x.Notification.OrderNum
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container)
                    .Order(loadParams);

                var count = data.Count();

                return new ListDataResult(data.Paging(loadParams).ToList(), count);
            }
        }

        private RisContragent GetContragent()
        {
            var dataSupplierProvider = this.Container.Resolve<IDataSupplierProvider>();
            using (this.Container.Using(dataSupplierProvider))
            {
                return dataSupplierProvider.GetCurrentDataSupplier();
            }
        }
    }
}