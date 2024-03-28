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
    /// Уволено за отчетный период
    /// </summary>
    public class DismissedDuringPeriodDataFiller : BaseManagingOrganizationDataFiller
    {
        private IDictionary<long, int> firedDuringPeriodDict;

        /// <summary>
        /// Домен-сервис <see cref="DisclosureInfo"/>
        /// </summary>
        public IDomainService<DisclosureInfo> DomainService { get; set; }

        /// <inheritdoc />
        public override void PrepareCache(BaseParams baseParams)
        {
            base.PrepareCache(baseParams);

            this.firedDuringPeriodDict =
                this.DomainService.GetAll()
                    .Where(x => this.ManorgQuery.Any(y => y.Id == x.ManagingOrganization.Id))
                    .Where(this.period.CreateContainsExpression<DisclosureInfo>(x => x.PeriodDi))
                    .Select(
                        x =>
                            new
                            {
                                x.ManagingOrganization.Id,
                                Dismissed = (x.DismissedAdminPersonnel ?? 0) + (x.DismissedEngineer ?? 0) + (x.DismissedWork ?? 0)
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Dismissed));
        }

        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = this.firedDuringPeriodDict.Get(value.EfManagingOrganization.ManagingOrganization.Id);
        }
    }
}