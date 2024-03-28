namespace Bars.Gkh.Map
{
    using B4.Modules.Mapping.Mappers;
    using Entities;
    
    /// <summary>
    /// Маппинг для "Управление домом, договор УК с собственниками"
    /// </summary>
    public class ManOrgContractOwnersMap : JoinedSubClassMap<ManOrgContractOwners>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManOrgContractOwnersMap() : 
                base("Управление домом, договор УК с собственниками", "GKH_MORG_CONTRACT_OWNERS")
        {
        }

        /// <summary>
        /// Map
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ContractFoundation, "Основание договора ук с собственниками").Column("CONTRACT_FOUNDATION").NotNull();

            this.Property(x => x.DocumentDateOnRegistry, "Дата приказа включения в реестр").Column("DOCUMENT_DATE_ON_REGISTRY");
            this.Property(x => x.DocumentNumberOnRegistry, "Номер приказа включения в реестр").Column("DOCUMENT_NUMBER_ON_REGISTRY");
            this.Property(x => x.DocumentNumberOffRegistry, "Номер приказа исключения из реестра").Column("DOCUMENT_NUMBER_OFF_REGISTRY");
            this.Property(x => x.DocumentDateOffRegistry, "Дата приказа исключения из реестра").Column("DOCUMENT_DATE_OFF_REGISTRY");
            this.Property(x => x.InputMeteringDeviceValuesBeginDate, "День месяца начала ввода показаний по приборам учета").Column("INPUT_MDV_BEGIN_DATE").DefaultValue(0);
            this.Property(x => x.InputMeteringDeviceValuesEndDate, "День месяца окончания ввода показаний по приборам учета").Column("INPUT_MDV_END_DATE").DefaultValue(0);
            this.Property(x => x.DrawingPaymentDocumentDate, "День выставления платежных документов").Column("DRAWING_PD_DATE").DefaultValue(0);
            this.Property(x => x.ProtocolNumber, "Номер протокола").Column("PROTOCOL_NUMBER");
            this.Property(x => x.ProtocolDate, "Дата протокола").Column("PROTOCOL_DATE");
            this.Reference(x => x.ProtocolFileInfo, "Файл протокола").Column("PROTOCOL_FILE_INFO_ID").Fetch();
            this.Reference(x => x.TerminationFile, "Файл расторжения").Column("TERMINATION_FILE").Fetch();
            this.Reference(x => x.OwnersSignedContractFile, "Файл - реестр собственников подписавших договор").Column("OWNERS_SIGNED_CONTRACT_FILE").Fetch();

            this.Property(x => x.PaymentAmount, "Сведения о плате - размер оплаты").Column("PAYMENT_AMOUNT");
            this.Reference(x => x.PaymentProtocolFile, "Сведения о плате - протокол").Column("PAYMENT_PROTOCOL_FILE");
            this.Property(x => x.PaymentProtocolDescription, "Сведения о плате - описание протокола").Column("PAYMENT_PROTOCOL_DESCRIPTION");

            this.Property(x => x.DateLicenceRegister, "Дата внесения в реестр лицензий").Column("DATE_LIC_REG");
            this.Property(x => x.DateLicenceDelete, "Дата исключения из реестра лицензий").Column("DATE_LIC_DEL");
            this.Property(x => x.RegisterReason, "Основание включения").Column("REG_REASON");
            this.Property(x => x.DeleteReason, "Основание исключения").Column("DEL_REASON");
        }
    }
}
