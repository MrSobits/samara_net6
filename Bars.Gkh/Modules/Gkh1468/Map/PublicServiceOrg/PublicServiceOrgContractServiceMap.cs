namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class PublicServiceOrgContractServiceMap : BaseImportableEntityMap<PublicServiceOrgContractService>
    {
        public PublicServiceOrgContractServiceMap() : base("GKH_RO_PUBRESORG_SERVICE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.StartDate, "Дата начала предоставления услуги").Column("START_DATE");
            this.Property(x => x.EndDate, "Дата окончания предоставления услуги").Column("END_DATE");
            this.Property(x => x.HeatingSystemType, "Тип системы теплоснабжения").Column("HEATING_SYSTEM_TYPE");
            this.Property(x => x.SchemeConnectionType, "Схема присоединения").Column("SCHEME_CONN_TYPE");
            this.Property(x => x.PlanVolume, "Плановый объём").Column("PLAN_VOLUME");
            this.Property(x => x.ServicePeriod, "Режим подачи").Column("SERVICE_PERIOD").Length(255);

            this.Reference(x => x.ResOrgContract, "Договор поставщика ресурсов с домом").Column("PUB_RESORG_ID").NotNull().Fetch();
            this.Reference(x => x.Service, "Услуга").Column("SRV_ID").NotNull().Fetch();
            this.Reference(x => x.CommunalResource, "Коммунальный ресурс").Column("COMMUN_RES_ID").NotNull().Fetch();
            this.Reference(x => x.UnitMeasure, "Единица измерения").Column("UNIT_MEASURE_ID").Fetch();
        }
    }
}
