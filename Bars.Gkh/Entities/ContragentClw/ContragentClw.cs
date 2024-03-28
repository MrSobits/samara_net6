using Bars.B4.DataAccess;
using System;

namespace Bars.Gkh.Entities
{

    /// <summary>
    /// Контрагент ПИР
    /// </summary>
    public class ContragentClw : BaseEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Дела с даты
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// Дела по дату
        /// </summary>
        public virtual DateTime DateTo { get; set; }
    }
}
