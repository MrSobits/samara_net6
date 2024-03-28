namespace Bars.Gkh.Entities.Administration.FormatDataExport
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums.Administration.FormatDataExport;

    public class FormatDataExportInfo : BaseEntity
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual FormatDataExportState State { get; set; }

        /// <summary>
        /// Наименование программы
        /// </summary>
        public virtual FormatDataExportObjectType ObjectType { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public virtual DateTime LoadDate { get; set; }

    }
}
