using Bars.B4.DataAccess;
using System;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    /// <summary>
    /// Запрос справочника ЕРКНМ
    /// </summary>
    public class ERKNMDict : BaseEntity
    {
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Гуид справочника ЕРКНМ
        /// </summary>
        public virtual string DictGuid { get; set; }

        /// <summary>
        /// Гуид КНМ
        /// </summary>
        public virtual string ControlTypeGuid { get; set; }

        /// <summary>
        /// Гуид КНО
        /// </summary>
        public virtual string KNOGuid { get; set; }

        /// <summary>
        /// Дата сопоставления
        /// </summary>
        public virtual DateTime CompareDate { get; set; }

        /// <summary>
        /// Дата сопоставления
        /// </summary>
        public virtual string Answer { get; set; }
    }
}
