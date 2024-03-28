namespace Bars.GkhDi.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Архив процента блока
    /// </summary>
    public class BaseArchivePercent : BaseEntity
    {
        /// <summary>
        /// Код блока
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Тип сущности блока в расчете процентов
        /// </summary>
        public virtual TypeEntityPercCalc TypeEntityPercCalc { get; set; }

        /// <summary>
        /// Процент раскрытия
        /// </summary>
        public virtual decimal? Percent { get; set; }

        /// <summary>
        ///  Дата расчета
        /// </summary>
        public virtual DateTime? CalcDate { get; set; }

        /// <summary>
        ///  Актуальная версия
        /// </summary>
        public virtual int ActualVersion { get; set; }

        /// <summary>
        /// Количество пунктов
        /// </summary>
        public virtual int PositionsCount { get; set; }

        /// <summary>
        /// Количество запоолненных пунктов
        /// </summary>
        public virtual int CompletePositionsCount { get; set; }
    }
}
