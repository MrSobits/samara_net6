﻿namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.ImportExport;

    /// <summary>
    /// Статус протокола МКД
    /// </summary>
    public class ProtocolMKDState : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }
    }
}

