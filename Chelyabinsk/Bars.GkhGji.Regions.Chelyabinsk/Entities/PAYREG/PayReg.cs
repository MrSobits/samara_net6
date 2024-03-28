
namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;
    using Enums;
    using System;

    public class PayReg : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС "получение платежей" (начисление)
        /// </summary>
        public virtual GisGmp GisGmp { get; set; }

        ///// <summary>
        /////Файл
        ///// </summary>
        //public virtual  FileInfo FileInfo { get; set; }

        /// <summary>
        ///amount
        /// </summary>
        public virtual decimal Amount { get; set; }


        /// <summary>
        ///kbk
        /// </summary>
        public virtual string Kbk { get; set; }

        /// <summary>
        ///oktmo
        /// </summary>
        public virtual string OKTMO { get; set; }

        /// <summary>
        ///paymentDate
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        ///paymentId
        /// </summary>
        public virtual string PaymentId { get; set; }

        /// <summary>
        ///purpose
        /// </summary>
        public virtual string Purpose { get; set; }

        /// <summary>
        ///supplierBillID УИН
        /// </summary>
        public virtual string SupplierBillID { get; set; }

        /// <summary>
        /// Тип организации, через которую проведена оплата
        /// </summary>
        public virtual string PaymentOrg { get; set; }

        /// <summary>
        /// Организация, через которую проведена оплата
        /// </summary>
        public virtual string PaymentOrgDescr { get; set; }

        /// <summary>
        /// Идентификатор плательщика
        /// </summary>
        public virtual string PayerId { get; set; }

        /// <summary>
        /// Счёт плательщика
        /// </summary>
        public virtual string PayerAccount { get; set; }

        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public virtual string PayerName { get; set; }

        /// <summary>
        /// Статус плательщика (поле 101)
        /// </summary>
        public virtual string BdiStatus { get; set; }

        /// <summary>
        /// Основание платежа (поле 106)
        /// </summary>
        public virtual string BdiPaytReason { get; set; }

        /// <summary>
        /// Период платежа (поле 107)
        /// </summary>
        public virtual string BdiTaxPeriod { get; set; }

        /// <summary>
        /// Номер документа (поле 108)
        /// </summary>
        public virtual string BdiTaxDocNumber { get; set; }

        /// <summary>
        /// Дата документа (поле 109)
        /// </summary>
        public virtual string BdiTaxDocDate { get; set; }

        /// <summary>
        /// Дата платежного документа
        /// </summary>
        public virtual DateTime? AccDocDate { get; set; }

        /// <summary>
        /// Номер платежного документа
        /// </summary>
        public virtual string AccDocNo { get; set; }

        /// <summary>
        /// Статус платежа
        /// </summary>
        public virtual byte? Status { get; set; }

        /// <summary>
        ///Сквитировано
        /// </summary>
        public virtual YesNoNotSet Reconcile { get; set; }

        ///// <summary>
        /////Сквитировано
        ///// </summary>
        //public virtual YesNoNotSet Reconcile { get; set; }

        public virtual string GisGmpUIN { get; set; }

        /// <summary>
        ///Сквитировано
        /// </summary>
        public virtual YesNoNotSet IsGisGmpConnected { get; set; }

        /// <summary>
        ///Добавлено в оплаты штрафов
        /// </summary>
        public virtual Boolean IsPayFineAdded { get; set; }

    }
}
