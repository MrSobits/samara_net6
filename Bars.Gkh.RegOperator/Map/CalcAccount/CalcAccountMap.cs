namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Map;
    using Bars.B4.Modules.Mapping.Mappers;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Расчетный счет"</summary>
    public class CalcAccountMap : BaseEntityMap<CalcAccount>
    {

        public CalcAccountMap() :
                base("Расчетный счет", "REGOP_CALC_ACC")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.AccountNumber, "Номер счета").Column("ACCOUNT_NUMBER").Length(100);
            this.Reference(x => x.CreditOrg, "Кредитная организация").Column("CREDIT_ORG_ID").Fetch();
            this.Reference(x => x.AccountOwner, "Владелец счета (контрагент)").Column("ACCOUNT_OWNER_ID").Fetch();
            this.Property(x => x.DateOpen, "Дата открытия счета").Column("DATE_OPEN").NotNull();
            this.Property(x => x.DateClose, "Дата закрытия счета").Column("DATE_CLOSE");
            this.Property(x => x.TypeAccount, "Тип счета").Column("TYPE_ACCOUNT").DefaultValue(TypeCalcAccount.NotSet).NotNull();
            this.Property(x => x.TypeOwner, "Тип владельца счета").Column("TYPE_OWNER").DefaultValue(0).NotNull();
            this.Property(x => x.TotalOut, "Итого по расходу").Column("TOTAL_OUT").DefaultValue(0m).NotNull();
            this.Property(x => x.TotalIn, "Итого по приходу").Column("TOTAL_IN").DefaultValue(0m).NotNull();
            this.Property(x => x.Balance, "Сальдо по счету").Column("ABALANCE").DefaultValue(0m).NotNull();
            this.Property(x => x.BalanceIn, "Входящий баланс").Column("ABALANCE_IN").DefaultValue(0m).NotNull();
            this.Property(x => x.BalanceOut, "Исходящий баланс").Column("ABALANCE_OUT").DefaultValue(0m).NotNull();
            this.Property(x => x.TransferGuid, "Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer").Column("TRANSFER_GUID").Length(250).NotNull();
        }

        public class CalcAccountNhMapping : ClassMapping<CalcAccount>
        {
            public CalcAccountNhMapping()
            {
                this.Property(x => x.ExportId, m =>
                {
                    m.Insert(false);
                    m.Update(false);
                });
            }
        }
    }
}
