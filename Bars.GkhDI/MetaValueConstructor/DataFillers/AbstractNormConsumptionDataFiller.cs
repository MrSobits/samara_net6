namespace Bars.GkhDi.MetaValueConstructor.DataFillers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Абстрактный источник данных нормативов потребления
    /// </summary>
    public abstract class AbstractNormConsumptionDataFiller : BaseManagingOrganizationDataFiller
    {
        private readonly string coldWaterCode = "17";
        private readonly string hotWaterCode = "18";
        private readonly string electricityCode = "20";
        private readonly string heatingCode = "22";

        private readonly string[] serviceCodes = { "17", "18", "20", "22" };
        protected IDictionary<long, NormConsumption> normConsumptionDict;

        /// <summary>
        /// Домен-сервис <see cref="CommunalService"/>
        /// </summary>
        public IDomainService<CommunalService> CommunalServiceDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="DisclosureInfo"/>
        /// </summary>
        public IDomainService<DisclosureInfoRelation> DisclosureInfoRelationDomainService { get; set; }

        /// <inheritdoc />
        public override void PrepareCache(BaseParams baseParams)
        {
            base.PrepareCache(baseParams);

            this.normConsumptionDict = baseParams.Params.GetAs<IDictionary<long, NormConsumption>>("normConsumptionDict");

            if (this.normConsumptionDict == null)
            {
                var disInfoQuery = this.DisclosureInfoRelationDomainService.GetAll()
                    .Where(x => this.ManorgQuery.Any(y => y.Id == x.DisclosureInfo.ManagingOrganization.Id))
                    .Where(this.period.CreateContainsExpression<DisclosureInfoRelation>(x => x.DisclosureInfo.PeriodDi));

                var roMoDict = disInfoQuery
                    .Select(z => new
                    {
                        RoId = z.DisclosureInfoRealityObj.RealityObject.Id,
                        MoId = z.DisclosureInfo.ManagingOrganization.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.MoId)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var communalServiceDict = this.CommunalServiceDomainService.GetAll()
                    .Where(x => disInfoQuery.Any(y => y.DisclosureInfoRealityObj.Id == x.DisclosureInfoRealityObj.Id))
                    .Where(x => this.serviceCodes.Contains(x.TemplateService.Code))
                    .Where(x => x.ConsumptionNormLivingHouse.HasValue)
                    .Select(x => new
                    {
                        x.DisclosureInfoRealityObj.RealityObject.Id,
                        x.TemplateService.Code,
                        ConsumptionNormLivingHouse = x.ConsumptionNormLivingHouse.Value
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => new
                        {
                            Heating = y.Where(x => x.Code == this.heatingCode).SafeAverage(x => x.ConsumptionNormLivingHouse),
                            ColdWater = y.Where(x => x.Code == this.coldWaterCode).SafeAverage(x => x.ConsumptionNormLivingHouse),
                            HotWater = y.Where(x => x.Code == this.hotWaterCode).SafeAverage(x => x.ConsumptionNormLivingHouse),
                            Electricity = y.Where(x => x.Code == this.electricityCode).SafeAverage(x => x.ConsumptionNormLivingHouse),
                        });

                this.normConsumptionDict = roMoDict
                    .ToDictionary(
                        x => x.Key,
                        y => new NormConsumption
                        {
                            ColdWater = y.Value.Select(x => communalServiceDict.Get(x.RoId)?.ColdWater ?? 0).Average(),
                            HotWater = y.Value.Select(x => communalServiceDict.Get(x.RoId)?.HotWater ?? 0).Average(),
                            Electricity = y.Value.Select(x => communalServiceDict.Get(x.RoId)?.Electricity ?? 0).Average(),
                            Heating = y.Value.Select(x => communalServiceDict.Get(x.RoId)?.Heating ?? 0).Average(),
                        });

                baseParams.Params.SetValue("normConsumptionDict", this.normConsumptionDict);
            }
        }

        /// <summary>
        /// Нормативы потребления
        /// </summary>
        protected class NormConsumption
        {
            /// <summary>
            /// ГВС
            /// </summary>
            public decimal HotWater { get; set; }

            /// <summary>
            /// ХВС
            /// </summary>
            public decimal ColdWater { get; set; }

            /// <summary>
            /// Электроэнергия
            /// </summary>
            public decimal Electricity { get; set; }

            /// <summary>
            /// Отопление
            /// </summary>
            public decimal Heating { get; set; }
        }
    }
}