namespace Bars.GkhDi.MetaValueConstructor.DataFillers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Фактически собранные платежи за отчетный период (Получено за предоставленные услуги)
    /// </summary>
    public class FactIncomeCompanyDataFiller : BaseManagingOrganizationDataFiller
    {
        private IDictionary<long, DisclosureInfoHouseManagingInfo> disclosureInfoHouseManagingInfo;

        /// <summary>
        /// Интерфейс сервиса получения информации о денежных движениях по управлению домами
        /// </summary>
        public IDisclosureInfoHouseManagingMoneys DisclosureInfoHouseManagingMoneys { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DisclosureInfo"/>
        /// </summary>
        public IDomainService<DisclosureInfo> DisclosureInfoDomainService { get; set; }

        /// <inheritdoc />
        public override void PrepareCache(BaseParams baseParams)
        {
            base.PrepareCache(baseParams);

            this.disclosureInfoHouseManagingInfo = baseParams.Params.GetAs<IDictionary<long, DisclosureInfoHouseManagingInfo>>("disclosureInfoHouseManagingInfo");

            if (this.disclosureInfoHouseManagingInfo == null)
            {
                var disInfoQuery =
                    this.DisclosureInfoDomainService.GetAll()
                        .Where(x => this.ManorgQuery.Any(y => y.Id == x.ManagingOrganization.Id))
                        .Where(this.period.CreateContainsExpression<DisclosureInfo>(x => x.PeriodDi));

                this.disclosureInfoHouseManagingInfo = this.DisclosureInfoHouseManagingMoneys.GetDisclosureInfoHouseManagingInfo(disInfoQuery);
                baseParams.Params.SetValue("disclosureInfoHouseManagingInfo", this.disclosureInfoHouseManagingInfo);
            }
        }

        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = this.disclosureInfoHouseManagingInfo.Get(value.EfManagingOrganization.ManagingOrganization.Id)?.ReceivedProvidedService ?? 0;
        }
    }
}