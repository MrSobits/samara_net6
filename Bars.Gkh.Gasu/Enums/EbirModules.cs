namespace Bars.Gkh.Gasu.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Модуль ЕБИР
    /// </summary>
    public enum EbirModule
    {
        /// <summary>
        /// Инспектирование жилищного фонда
        /// </summary>
        [Display("Инспектирование жилищного фонда")]
        InspectionOfHousingStock = 1,

        /// <summary>
        /// Ветхий
        /// </summary>
        [Display("Управление в сфере ЖКХ")]
        ManageInHousingSector = 2,

        /// <summary>
        /// Исправный
        /// </summary>
        [Display("Мониторинг энергоэффективности")]
        MonitoringOfEnergyEfficiency = 3,

        /// <summary>
        /// Снесен
        /// </summary>
        [Display("Управление тарифами в регионе")]
        ManageRegionTarif = 4,

        /// <summary>
        /// Снесен
        /// </summary>
        [Display("Управление программой капитального ремонта")]
        ManageProgramCr = 5
    }
}