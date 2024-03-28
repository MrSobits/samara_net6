namespace Bars.GkhGji.Domain.SpecialAccountReport.Serialize
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Bars.GkhGji.Entities;
    using Gkh.Entities;
    using System;

    public class ReportElement
    {
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Ro
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Номер специального счета
        /// </summary>
        public virtual string SpecialAccountNum { get; set; }

        // <summary>
        /// Поступление взносов
        /// </summary>
        public virtual Decimal Incoming { get; set; }

        // <summary>
        /// Размер задолжнности
        /// </summary>
        public virtual Decimal AmmountDebt { get; set; }

        // <summary>
        /// Размер остатка
        /// </summary>
        public virtual Decimal Ballance { get; set; }

        // <summary>
        /// Размер перечислений
        /// </summary>
        public virtual Decimal Transfer { get; set; }
    }
}