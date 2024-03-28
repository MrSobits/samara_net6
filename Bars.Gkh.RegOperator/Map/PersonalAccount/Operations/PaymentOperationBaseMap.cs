namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    /// <summary>
    /// Маппинг для <see cref="PaymentOperationBase"/>
    /// </summary>
    public class PaymentOperationBaseMap : BaseImportableEntityMap<PaymentOperationBase>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentOperationBaseMap() 
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.PaymentOperationBase", "REGOP_PAYMENT_OPERATION_BASE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOC_NUM").Length(200);
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOC_DATE");
            this.Property(x => x.OperationDate, "Дата операции").Column("OP_DATE").NotNull();
            this.Property(x => x.FactOperationDate, "Фактическая дата операции").Column("FACT_OP_DATE").NotNull();
            this.Property(x => x.Reason, "Причина").Column("REASON").Length(200);
            this.Property(x => x.User, "Логин пользователя").Column("USER_NAME").Length(100).NotNull();
            this.Property(x => x.PaymentSource, "Тип источника поступления").Column("PAYMENT_SOURCE").NotNull();
            this.Property(x => x.OriginatorGuid, "Guid инициатора").Column("GUID").Length(250).NotNull();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.Document, "Документ-основание").Column("DOC_ID").Fetch();
        }
    }
}