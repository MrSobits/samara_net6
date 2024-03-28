namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Заявление о выдаче судебного приказа без искового заявления
    /// </summary>
    public class CourtOrderClaim : Lawsuit
    {
        /// <summary>
        /// Поступило возражение
        /// </summary>
        public virtual YesNo ObjectionArrived { get; set; }

        /// <summary>
        /// Дата заявления
        /// </summary>
        public virtual DateTime ClaimDate { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Document { get; set; }
    }
}