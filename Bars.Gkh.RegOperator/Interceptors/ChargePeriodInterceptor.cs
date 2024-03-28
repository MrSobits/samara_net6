namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;
    using B4;

    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain;

    using Entities;

    public class ChargePeriodInterceptor : EmptyDomainInterceptor<ChargePeriod>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ChargePeriod> service, ChargePeriod entity)
        {
            if (service.GetAll()
                    .Any(x => x.StartDate == entity.StartDate && x.EndDate == entity.EndDate && x.Id != entity.Id))
            {
                return Failure("Невозможно создать два периода начислений в одном временном промежутке!");
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<ChargePeriod> service, ChargePeriod entity)
        {
            ChargePeriodProvider.Repository.InvalidateCache();

            return this.Success();
        }
    }
}