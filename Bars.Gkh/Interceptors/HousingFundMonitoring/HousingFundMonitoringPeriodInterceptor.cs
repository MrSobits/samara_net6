namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    using NHibernate.Util;

    /// <summary>
    /// Интерцептор для Период Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringPeriodInterceptor : EmptyDomainInterceptor<HousingFundMonitoringPeriod>
    {
        /// <summary>
        /// Домен-сервис для Запись Мониторинга жилищного фонда
        /// </summary>
        public IDomainService<HousingFundMonitoringInfo> HousingFundMonitoringInfoDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<HousingFundMonitoringPeriod> service, HousingFundMonitoringPeriod entity)
        {
            this.HousingFundMonitoringInfoDomain.GetAll()
                .Where(x => x.Period.Id == entity.Id)
                .ForEach(x => this.HousingFundMonitoringInfoDomain.Delete(x.Id));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}