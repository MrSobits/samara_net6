namespace Bars.Gkh.Map
{
    using B4.Modules.Mapping.Mappers;
    using Entities;
    
    /// <summary>
    /// Маппинг для "Управление домами (ТСЖ / ЖСК)"
    /// </summary>
    public class ManOrgJskTsjContractMap : JoinedSubClassMap<ManOrgJskTsjContract>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ManOrgJskTsjContractMap() : 
                base("Управление домами (ТСЖ / ЖСК)", "GKH_MORG_JSKTSJ_CONTRACT")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.IsTransferredManagement, "Передано управление").Column("IS_TRANSFER_MANAGEMENT").NotNull();
            this.Reference(x => x.ManOrgTransferredManagement, "Управляющая организация (передано управление)").Column("MAN_ORG_TRANSFERRED_MANAG_ID").Fetch();
            this.Property(x => x.ProtocolNumber, "Номер протокола").Column("PROTOCOL_NUMBER");
            this.Property(x => x.ProtocolDate, "Дата протокола").Column("PROTOCOL_DATE");
            this.Reference(x => x.ProtocolFileInfo, "Файл протокола").Column("PROTOCOL_FILE_INFO_ID").Fetch();
            this.Property(x => x.InputMeteringDeviceValuesBeginDate, "День месяца начала ввода показаний по приборам учета").Column("INPUT_MDV_BEGIN_DATE");
            this.Property(x => x.InputMeteringDeviceValuesEndDate, "День месяца окончания ввода показаний по приборам учета").Column("INPUT_MDV_END_DATE");
            this.Property(x => x.DrawingPaymentDocumentDate, "День выставления платежных документов").Column("DRAWING_PD_DATE");
            this.Property(x => x.ContractFoundation, "Основание заключения договора").Column("CONTRACT_FOUNDATION").NotNull();
            this.Reference(x => x.TerminationFile, "Файл расторжения").Column("TERMINATION_FILE").Fetch();

            this.Property(x => x.CompanyReqiredPaymentAmount, "Размер обязательного платежа (члены товарищества)").Column("COMP_REQIRED_PAY_AMOUNT");
            this.Reference(x => x.CompanyPaymentProtocolFile, "Протокол (члены товарищества)").Column("COMP_PAYMENT_PROTOCOL");
            this.Property(x => x.CompanyPaymentProtocolDescription, "Описание протокола (члены товарищества)").Column("COMP_PAY_PROTOCOL_DESCR");
            this.Property(x => x.ReqiredPaymentAmount, "Размер обязательного платежа").Column("REQIRED_PAY_AMOUNT");
            this.Reference(x => x.PaymentProtocolFile, "Сведения о плате - протокол").Column("PAYMENT_PROTOCOL");
            this.Property(x => x.PaymentProtocolDescription, "Сведения о плате - описание протокола").Column("PAY_PROTOCOL_DESCR");
        }
    }
}
