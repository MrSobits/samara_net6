namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;


    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccount.AccountExcelSaldoChange"</summary>
    public class AccountExcelSaldoChangeMap : BaseEntityMap<AccountExcelSaldoChange>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AccountExcelSaldoChangeMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccount.AccountExcelSaldoChange", "REGOP_SALDO_CHANGE_EXPORT_PA")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.SaldoByBaseTariffBefore, "Сальдо по базовому тарифу до изменений").Column("SALDO_BASE_BEFORE").NotNull();
            this.Property(x => x.SaldoByDecisinTariffBefore, "Сальдо по тарифу решения до изменений").Column("SALDO_DEC_BEFORE").NotNull();
            this.Property(x => x.SaldoByPenaltyBefore, "Сальдо по пени до изменений").Column("SALDO_PENALTY_BEFORE").NotNull();

            this.Reference(x => x.SaldoChangeExport, "Экспорт, в рамках которого был выгружен данный ЛС").Column("CHANGE_ID").NotNull().Fetch();
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACC_ID").NotNull().Fetch();
        }
    }
}
