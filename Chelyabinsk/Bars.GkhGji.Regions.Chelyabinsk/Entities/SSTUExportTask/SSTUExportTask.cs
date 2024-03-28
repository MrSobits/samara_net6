using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bars.GkhGji.Enums;
using Bars.B4.Modules.FileStorage;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class SSTUExportTask : BaseEntity
    {
        /// <summary>
        /// Инициатор задания
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Статус выгрузки
        /// </summary>
        public virtual SSTUExportState SSTUExportState { get; set; }

        /// <summary>
        /// Сформированный фаил
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Источник поступления
        /// </summary>
        public virtual SSTUSource SSTUSource { get; set; }

        /// <summary>
        /// Экспортировать экспортированные
        /// </summary>
        public virtual bool ExportExported { get; set; }
    }
}
