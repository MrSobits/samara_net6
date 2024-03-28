namespace Bars.Gkh.Entities.Administration.FormatDataExport
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums.Administration.FormatDataExport;

    public class FormatDataExportEntity : BaseEntity
    {
        /// <summary>
        /// Идентификатор сущности в ИС РСКР
        /// </summary>
        public virtual string EntityId { get; set; }

        /// <summary>
        /// Идентификатор сущности в ГИС ЖКХ
        /// </summary>
        public virtual Guid? ExternalGuid { get; set; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        public virtual EntityType EntityType { get; set; }

        /// <summary>
        /// Дата выгрузки в ГИС ЖКХ
        /// </summary>
        public virtual DateTime? ExportDate { get; set; }

        /// <summary>
        /// Статус выгрузки 
        /// </summary>
        public virtual FormatDataExportEntityState ExportEntityState { get; set; }

        /// <summary>
        /// Причина ошибки выгрузки
        /// </summary>
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Информация о загрузке
        /// </summary>
        public virtual FormatDataExportInfo FormatDataExportInfo { get; set; }
    }
}
