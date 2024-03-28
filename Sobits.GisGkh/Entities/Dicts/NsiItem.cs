using Bars.B4.DataAccess;
using Bars.Gkh.Enums;
using System;

namespace Sobits.GisGkh.Entities
{
    public class NsiItem : BaseEntity
    {
        /// <summary>
        /// Справочник
        /// </summary>
        public virtual NsiList NsiList { get; set; }

        /// <summary>
        /// Код пункта справочника в ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhItemCode { get; set; }

        /// <summary>
        /// GUID пункта справочника в ГИС ЖКХ
        /// </summary>
        public virtual string GisGkhGUID { get; set; }

        /// <summary>
        /// ИД записи в системном справочнике
        /// </summary>
        public virtual Int64? EntityItemId { get; set; }

        /// <summary>
        /// Актальность
        /// </summary>
        public virtual YesNo IsActual { get; set; }

        /// <summary>
        /// Родительский элемент
        /// </summary>
        public virtual NsiItem ParentItem { get; set; }
    }
}