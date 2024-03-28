namespace Bars.GisIntegration.RegOp.DataExtractors.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using GisIntegration.Base.DataExtractors;
    using GisIntegration.Base.Entities;

    /// <summary>
    /// Экстрактор платежных периодов
    /// </summary>
    public class OpenOrgPaymentPeriodExtractor : BaseDataExtractor<OrgPaymentPeriod, ChargePeriod>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<ChargePeriod> GetExternalEntities(DynamicDictionary parameters)
        {
            var extPeriodRepository = this.Container.ResolveDomain<ChargePeriod>();
            var periodRepository = this.Container.ResolveDomain<OrgPaymentPeriod>();
            var contragentRepository = this.Container.ResolveDomain<RisContragent>();

            var specifiedPeriodId = parameters.GetAs<long?>("specifiedPeriodId");
            var contragentId = parameters.GetAs<long?>("contragentId");
            if (contragentId != null)
            {
                this.Contragent = contragentRepository.Get(contragentId);
            }

            try
            {
                //Определяем, у текущего контрагента последний открытый период
                var lastOpenedPeriod = periodRepository
                    .GetAll()
                    .Where(x => x.Contragent.Id == this.Contragent.Id)
                    .Where(x => x.IsApplied)
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .FirstOrDefault();

                DateTime? nextPeriod = null;
                if (lastOpenedPeriod != null)
                {
                    nextPeriod = new DateTime(lastOpenedPeriod.Year, lastOpenedPeriod.Month, 1).AddMonths(1);
                }

                var res = extPeriodRepository
                    .GetAll()
                    .WhereIf(specifiedPeriodId != null, x => x.Id == specifiedPeriodId)
                    .WhereIf(nextPeriod != null, x => x.StartDate.Year == nextPeriod.Value.Year && x.StartDate.Month == nextPeriod.Value.Month)
                    .ToList();

                return res;
            }
            finally
            {
                this.Container.Release(extPeriodRepository);
                this.Container.Release(periodRepository);
                this.Container.Release(contragentRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(ChargePeriod externalEntity, OrgPaymentPeriod risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "ris";
            risEntity.Contragent = this.Contragent;
            risEntity.Month = externalEntity.StartDate.Month;
            risEntity.Year = externalEntity.StartDate.Year;
            //в регионах, где региональный оператор - всегда передаем УО
            risEntity.RisPaymentPeriodType = RisPaymentPeriodType.UO;
        }
    }
}