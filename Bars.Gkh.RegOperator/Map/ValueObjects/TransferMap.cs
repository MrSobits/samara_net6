namespace Bars.Gkh.RegOperator.Map.ValueObjects
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Маппинг для "Трансфер между источником и получателем денег"
    /// </summary>
    /// <typeparam name="TTransfer"> Тип трансфера </typeparam>
    /// <typeparam name="TOwner"> Тип владельца трансфера </typeparam>
    public abstract class TransferMap<TTransfer, TOwner> : TransferMap<TTransfer>
        where TTransfer : TransferWithOwner<TOwner>
        where TOwner : class, ITransferOwner
    {
        /// <inheritdoc />
        protected TransferMap(string entityName, string tableName) : base(entityName, tableName)
        {
        }

        /// <inheritdoc />
        public override void InitMap()
        {
            base.InitMap();
            this.Reference(x => x.Owner, "Владелец трансфера").Column("OWNER_ID").NotNull();
        }
    }

    /// <summary>
    /// Маппинг для "Трансфер между источником и получателем денег"
    /// </summary>
    /// <typeparam name="TTransfer"> Тип трансфера </typeparam>
    public abstract class TransferMap<TTransfer> : BaseEntityMap<TTransfer> where TTransfer : Transfer
    {
        /// <inheritdoc />
        protected TransferMap(string entityName, string tableName) : base(entityName, tableName)
        {
        }

        /// <inheritdoc />
        public override void InitMap()
        {
            base.InitMap();

            this.Property(x => x.SourceGuid, "Источник денег").Column("SOURCE_GUID").Length(250);
            this.Property(x => x.TargetGuid, "Получатель денег").Column("TARGET_GUID").Length(250);
            this.Property(x => x.TargetCoef, "Коэффициент суммы у получателя").Column("TARGET_COEF");
            this.Property(x => x.Amount, "Сумма перевода").Column("AMOUNT");
            this.Property(x => x.Reason, "Причина перевода").Column("REASON").Length(250);
            this.Property(x => x.OriginatorName, "Плательщик/получатель/основание").Column("ORIGINATOR_NAME").Length(150);
            this.Property(x => x.PaymentDate, "Дата фактической оплаты. Важно знать, когда оплата садится задним числом").Column("PAYMENT_DATE");
            this.Property(x => x.OperationDate, "Дата операции").Column("OPERATION_DATE");
            this.Property(x => x.IsInDirect, "Признак того, что трансфер является \"транзитным\". Т.е. сначала трансфер произошел на целевой кошелек, а затем просто закинулся на другой").Column("IS_INDIRECT");
            this.Property(x => x.IsAffect, "Влияющий на баланс").Column("IS_AFFECT");
            this.Property(x => x.IsLoan, "Является займом").Column("IS_LOAN");
            this.Property(x => x.IsReturnLoan, "Является возвратом займа").Column("IS_RETURN_LOAN");

            this.Reference(x => x.ChargePeriod, "Период расчета").Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.Operation, "Операция, в рамках которой проводился трансфер").Column("OP_ID").NotNull().Fetch();
        }
    }
}
