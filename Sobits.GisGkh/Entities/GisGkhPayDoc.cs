using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.RegOperator.Entities;
using Sobits.GisGkh.Enums;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Платёжный документ ГИС ЖКХ
    /// </summary>
    public class GisGkhPayDoc : BaseEntity
    {
        /// <summary>
        /// Лицевой счёт
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }

        /// <summary>
        /// Идентификатор платёжного документа
        /// </summary>
        public virtual string PaymentDocumentID { get; set; }

        /// <summary>
        /// Транспортный идентификатор платёжного документа
        /// </summary>
        public virtual string PaymentDocumentTransportGUID { get; set; }

        /// <summary>
        /// GUID платёжного документа
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
    }
}
