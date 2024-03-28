/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.PersonalAccount;
/// 
///     public class DebtorMap : BaseEntityMap<Debtor>
///     {
///         public DebtorMap() : base("REGOP_DEBTOR")
///         {
///             References(x => x.PersonalAccount, "ACCOUNT_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.PenaltyDebt, "PENALTY_DEBT", true);
///             Map(x => x.DebtSum, "DEBT_SUM", true);
///             Map(x => x.ExpirationDaysCount, "DAYS_COUNT", true);
///             Map(x => x.StartDate, "START_DATE");
///             Map(x => x.ExpirationMonthCount, "MONTH_COUNT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccount.Debtor"</summary>
    public class DebtorMap : BaseEntityMap<Debtor>
    {
        
        public DebtorMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccount.Debtor", "REGOP_DEBTOR")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.PersonalAccount, "PersonalAccount").Column("ACCOUNT_ID").NotNull().Fetch();
            this.Property(x => x.DebtSum, "DebtSum").Column("DEBT_SUM").NotNull();
            this.Property(x => x.DebtBaseTariffSum, "Сумма задолженности по базовому тарифу").Column("DEBT_BASE_TARIFF_SUM").NotNull();
            this.Property(x => x.DebtDecisionTariffSum, "Сумма задолженности по тарифу решения").Column("DEBT_DECISION_TARIFF_SUM").NotNull();
            this.Property(x => x.PenaltyDebt, "PenaltyDebt").Column("PENALTY_DEBT").NotNull();
            this.Property(x => x.ExpirationDaysCount, "ExpirationDaysCount").Column("DAYS_COUNT").NotNull();
            this.Property(x => x.ExpirationMonthCount, "ExpirationMonthCount").Column("MONTH_COUNT").NotNull();
            this.Property(x => x.StartDate, "Дата с которой идет отсчет времени").Column("START_DATE").NotNull();
            this.Property(x => x.CourtType, "Тип судебного учреждения").Column("COURT_TYPE");
            this.Property(x => x.ExtractExists, "Имеется выписка").Column("EXTRACT_EXISTS");
            this.Property(x => x.ExtractDate, "Дата выписки").Column("EXTRACT_DATE");
            this.Property(x => x.AccountRosregMatched, "Данные совпадают").Column("ROSREG_ACC_MATCHED");
            this.Property(x => x.ProcessedByTheAgent, "ЛС обрабатывается агентом").Column("PROCESSED_BY_AGENT");
            this.Reference(x => x.JurInstitution, "Cудебное учреждение").Column("JUR_INST_ID").Fetch();
            this.Property(x => x.ClaimworkId, "Имеется выписка").Column("claim_work_id");
            this.Property(x => x.LastClwDebt, "Имеется выписка").Column("LASTCLW_DEBT_SUM");
            this.Property(x => x.PaymentsSum, "Имеется выписка").Column("PAYMENTS_SUM");
            this.Property(x => x.MewClaimDebt, "Имеется выписка").Column("NEW_CLAIM_DEBT");
            this.Property(x => x.LastPirPeriod, "Имеется выписка").Column("LAST_DEBT_PERIOD");
        }
    }
}