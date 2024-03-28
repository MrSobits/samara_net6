namespace Bars.Gkh.Regions.Tatarstan.Entities.RealityObject
{
    using System;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Домофон
    /// </summary>
    public class Intercom : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Количество подъездных домофонов в доме
        /// </summary>
        public virtual int IntercomCount { get; set; }

        /// <summary>
        /// Наличие видеозаписи
        /// </summary>
        public virtual YesNoNotSet Recording { get; set; }

        /// <summary>
        /// Глубина архива видеозаписи (сут.)
        /// </summary>
        public virtual int? ArchiveDepth { get; set; }

        /// <summary>
        /// Постоянный удаленный доступ к архиву у МВД РТ
        /// </summary>
        public virtual YesNoNotSet ArchiveAccess{ get; set; }

        /// <summary>
        /// Минимальный тариф  (руб.)
        /// </summary>
        public virtual decimal? Tariff { get; set; }

        /// <summary>
        /// Планируемая дата смены аналогового домофона на IP - домофон
        /// </summary>
        public virtual DateTime? InstallationDate { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual IntercomUnitMeasure? UnitMeasure { get; set; }

        /// <summary>
        /// Нет единого тарифа
        /// </summary>
        public virtual bool HasNotTariff { get; set; }

        /// <summary>
        /// Количество подъездных аналоговых домофонов (ед.)
        /// </summary>
        public virtual int? AnalogIntercomCount { get; set; }

        /// <summary>
        /// Количество подъездных IP домофонов (ед.)
        /// </summary>
        public virtual int? IpIntercomCount { get; set; }

        /// <summary>
        /// Количество подъездов без домофонов в доме (ед.)
        /// </summary>
        public virtual int? EntranceCount { get; set; }

        /// <summary>
        /// Планируемая дата установки IP - домофона
        /// </summary>
        public virtual DateTime? IntercomInstallationDate { get; set; }
    }
}
