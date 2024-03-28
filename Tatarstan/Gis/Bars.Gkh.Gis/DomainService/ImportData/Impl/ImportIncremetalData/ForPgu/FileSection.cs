using Bars.Gkh.Gis.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.Gis.DomainService.ImportData.Impl.ImportIncremetalData.ForPgu
{
    /// <summary>
    /// Обрабатываемая секция
    /// </summary>
    public class FileSection
    {
        /// <summary>
        /// Секция в файле
        /// </summary>
        public PguFileSection PguFileSection { get; set; }

        /// <summary>
        /// Наименование таблицы, соответствующей секции
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Столбцы таблицы
        /// </summary>
        public string[] Columns { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}
