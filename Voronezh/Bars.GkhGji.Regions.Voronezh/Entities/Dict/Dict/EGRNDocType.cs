namespace Bars.GkhGji.Regions.Voronezh.Entities
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
    /// Справочник "Документы ЕГРН"
    /// </summary>
    public class EGRNDocType : BaseEntity
    {
        /// <summary>
        /// Классификационный код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        ///Наименование 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        ///Комментарий
        /// </summary>
        public virtual string Description { get; set; }

    }
}
