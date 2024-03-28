namespace Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption
{
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Сведения для норматива потребления Подогрев
    /// </summary>
    public class NormConsumptionHeating : NormConsumptionRecord
    {
        /// <summary>
        /// Наличие технической возможности установки ОПУ, да/нет
        /// </summary>
        public virtual YesNo TechnicalCapabilityOpu { get; set; }

        /// <summary>
        /// Наличие отдельного ипу у нежилых помещений (да/нет)
        /// </summary>
        public virtual YesNo IsIpuNotLivingPermises { get; set; }

        /// <summary>
        /// Расход по показаниям ИПУ нежилых помещений суммарный расход тепловой энергии на подогрев ХВ, 
        /// по показаниям ИПУ нежилых помещений за отопительный период 2014-2015 Гкал
        /// </summary>
        public virtual decimal? HeatEnergyConsumptionInPeriod { get; set; }

        /// <summary>
        /// Расход по показаниям ИПУ нежилых помещений суммарный расход горячей воды
        /// по показаниям ИПУ нежилых помещений за отопительный период 2014-2015 куб.м 
        /// </summary>
        public virtual decimal? HotWaterConsumptionInPeriod { get; set; }

        /// <summary>
        /// Вид системы горячего водоснабжения (с наружной сетью ГВС, без наружной сети ГВС)
        /// </summary>
        public virtual TypeHotWaterSystem TypeHotWaterSystem { get; set; }

        /// <summary>
        /// Наличие полотенце сушителей да/нет
        /// </summary>
        public virtual YesNo IsHeatedTowelRail { get; set; }

        /// <summary>
        /// Наличие стояков (изолированные, не изолированные)
        /// </summary>
        public virtual TypeRisers Risers { get; set; }

        /// <summary>
        /// Для домов с ОПУ суммарный расход тепловой энергии на подогрев ХВ, 
        /// по показаниям ОПУза отопительный период 2014-2015 Гкал 
        /// (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) Гкал
        /// </summary>
        /// <returns></returns>
        public virtual decimal? HeatEnergyConsumptionNotLivInPeriod { get; set; }

        /// <summary>
        /// Для домов с ОПУ суммарный расход горячей воды  по показаниям ОПУза отопительный период 2014-2015 
        /// (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) куб.м
        /// </summary>
        public virtual decimal? HotWaterConsumptionNotLivInPeriod { get; set; }

        /// <summary>
        /// Отопительный период, дни
        /// </summary>
        public virtual int? HeatingPeriod { get; set; }

        /// <summary>
        /// Средняя температура холодной воды в сети водопровода, 
        /// по данным Гидрометеослужбы (при наличии сведений)
        /// </summary>
        public virtual decimal? AvgTempColdWater { get; set; }

        /// <summary>
        /// Износ внутридомовых инженерных сетей %
        /// </summary>
        public virtual decimal? WearIntrahouseUtilites { get; set; }
    }
}