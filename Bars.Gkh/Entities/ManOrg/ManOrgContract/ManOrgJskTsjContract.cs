namespace Bars.Gkh.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using Enums;

    /// <summary>
    /// Управление домами (ТСЖ / ЖСК)
    /// </summary>
    public class ManOrgJskTsjContract : ManOrgBaseContract
    {
        /// <summary>
        /// Управляющая организация (передано управление)
        /// </summary>
        [ObsoleteAttribute("ManOrgTransferredManagement has been deprecated. Use ManOrgContractRelation.")]
        public virtual ManagingOrganization ManOrgTransferredManagement { get; set; }

        /// <summary>
        /// Передано управление
        /// </summary>
        [ObsoleteAttribute("ManOrgTransferredManagement has been deprecated. Use ManOrgContractRelation.")]
        public virtual YesNoNotSet IsTransferredManagement { get; set; }

        /// <summary>
        /// Номер протокола
        /// </summary>
        public virtual string ProtocolNumber { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        public virtual DateTime? ProtocolDate { get; set; }

        /// <summary>
        /// Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFileInfo { get; set; }

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
        /// Основание договора
        /// </summary>
        public virtual ManOrgJskTsjContractFoundation ContractFoundation { get; set; }

        /// <summary>
        /// Файл расторжения
        /// </summary>
        public virtual FileInfo TerminationFile { get; set; }

        /// <summary>
        /// Сведения о плате - размер обязательного платежа (члены товарищества)
        /// </summary>
        public virtual decimal CompanyReqiredPaymentAmount { get; set; }

        /// <summary>
        /// Сведения о плате - протокол (члены товарищества)
        /// </summary>
        public virtual FileInfo CompanyPaymentProtocolFile { get; set; }

        /// <summary>
        /// Сведения о плате - описание протокола (члены товарищества)
        /// </summary>
        public virtual string CompanyPaymentProtocolDescription { get; set; }

        /// <summary>
        /// Сведения о плате - размер обязательного платежа
        /// </summary>
        public virtual decimal? ReqiredPaymentAmount { get; set; }

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