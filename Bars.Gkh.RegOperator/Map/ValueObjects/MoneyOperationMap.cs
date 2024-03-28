namespace Bars.Gkh.RegOperator.Map.ValueObjects
{
    using System;

    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>Маппинг для "Операция, в рамках которой могут происходить различные движения денег"</summary>
    public class MoneyOperationMap : BaseEntityMap<MoneyOperation>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public MoneyOperationMap() : 
                base("Операция, в рамках которой могут происходить различные движения денег", "REGOP_MONEY_OPERATION")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.CanceledOperation, "Операция, которая была отменена данной операцией").Column("CANCELED_OP_ID");
            this.Reference(x => x.Period, "Период операции").Column("PERIOD_ID");
            this.Property(x => x.OperationGuid, "Гуид операции").Column("OP_GUID").Length(250);
            this.Property(x => x.OriginatorGuid, "Гуид инициатора").Column("ORIGINATOR_GUID").Length(250);
            this.Property(x => x.IsCancelled, "IsCancelled").Column("IS_CANCELLED");
            this.Property(x => x.Amount, "Сумма перевода").Column("AMOUNT");
            this.Property(x => x.Reason, "Причина перевода").Column("REASON").Length(250);
            this.Reference(x => x.Document, "Документ операции").Column("DOCUMENT_ID").Fetch();
            this.Property(x => x.OperationDate, "Дата операции").Column("OPERATION_DATE").DefaultValue(DateTime.MinValue).NotNull();
            this.Property(x => x.UserLogin, "Логин пользователя").Column("USER_LOGIN").Length(100);
        }
    }
}
