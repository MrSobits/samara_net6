namespace Bars.GisIntegration.RegOp.DataExtractors.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    /// Селектор для уведмолений, помеченных к удалению
    /// </summary>
    public class NotificationOfOrderExecutionCancellationDataSelector : IDataSelector<NotificationOfOrderExecution>
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        public List<NotificationOfOrderExecution> GetExternalEntities(DynamicDictionary parameters)
        {
            var contragent = parameters.GetAs<RisContragent>("Contragent");
            var stringSelectedData = parameters.GetAs("selectedData", string.Empty, true);
            var selectedData = stringSelectedData.ToLower() == "ALL" ? null : stringSelectedData.ToLongArray();

            var notificationDomain = this.Container.ResolveDomain<NotificationOfOrderExecution>();
            var transferDomain = this.Container.ResolveDomain<Transfer>();

            using (this.Container.Using(notificationDomain, transferDomain))
            {
                var res = new List<NotificationOfOrderExecution>();

                //Получаем записи из РИСовых таблиц
                res.AddRange(notificationDomain.GetAll()
                    .Fetch(x => x.RisPaymentDocument)
                    .ThenFetch(x => x.Account)
                    .WhereIf(contragent != null, x => x.Contragent.Id == contragent.Id)
                    .Where(x => x.Guid != null)
                    .Where(x => x.Operation == RisEntityOperation.Delete) //забираем только те, которые удалены из внешней системы, но остались у нас
                    .WhereIf(selectedData != null && selectedData.Any(), x => selectedData.Contains(x.Id))
                    .ToList());

                //Получаем записи из таблиц МЖФ
                res.AddRange(transferDomain.GetAll()
                    .Where(x => x.Operation.IsCancelled)
                    .WhereIf(selectedData != null && selectedData.Any(), x => selectedData.Contains(x.Id))
                    .Select(x => new NotificationOfOrderExecution
                    {
                        ExternalSystemName = "gkh",
                        Id = x.Id,
                        AccountNumber = x.OriginatorName,
                        OrderDate = x.OperationDate != DateTime.MinValue ? (DateTime?)x.OperationDate : null,
                        OrderId = x.Id.ToString(),
                        ObjectEditDate = x.PaymentDate,
                        OrderNum = x.Id.ToString()
                    }).ToList());

                return res;
            }
        }
    }
}