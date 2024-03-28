namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Прокуратура для ГИС ЕРП
    /// </summary>
    public class ProsecutorOffice : BaseEntity
    {
        /// <summary>
        /// Код региона
        /// </summary>
        public virtual string RegionsCode { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string ERKNMCode { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код НП
        /// </summary>
        public virtual string LocalAreasCode { get; set; }
       
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код федерального центра
        /// </summary>
        public virtual string FederalCentersCode { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }

    }
}
