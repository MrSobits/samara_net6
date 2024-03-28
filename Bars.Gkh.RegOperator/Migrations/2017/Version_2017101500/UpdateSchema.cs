namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017101500
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    using Dapper;

    [Migration("2017101500")]
    [MigrationDependsOn(typeof(Version_2017092700.UpdateSchema))]
    [MigrationDependsOn(typeof(Gkh.Migrations._2017.Version_2017101500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.UpdateDebtSum();
        }

        /// <inheritdoc />
        public override void Down()
        {
        }

        private void UpdateDebtSum()
        {
            #region SQL
            var selectDebtorSql = @"
SELECT 
    d.id as Id,
    a.account_id as AccountId,
    d.CUR_CHARGE_BASE_TARIFF_DEBT as CurrChargeBaseTariffDebt,
    d.CUR_CHARGE_DECISION_TARIFF_DEBT as CurrChargeDecisionTariffDebt,
    d.CUR_CHARGE_DEBT as CurrChargeDebt,
    d.ORIG_CHARGE_BASE_TARIFF_DEBT as OrigChargeBaseTariffDebt,
    d.ORIG_CHARGE_DECISION_TARIFF_DEBT as OrigChargeDecisionTariffDebt,
    d.ORIG_CHARGE_DEBT as OrigChargeDebt
FROM CLW_DEBTOR_CLAIM_WORK d
join CLW_CLAIM_WORK b on b.id=d.id
JOIN CLW_CLAIM_WORK_ACC_DETAIL a ON a.CLAIM_WORK_ID = d.id
where b.is_debt_paid =false;";

            var selectPeriodSummarySql = @"
SELECT
    s.id as Id, 
    s.ACCOUNT_ID as AccountId,
    s.BASE_TARIFF_DEBT as BaseTariffDebt,
    s.DEC_TARIFF_DEBT as DecisionTariffDebt,
    s.BASE_TARIFF_DEBT + s.DEC_TARIFF_DEBT as SumDebt
FROM REGOP_PERS_ACC_PERIOD_SUMM s
WHERE s.ACCOUNT_ID in (
    SELECT DISTINCT d.account_id
    FROM CLW_DEBTOR_CLAIM_WORK w
    JOIN CLW_CLAIM_WORK_ACC_DETAIL d ON d.CLAIM_WORK_ID = w.id
);
";
            var updateDebtorSql = @"
UPDATE CLW_DEBTOR_CLAIM_WORK
SET 
    CUR_CHARGE_BASE_TARIFF_DEBT = @CurrChargeBaseTariffDebt,
    CUR_CHARGE_DECISION_TARIFF_DEBT = @CurrChargeDecisionTariffDebt,
    ORIG_CHARGE_BASE_TARIFF_DEBT = @OrigChargeBaseTariffDebt,
    ORIG_CHARGE_DECISION_TARIFF_DEBT = @OrigChargeDecisionTariffDebt
WHERE id = @Id;
";
            var updateDebtorDetailSql = @"
UPDATE CLW_CLAIM_WORK_ACC_DETAIL d
SET     
    CUR_CHARGE_BASE_TARIFF_DEBT = w.CUR_CHARGE_BASE_TARIFF_DEBT,
    CUR_CHARGE_DECISION_TARIFF_DEBT = w.CUR_CHARGE_DECISION_TARIFF_DEBT,
    ORIG_CHARGE_BASE_TARIFF_DEBT = w.ORIG_CHARGE_BASE_TARIFF_DEBT,
    ORIG_CHARGE_DECISION_TARIFF_DEBT = w.ORIG_CHARGE_DECISION_TARIFF_DEBT
FROM CLW_DEBTOR_CLAIM_WORK w
WHERE d.CLAIM_WORK_ID = w.ID
AND d.ID in (
    SELECT max(d.id) FROM CLW_DEBTOR_CLAIM_WORK w
    JOIN CLW_CLAIM_WORK_ACC_DETAIL d ON d.CLAIM_WORK_ID = w.id
    GROUP BY d.account_id
    HAVING count(d.id) = 1
);

UPDATE CLW_LAWSUIT l
SET
    DEBT_BASE_TARIFF_SUM = d.CurrChargeBaseTariffDebt,
    DEBT_DECISION_TARIFF_SUM = d.CurrChargeDecisionTariffDebt
FROM (
    SELECT
        l.id,
        case when w.CUR_CHARGE_DEBT = l.debt_sum
            then w.CUR_CHARGE_BASE_TARIFF_DEBT
            else l.debt_sum
        end as CurrChargeBaseTariffDebt,
        case when w.CUR_CHARGE_DEBT = l.debt_sum
            then w.CUR_CHARGE_DECISION_TARIFF_DEBT
            else 0
        end as CurrChargeDecisionTariffDebt
    FROM CLW_LAWSUIT l
    JOIN CLW_DOCUMENT d ON d.id = l.id
    JOIN CLW_DEBTOR_CLAIM_WORK w ON d.claimwork_id = w.id
) d
WHERE l.id = d.id;

UPDATE CLW_PRETENSION p
SET
    DEBT_BASE_TARIFF_SUM = d.CurrChargeBaseTariffDebt,
    DEBT_DECISION_TARIFF_SUM = d.CurrChargeDecisionTariffDebt
FROM (
    SELECT
        p.id,
        case when w.CUR_CHARGE_DEBT = p.sum 
            then w.CUR_CHARGE_BASE_TARIFF_DEBT 
            else p.sum 
        end as CurrChargeBaseTariffDebt,
        case when w.CUR_CHARGE_DEBT = p.sum
            then w.CUR_CHARGE_DECISION_TARIFF_DEBT
            else 0
        end as CurrChargeDecisionTariffDebt
    FROM CLW_PRETENSION p
    JOIN CLW_DOCUMENT d ON d.id = p.id
    JOIN CLW_DEBTOR_CLAIM_WORK w ON d.claimwork_id = w.id
) d
WHERE p.id = d.id;
";
            #endregion

            var connection = this.Database.Connection;
            var debtors = connection.Query<DebtorInfo>(selectDebtorSql).ToList();
            var periodSummaryList = connection.Query<PeriodSummaryInfo>(selectPeriodSummarySql).ToList();

            var debtorDict = debtors.GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.Single(y => x.Count() == 1));

            var periodSummaryDict = periodSummaryList.GroupBy(x => x.AccountId)
                .ToDictionary(x => x.Key, x => x.ToList());

            foreach (var debtorInfo in debtorDict)
            {
                this.UpdateDebtorInfo(debtorInfo, periodSummaryDict.Get(debtorInfo.Key));
            }

            connection.Execute(updateDebtorSql, debtorDict.Values.ToArray());
            connection.Execute(updateDebtorDetailSql);
        }

        private void UpdateDebtorInfo(KeyValuePair<long, DebtorInfo> debtorInfo, List<PeriodSummaryInfo> priodSummaryInfo)
        {
            var origDebtInfo = priodSummaryInfo?
                .FirstOrDefault(x => x.SumDebt == debtorInfo.Value.OrigChargeDebt);

            var currDebtInfo = priodSummaryInfo?
                .FirstOrDefault(x => x.SumDebt == debtorInfo.Value.CurrChargeDebt);

            debtorInfo.Value.OrigChargeBaseTariffDebt = origDebtInfo?.BaseTariffDebt ?? debtorInfo.Value.OrigChargeDebt;
            debtorInfo.Value.OrigChargeDecisionTariffDebt = origDebtInfo?.DecisionTariffDebt ?? 0;
            debtorInfo.Value.CurrChargeBaseTariffDebt = currDebtInfo?.BaseTariffDebt ?? debtorInfo.Value.CurrChargeDebt;
            debtorInfo.Value.CurrChargeDecisionTariffDebt = currDebtInfo?.DecisionTariffDebt ?? 0;
        }

        private class DebtorInfo
        {
            public long Id { get; set; }
            public long AccountId { get; set; }
            public decimal CurrChargeBaseTariffDebt { get; set; }
            public decimal CurrChargeDecisionTariffDebt { get; set; }
            public decimal CurrChargeDebt { get; set; }
            public decimal OrigChargeBaseTariffDebt { get; set; }
            public decimal OrigChargeDecisionTariffDebt { get; set; }
            public decimal OrigChargeDebt { get; set; }
        }

        private class PeriodSummaryInfo
        {
            public long Id { get; set; }
            public long AccountId { get; set; }
            public decimal BaseTariffDebt { get; set; }
            public decimal DecisionTariffDebt { get; set; }
            public decimal SumDebt { get; set; }
        }
    }
}