namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Enum;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    /// <summary>
    /// Версионирование второго этапа
    /// </summary>
    public class VersionRecordStage2 : BaseEntity
    {
        /// <summary>
        /// Версия 3го этапа
        /// </summary>
        public virtual VersionRecord Stage3Version { get; set; }

        /// <summary>
        /// Вес конструктивного элемента
        /// </summary>
        public virtual int CommonEstateObjectWeight { get; set; }

        /// <summary>
        /// Сумма по 2му этапу
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Объект общего имущества
        /// </summary>
        public virtual CommonEstateObject CommonEstateObject { get; set; }

        /// <summary>
        /// тип записи ДПКР
        /// </summary>
        public virtual TypeDpkrRecord TypeDpkrRecord { get; set; }

        /// <summary>
        /// тип проведения корректировки
        /// </summary>
        public virtual TypeCorrection TypeCorrection { get; set; }

        [Obsolete("Это поле используется только при расчете, не хранимое поле")]
        public virtual decimal Volume { get; set; }
    }
}