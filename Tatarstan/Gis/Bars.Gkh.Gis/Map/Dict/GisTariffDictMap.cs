namespace Bars.Gkh.Gis.Map.Dict
{
    using B4.Modules.Mapping.Mappers;
    using Entities.Dict;

    public class GisTariffDictMap : BaseEntityMap<GisTariffDict>
    {
        public GisTariffDictMap() : 
                base("Справочник тарифы ГИС ЖКХ", "GIS_TARIFF_DICTIONARY")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExternalId, "Идентификатор в системе ЕИАС").Column("EXTERNAL_ID");
            this.Property(x => x.EiasUploadDate, "Дата загрузки в ЕИАС").Column("EIAS_UPLOAD_DATE");
            this.Property(x => x.EiasEditDate, "Дата последнего изменения в ЕИАС").Column("EIAS_EDIT_DATE");
            this.Property(x => x.ActivityKind, "Вид деятельности поставщика").Column("ACTIVITY_KIND");
            this.Property(x => x.ContragentName, "Наименование контрагента в базовом периоде").Column("CONTRAGENT_NAME");
            this.Property(x => x.StartDate, "Дата начала периода").Column("START_DATE").NotNull();
            this.Property(x => x.EndDate, "Дата окончания периода").Column("END_DATE").NotNull();
            this.Property(x => x.TariffKind, "Вид тарифа").Column("TARIFF_KIND").NotNull();
            this.Property(x => x.ZoneCount, "Количество зон").Column("ZONE_COUNT");
            this.Property(x => x.TariffValue, "Значение тарифа").Column("TARIFF_VALUE");
            this.Property(x => x.TariffValue1, "Значение тарифа 1").Column("TARIFF_VALUE1");
            this.Property(x => x.TariffValue2, "Значение тарифа 2").Column("TARIFF_VALUE2");
            this.Property(x => x.TariffValue3, "Значение тарифа 3").Column("TARIFF_VALUE3");
            this.Property(x => x.IsNdsInclude, "Включая НДС").Column("IS_NDS_INCLUDE");
            this.Property(x => x.IsSocialNorm, "В пределах социальной нормы").Column("IS_SOCIAL_NORM");
            this.Property(x => x.IsMeterExists, "Наличие прибора учета").Column("IS_METER_EXISTS");
            this.Property(x => x.IsElectricStoveExists, "Наличие электрической плиты").Column("IS_ELECTRIC_STOVE_EXISTS");
            this.Property(x => x.Floor, "Этаж").Column("FLOOR");
            this.Property(x => x.ConsumerType, "Вид потребителя").Column("CONSUMER_TYPE");
            this.Property(x => x.SettelmentType, "Вид населенногоо пункта").Column("SETTELMENT_TYPE");
            this.Property(x => x.ConsumerByElectricEnergyType, "Тип потребителя по электроэнергии").Column("CONSUMER_BY_ELECTRIC_ENERGY_TYPE");
            this.Property(x => x.RegulatedPeriodAttribute, "Дополнительный признак организации в регулируемом периоде").Column("REG_PERIOD_ATTRIBUTE");
            this.Property(x => x.BasePeriodAttribute, "Дополнительный признак организации в базовом периоде").Column("BASE_PERIOD_ATTRIBUTE");
            

            this.Reference(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_ID").NotNull();
            this.Reference(x => x.Service, "Услуга").Column("SERVICE_ID").NotNull();
            this.Reference(x => x.Contragent, "Поставщик").Column("CONTRAGENT_ID").NotNull();
        }
    }
}