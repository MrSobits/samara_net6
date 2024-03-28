namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Программа переселения
    /// </summary>
    public class EmerObjResettlementProgram : BaseGkhEntity
    {
        /// <summary>
        /// Аварийность жилого дома
        /// </summary>
        public virtual EmergencyObject EmergencyObject { get; set; }

        /// <summary>
        /// Источник по программе переселения
        /// </summary>
        public virtual ResettlementProgramSource ResettlementProgramSource { get; set; }

        /// <summary>
        /// Количество жителей
        /// </summary>
        public virtual int? CountResidents { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Плановая cтоимость
        /// </summary>
        public virtual decimal? Cost { get; set; }

        /// <summary>
        /// Фактическая cтоимость
        /// </summary>
        public virtual decimal? ActualCost { get; set; }
    }
}
