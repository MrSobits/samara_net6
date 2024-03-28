namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Версия программы
    /// </summary>
    public class ProgramVersion : BaseEntity, IStatefulEntity, IHaveExportId
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime VersionDate { get; set; }

        /// <summary>
        /// Является основной
        /// </summary>
        public virtual bool IsMain { get; set; }

        /// <summary>
        /// Программа опубликована
        /// </summary>
        public virtual bool? IsProgramPublished { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Статус копирования
        /// </summary>
        public virtual ProgramVersionCopyingState CopyingState { get; set; }

        /// <inheritdoc />
        public virtual long ExportId { get; set; }
    }
}