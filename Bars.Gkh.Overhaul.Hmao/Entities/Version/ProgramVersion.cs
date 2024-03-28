namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Версия программы
    /// </summary>
    public class ProgramVersion : BaseGkhEntity, IHaveExportId
    {
        /// <summary>
        /// Название версия
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата создания версии
        /// </summary>
        public virtual DateTime VersionDate { get; set; }
        
        /// <summary>
        /// ГИС ЖКХ ГУИД
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// Основная ли программа
        /// </summary>
        public virtual bool IsMain { get; set; }

        /// <summary>
        /// Дата последнего добавления актуальных записей
        /// </summary>
        public virtual DateTime ActualizeDate { get; set; }

        /// <summary>
        /// Версия,с которой сделано копирование
        /// </summary>
        public virtual ProgramVersion ParentVersion { get; set; }
        
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <inheritdoc />
        public virtual long ExportId { get; set; }
    }
}