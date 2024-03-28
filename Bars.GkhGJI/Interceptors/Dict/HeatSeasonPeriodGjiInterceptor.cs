namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class HeatSeasonPeriodGjiInterceptor : EmptyDomainInterceptor<HeatSeasonPeriodGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<HeatSeasonPeriodGji> service, HeatSeasonPeriodGji entity)
        {
            if (Container.Resolve<IDomainService<HeatSeason>>().GetAll().Any(x => x.Period.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Подготовка к отопительному сезону;");
            }

            return this.Success();
        }
    }
}