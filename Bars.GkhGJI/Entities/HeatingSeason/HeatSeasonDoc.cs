namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Документ подготовки к отопительному сезону
    /// </summary>
    public class HeatSeasonDoc : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Отопительный сезон
        /// </summary>
        public virtual HeatSeason HeatingSeason { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public virtual HeatSeasonDocType TypeDocument { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}