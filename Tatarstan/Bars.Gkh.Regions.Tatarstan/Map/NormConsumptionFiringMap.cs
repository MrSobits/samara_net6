namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionFiringMap : JoinedSubClassMap<NormConsumptionFiring>
    {
        public NormConsumptionFiringMap()
            : base("Норматив потребления отопления", "GKH_TAT_NORM_CONS_FIRING")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TechnicalCapabilityOpu, "Наличие технической возможности установки ОПУ").Column("TECH_CAPABILITY_OPU").NotNull();
            this.Property(x => x.IsIpuNotLivingPermises, "Наличие отдельного ипу у нежилых помещений").Column("IS_IPU_NOT_LIV_PERMISES").NotNull();
            this.Property(x => x.AreaNotLivingIpu, "Площадь нежилых помещений при наличии отдельного ипу у нежилых помещений")
                .Column("AREA_NOT_LIV_IPU");
            this.Property(x => x.AmountHeatEnergyNotLivingIpu, "Количество тепловой энергии на отопление по нежилым помещениям при наличии ипу")
                .Column("AMOUNT_HEAT_NOT_LIV_IPU");
            this.Property(x => x.AmountHeatEnergyNotLivInPeriod, "Для домов с ОПУ Количество тепловой энергии на отопление жилых домов " +
                "за отопительный период 2014-2015г. (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) Гкал")
                .Column("AMOUNT_HEAT_NOT_LIV_PERIOD");
            this.Property(x => x.HeatingPeriod, "Отопительный период, дни").Column("HEATING_PERIOD");
            this.Property(x => x.HourlyHeatLoadForPassport, "Часовая тепловая нагрузка, Ккал/час по паспорту здания")
                .Column("HOURLY_HEAT_LOAD_FOR_PASS");
            this.Property(x => x.HourlyHeatLoadForDocumentation, "Часовая тепловая нагрузка, Ккал/час по проектной документации")
                .Column("HOURLY_HEAT_LOAD_FOR_DOC");
        }
    }
}