using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Sobits.GisGkh.Enums;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Скачивание файлов из ГИС ЖКХ
    /// </summary>
    public class GisGkhDownloads : BaseEntity
    {
        /// <summary>
        /// Guid файла
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        public virtual string EntityT { get; set; }

        /// <summary>
        /// Id записи
        /// </summary>
        public virtual long RecordId { get; set; }

        /// <summary>
        /// Поле для файла
        /// </summary>
        public virtual string FileField { get; set; }

        /// <summary>
        /// orgPpaGuid
        /// </summary>
        public virtual string orgPpaGuid { get; set; }
    }
}
