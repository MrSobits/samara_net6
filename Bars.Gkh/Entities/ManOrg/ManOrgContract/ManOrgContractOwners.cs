namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Управление домом, договор УК с собственниками
    /// </summary>
    public class ManOrgContractOwners : ManOrgBaseContract
    {
        /// <summary>
        /// Основание договора ук с собственниками
        /// </summary>
        public virtual ManOrgContractOwnersFoundation ContractFoundation { get; set; }

        /// <summary>
        /// Номер приказа включения в реестр
        /// </summary>
        public virtual string DocumentNumberOnRegistry { get; set; }

        /// <summary>
        /// Дата приказа включения в реестр
        /// </summary>
        public virtual DateTime? DocumentDateOnRegistry { get; set; }

        /// <summary>
        /// Номер приказа исключения из реестра
        /// </summary>
        public virtual string DocumentNumberOffRegistry { get; set; }

        /// <summary>
        /// Дата приказа исключения из реестра
        /// </summary>
        public virtual DateTime? DocumentDateOffRegistry { get; set; }

        /// <summary>
        /// Файл - реестр собственников подписавших договор
        /// </summary>
        public virtual FileInfo OwnersSignedContractFile { get; set; }

        /// <summary>
        /// День месяца начала ввода показаний по приборам учета
        /// </summary>
        public virtual byte? InputMeteringDeviceValuesBeginDate { get; set; }


        /// <summary>
        /// День месяца окончания ввода показаний по приборам учета
        /// </summary>
        public virtual byte? InputMeteringDeviceValuesEndDate { get; set; }

        /// <summary>
        /// День выставления платежных документов
        /// </summary>
        public virtual byte? DrawingPaymentDocumentDate { get; set; }

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

        /// <summary>
        /// Дата внесения в реестр лицензий
        /// </summary>
        public virtual DateTime? DateLicenceRegister { get; set; }

        /// <summary>
        /// Дата исключения из реестра лицензий
        /// </summary>
        public virtual DateTime? DateLicenceDelete { get; set; }

        /// <summary>
        /// Основание включения
        /// </summary>
        public virtual string RegisterReason { get; set; }

        /// <summary>
        /// Основание исключения
        /// </summary>
        public virtual string DeleteReason { get; set; }
    }
}