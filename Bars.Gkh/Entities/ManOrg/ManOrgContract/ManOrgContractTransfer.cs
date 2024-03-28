namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Управление домами, договор УК с ЖСК/ТСЖ
    /// </summary>
    public class ManOrgContractTransfer : ManOrgBaseContract
    {
        /// <summary>
        /// (не хранимое) Договор ЖСК/ТСЖ
        /// </summary>
        public virtual long JskTsjContractId { get; set; }

        /// <summary>
        /// ЖСК/ТСЖ
        /// </summary>
        public virtual ManagingOrganization ManOrgJskTsj { get; set; }

        /// <summary>
        /// День месяца начала ввода показаний по приборам учета
        /// </summary>
        public virtual byte InputMeteringDeviceValuesBeginDate { get; set; }

        /// <summary>
        /// День месяца окончания ввода показаний по приборам учета
        /// </summary>
        public virtual byte InputMeteringDeviceValuesEndDate { get; set; }

        /// <summary>
        /// День выставления платежных документов
        /// </summary>
        public virtual byte DrawingPaymentDocumentDate { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public virtual string ProtocolNumber { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        public virtual DateTime ProtocolDate { get; set; }

        /// <summary>
        /// Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFileInfo { get; set; }

        /// <summary>
        /// Основание
        /// </summary>
        public virtual ManOrgTransferContractFoundation ContractFoundation { get; set; }

        /// <summary>
        /// Файл расторжения
        /// </summary>
        public virtual FileInfo TerminationFile { get; set; }

        /// <summary>
        /// Сведения о плате - размер оплаты
        /// </summary>
        public virtual decimal? PaymentAmount { get; set; }

        /// <summary>
        /// Сведения о плате - протокол
        /// </summary>
        public virtual FileInfo PaymentProtocolFile { get; set; }

        /// <summary>
        /// Сведения о плате - описание протокола
        /// </summary>
        public virtual string PaymentProtocolDescription { get; set; }
    }
}