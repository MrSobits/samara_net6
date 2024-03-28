namespace Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сведения для норматива потребления Отопление
    /// </summary>
    public class NormConsumptionFiring : NormConsumptionRecord
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
        /// Площадь нежилых помещений при наличии отдельного ипу у нежилых помещений, кв.м 
        /// </summary>
        public virtual decimal? AreaNotLivingIpu { get; set; }

        /// <summary>
        /// Количество тепловой энергии на отопление по нежилым помещениям при наличии ипу Гкал
        /// </summary>
        public virtual decimal? AmountHeatEnergyNotLivingIpu { get; set; }

        /// <summary>
        /// Для домов с ОПУ Количество тепловой энергии на отопление жилых домов за отопительный период 2014-2015г.
        /// (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) Гкал
        /// </summary>?
        public virtual decimal? AmountHeatEnergyNotLivInPeriod { get; set; }

        /// <summary>
        /// Отопительный период, дни
        /// </summary>
        public virtual int? HeatingPeriod { get; set; }

        /// <summary>
        /// Часовая тепловая нагрузка, Ккал/час по паспорту здания
        /// </summary>
        public virtual decimal? HourlyHeatLoadForPassport { get; set; }

        /// <summary>
        /// Часовая тепловая нагрузка, Ккал/час по проектной документации
        /// </summary>
        public virtual decimal? HourlyHeatLoadForDocumentation { get; set; }
    }
}