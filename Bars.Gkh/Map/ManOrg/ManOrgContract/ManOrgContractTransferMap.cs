namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Управление домами, договор УК с ЖСК/ТСЖ"</summary>
    public class ManOrgContractTransferMap : JoinedSubClassMap<ManOrgContractTransfer>
    {
        
        public ManOrgContractTransferMap() : 
                base("Управление домами, договор УК с ЖСК/ТСЖ", "GKH_MORG_CONTRACT_JSKTSJ")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ManOrgJskTsj, "ЖСК/ТСЖ").Column("MAN_ORG_JSK_TSJ_ID").NotNull().Fetch();
            this.Property(x => x.InputMeteringDeviceValuesBeginDate, "День месяца начала ввода показаний по приборам учета")
                .Column("INPUT_MDV_BEGIN_DATE");
            this.Property(x => x.InputMeteringDeviceValuesEndDate, "День месяца окончания ввода показаний по приборам учета")
                .Column("INPUT_MDV_END_DATE");
            this.Property(x => x.DrawingPaymentDocumentDate, "День выставления платежных документов")
                .Column("DRAWING_PD_DATE");
            this.Property(x => x.ProtocolNumber, "Номер протокола").Column("PROTOCOL_NUMBER");
            this.Property(x => x.ProtocolDate, "Дата протокола").Column("PROTOCOL_DATE");
            this.Reference(x => x.ProtocolFileInfo, "Файл протокола").Column("PROTOCOL_FILE_INFO_ID").Fetch();
            this.Property(x => x.ContractFoundation, "Основание").Column("CONTRACT_FOUNDATION").NotNull();
            this.Reference(x => x.TerminationFile, "Файл расторжения").Column("TERMINATION_FILE").Fetch();

            this.Property(x => x.PaymentAmount, "Сведения о плате - размер оплаты").Column("PAYMENT_AMOUNT");
            this.Reference(x => x.PaymentProtocolFile, "Сведения о плате - протокол").Column("PAYMENT_PROTOCOL_FILE");
            this.Property(x => x.PaymentProtocolDescription, "Сведения о плате - описание протокола").Column("PAYMENT_PROTOCOL_DESCRIPTION");
        }
    }
}
