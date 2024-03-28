namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Импортируемая оплата"</summary>
    public class ImportedPaymentMap : BaseImportableEntityMap<ImportedPayment>
    {
        
        public ImportedPaymentMap() : 
                base("Импортируемая оплата", "REGOP_IMPORTED_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Account, "Счет").Column("PAYMENT_ACCOUNT");
            this.Property(x => x.PaymentType, "Тип оплаты").Column("PAYMENT_TYPE").NotNull();
            this.Property(x => x.Sum, "Сумма").Column("PAYMENT_SUM");
            this.Property(x => x.PaymentNumberUs, "Номер платежа в Системе ПА").Column("PAYMENT_NUM_US");
            this.Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            this.Property(x => x.PaymentState, "Статус оплаты").Column("PAYMENT_STATE").NotNull();
            this.Property(x => x.PaymentId, "Идентификатор созданной сущности").Column("PAYMENT_ID");
            this.Property(x => x.ReceiverNumber, "Р/С получателя (файл)").Column("RECEIVER_NUMBER");
            this.Property(x => x.AddressByImport, "Адрес (файл)").Column("ADDRESS_BY_IMPORT").Length(500);
            this.Property(x => x.OwnerByImport, "Абонент (файл)").Column("OWNER_BY_IMPORT").Length(250);
            this.Property(x => x.ExternalTransaction, "Идентификатор внешней транзакции если есть").Column("EXTERNAL_TRANSACTION").Length(64);
            this.Property(x => x.Accepted, "Подтверждено").Column("ACCEPTED").NotNull();
            this.Property(x => x.AcceptDate, "Дата подтверждения").Column("ACCEPT_DATE");
            this.Property(x => x.PersonalAccountDeterminationState, "Статус определения ЛС").Column("PAD_STATE").NotNull();
            this.Property(x => x.PaymentConfirmationState, "Статус подтверждения оплат").Column("PC_STATE").NotNull();
            this.Property(x => x.FactReceiverNumber, "Р/С получателя (в системе)").Column("FACT_RECEIVER_NUMBER").Length(100);
            this.Property(x => x.ExternalAccountNumber, "Лицевой Счет (во внешней системе) (файл)").Column("EXT_ACC_NUMBER").Length(100);
            this.Property(x => x.IsDeterminateManually, "ЛС сопоставлен вручную").Column("IS_DETERMINATE_MANUALLY").NotNull().DefaultValue(false);
            this.Property(x => x.PersonalAccountNotDeterminationStateReason, "Причина несоответствия ЛС").Column("PAND_STATE_REASON");

            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("PERS_ACC_ID").Fetch();
            this.Reference(x => x.BankDocumentImport, "Документ, загруженный из банка").Column("BANK_DOC_ID").Fetch();
        }
    }
}
