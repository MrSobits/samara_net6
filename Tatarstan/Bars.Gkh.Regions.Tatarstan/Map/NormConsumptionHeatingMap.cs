namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionHeatingMap : JoinedSubClassMap<NormConsumptionHeating>
    {
        public NormConsumptionHeatingMap()
            : base("Норматив потребления подогрева", "GKH_TAT_NORM_CONS_HEATING")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TechnicalCapabilityOpu, "Наличие технической возможности установки ОПУ").Column("TECH_CAPABILITY_OPU").NotNull();
            this.Property(x => x.IsIpuNotLivingPermises, "Наличие отдельного ипу у нежилых помещений").Column("IS_IPU_NOT_LIV_PERMISES").NotNull();
            // this.Property(x => x.HeatEnergyConsumptionInPeriod, "Расход по показаниям ИПУ нежилых помещений суммарный расход тепловой энергии на подогрев ХВ, "
            //     + "по показаниям ИПУ нежилых помещений за отопительный период 2014-2015 Гкал").Column("IS_IPU_NOT_LIV_PERMISES");
            this.Property(x => x.HotWaterConsumptionInPeriod, "Расход по показаниям ИПУ нежилых помещений суммарный расход горячей воды " +
                "по показаниям ИПУ нежилых помещений за отопительный период 2014-2015 куб.м ")
                .Column("HOT_WATER_CONS_IN_PERIOD");
            this.Property(x => x.TypeHotWaterSystem, "Вид системы горячего водоснабжения (с наружной сетью ГВС, без наружной сети ГВС)")
                .Column("TYPE_HOT_WATTER_SYSTEM").NotNull();
            this.Property(x => x.IsHeatedTowelRail, "Наличие полотенце сушителей да/нет").Column("IS_HEATED_TOWEL_RAIL").NotNull();
            this.Property(x => x.Risers, "Наличие стояков (изолированные, не изолированные)").Column("RISERS").NotNull();
            this.Property(x => x.HeatEnergyConsumptionNotLivInPeriod, "Для домов с ОПУ суммарный расход тепловой энергии на подогрев ХВ, " +
                "по показаниям ОПУза отопительный период 2014-2015 Гкал (без показаний потребления тепловой энергии нежилыми помещениями " +
                "- магазинами и т.д.) Гкал").Column("HEAT_CONS_NOT_LIV_PERIOD");
            this.Property(x => x.HotWaterConsumptionNotLivInPeriod, "Для домов с ОПУ суммарный расход горячей воды  по показаниям ОПУ " +
                "за отопительный период 2014-2015 (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) куб.м")
                .Column("HOT_WATER_CONS_NOT_LIV_PER");
            this.Property(x => x.HeatingPeriod, "Отопительный период, дни").Column("HEATING_PERIOD");
            this.Property(x => x.AvgTempColdWater, "Средняя температура холодной воды в сети водопровода, " +
                "по данным Гидрометеослужбы (при наличии сведений)").Column("AVG_TEMP_COLD_WATER");
            this.Property(x => x.WearIntrahouseUtilites, "Износ внутридомовых инженерных сетей %").Column("WEAR_INTRAHOUSE_UTIL");
        }
    }
}