
namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;
    using Enums;
    using System;

    public class GISGMPPayments : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ГИС ГМП
        /// </summary>
        public virtual GisGmp GisGmp { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }

        /// <summary>
        ///paymentId
        /// </summary>
        public virtual string PaymentId { get; set; }

        /// <summary>
        ///supplierBillID УИН
        /// </summary>
        public virtual string SupplierBillID { get; set; }

        /// <summary>
        ///purpose
        /// </summary>
        public virtual string Purpose { get; set; }

        /// <summary>
        ///amount
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        ///paymentDate
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        ///kbk
        /// </summary>
        public virtual string Kbk { get; set; }

        /// <summary>
        ///oktmo
        /// </summary>
        public virtual string OKTMO { get; set; }

        /// <summary>
        ///Сквитировано
        /// </summary>
        public virtual YesNoNotSet Reconcile { get; set; }



    }
}
