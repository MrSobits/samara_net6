namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    /// <summary>
    /// Маппинг для <see cref="PaymentCorrection"/>
    /// </summary>
    public class PaymentCorrectionMap : BaseImportableEntityMap<PaymentCorrection>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentCorrectionMap() 
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.PaymentCorrection", "REGOP_PAYMENT_CORRECTION")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.PaymentType, "Тип оплат").Column("PAYMENT_TYPE").NotNull();
            this.Property(x => x.TakeAmount, "Снятие").Column("TAKE_AMOUNT").NotNull();
            this.Property(x => x.EnrollAmount, "Зачисление").Column("ENROLL_AMOUNT").NotNull();
            this.Reference(x => x.PaymentOperation, "Операция оплаты").Column("PAYMENT_OP_ID").NotNull().Fetch();
        }
    }
}