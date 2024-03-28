namespace Bars.Gkh.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class PeriodNormConsumptionInterceptor : EmptyDomainInterceptor<PeriodNormConsumption>
    {
        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<PeriodNormConsumption> service, PeriodNormConsumption entity)
        {
            var normConsRecRdomain = this.Container.ResolveDomain<NormConsumptionRecord>();
            var normConsDomain = this.Container.ResolveDomain<NormConsumption>();

            try
            {
                var normConsRecIds = normConsRecRdomain.GetAll().Where(x => x.NormConsumption.Period.Id == entity.Id).Select(x => x.Id).ToList();
                var normConsids = normConsDomain.GetAll().Where(x => x.Period.Id == entity.Id).Select(x => x.Id).ToList();

                foreach (var normConsRec in normConsRecIds)
                {
                    normConsRecRdomain.Delete(normConsRec);
                }

                foreach (var normCons in normConsids)
                {
                    normConsDomain.Delete(normCons);
                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(normConsDomain);
                this.Container.Release(normConsRecRdomain);
            }
        }
    }
}