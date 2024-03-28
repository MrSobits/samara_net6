namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using System;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    /// <summary>Маппинг для "Процент планируемых оплат"</summary>
    public class PlanPaymentsPercentageMap : BaseEntityMap<PlanPaymentsPercentage>
    {

        public PlanPaymentsPercentageMap() :
                base("Процент планируемых оплат", "GKH_PLAN_PAYMENTS_PERCENTAGE")
        {
        }
        protected override void Map()
        {
            this.Reference(x => x.PublicServiceOrg, "Поставщик ресурсов").Column("PUBLIC_SERVICE_ORG").NotNull();
            this.Reference(x => x.Service, "Услуга").Column("SERVICE").NotNull();
            this.Reference(x => x.Resource, "Ресурс").Column("RESOURCE").NotNull();
            this.Property(x => x.Percentage, "Процент оплаты").Column("PERCENTAGE").NotNull();
            this.Property(x => x.DateStart, "Дата начала действия").Column("DATE_START").DefaultValue(DateTime.MinValue).NotNull();
            this.Property(x => x.DateEnd, "Дата окончания действия").Column("DATE_END").DefaultValue(DateTime.MinValue).NotNull();
        }
    }
}