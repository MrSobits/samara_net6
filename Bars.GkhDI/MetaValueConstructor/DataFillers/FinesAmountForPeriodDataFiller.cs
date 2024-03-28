namespace Bars.GkhDi.MetaValueConstructor.DataFillers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Сумма штрафов за отчетный период 
    /// </summary>
    public class FinesAmountForPeriodDataFiller : BaseManagingOrganizationDataFiller
    {
        private IDictionary<long, decimal> finesAmountForPeriodDict;

        /// <summary>
        /// Домен-сервис <see cref="AdminResp"/>
        /// </summary>
        public IDomainService<AdminResp> AdminRespDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DisclosureInfo"/>
        /// </summary>
        public IDomainService<DisclosureInfo> DisclosureInfoDomainService { get; set; }

        /// <inheritdoc />
        public override void PrepareCache(BaseParams baseParams)
        {
            base.PrepareCache(baseParams);
            var periodDiExpression = this.period.CreateContainsExpression<DisclosureInfo>(y => y.PeriodDi);

            var diQuery = this.DisclosureInfoDomainService.GetAll()
                .Where(x => this.ManorgQuery.Any(y => y.Id == x.ManagingOrganization.Id))
                .Where(periodDiExpression);

            this.finesAmountForPeriodDict = this.AdminRespDomain.GetAll()
                .Where(x => diQuery.Any(y => y.Id == x.DisclosureInfo.Id))
                .Where(x => x.SumPenalty.HasValue)
                .Select(x => new
                {
                    x.DisclosureInfo.ManagingOrganization.Id,
                    SumPenalty = x.SumPenalty.Value
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.SumPenalty));
        }

        /// <summary>
        /// Метод заполняет значение поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = this.finesAmountForPeriodDict.Get(value.EfManagingOrganization.ManagingOrganization.Id);
        }
    }
}