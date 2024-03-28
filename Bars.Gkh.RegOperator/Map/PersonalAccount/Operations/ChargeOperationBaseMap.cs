namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    /// <summary>
    /// Маппинг для <see cref="ChargeOperationBase"/>
    /// </summary>
    public class ChargeOperationBaseMap : BaseImportableEntityMap<ChargeOperationBase>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ChargeOperationBaseMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.ChargeOperationBase", "REGOP_CHARGE_OPERATION_BASE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Document, "Документ-основание").Column("DOC_ID").Fetch();
            this.Property(x => x.OperationDate, "Дата операции").Column("OP_DATE").NotNull();
            this.Property(x => x.FactOperationDate, "Фактическая дата операции").Column("FACT_OP_DATE").NotNull();
            this.Property(x => x.Reason, "Причина").Column("REASON").Length(200);
            this.Property(x => x.User, "Логин пользователя").Column("USER_NAME").Length(100).NotNull();
            this.Property(x => x.ChargeSource, "Тип источника поступления начислений").Column("CHARGE_SOURCE").NotNull();
            this.Property(x => x.OriginatorGuid, "Guid инициатора").Column("GUID").Length(250).NotNull();
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
        }
    }
}
