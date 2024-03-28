namespace Bars.GkhGji.Interceptors.FuelInfo
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    using NHibernate.Util;

    /// <summary>
    /// Интерцептор для Период сведений о наличии и расходе топлива
    /// </summary>
    public class FuelInfoPeriodInterceptor : EmptyDomainInterceptor<FuelInfoPeriod>
    {
        /// <summary>
        /// Домен-сервис для Базовый класс сведений наличии и расходе топлива
        /// </summary>
        public IDomainService<BaseFuelInfo> BaseFuelInfoDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<FuelInfoPeriod> service, FuelInfoPeriod entity)
        {
            this.BaseFuelInfoDomain.GetAll()
                .Where(x => x.FuelInfoPeriod.Id == entity.Id)
                .ForEach(x => this.BaseFuelInfoDomain.Delete(x.Id));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}