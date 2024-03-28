namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ExecutionAction;

    using NHibernate;

    /// <summary>
    /// ПИР - Настройка прав доступа для полей суммы задолженности
    /// </summary>
    public class DebtorSumPermissionAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "ПИР - Настройка прав доступа для полей суммы задолженности";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <inheritdoc />
        public override string Description => "Действие массово настраивает тип отображения суммы задолженностей "
            + "в реестре ПИР и реестре должников для всех ролей, имеющих доступ к данным разделам";

        private BaseDataResult Execute()
        {
            var defaultClaimWorkPermission = new[]
            {
                "Clw.ClaimWork.Legal.Columns.DebtSum",
                "Clw.ClaimWork.Legal.CrDebt.CurrDebtSum",
                "Clw.ClaimWork.Legal.CrDebt.OrigDebtSum",
                "Clw.ClaimWork.Individual.Columns.DebtSum",
                "Clw.ClaimWork.Individual.CrDebt.CurrDebtSum",
                "Clw.ClaimWork.Individual.CrDebt.OrigDebtSum",
                "Clw.ClaimWork.Debtor.Pretension.DebtSum",
                "Clw.ClaimWork.Debtor.CourtOrderApplication.DebtSum",
                "Clw.ClaimWork.Debtor.ClaimStatement.DebtSum"
            };
            var defaultDebtorPermission = new[]
            {
                "GkhRegOp.PersonalAccountOwner.Debtor.Columns.DebtSum"
            };
            var baseAndDecisionClaimWorkPermission = new[]
            {
                "Clw.ClaimWork.Legal.Columns.BaseTariffDebtSum",
                "Clw.ClaimWork.Legal.Columns.DecisionTariffDebtSum",
                "Clw.ClaimWork.Legal.CrDebt.CurrBaseTariffDebtSum",
                "Clw.ClaimWork.Legal.CrDebt.CurrDecisionTariffDebtSum",
                "Clw.ClaimWork.Legal.CrDebt.OrigBaseTariffDebtSum",
                "Clw.ClaimWork.Legal.CrDebt.OrigDecisionTariffDebtSum",
                "Clw.ClaimWork.Individual.Columns.BaseTariffDebtSum",
                "Clw.ClaimWork.Individual.Columns.DecisionTariffDebtSum",
                "Clw.ClaimWork.Individual.CrDebt.CurrBaseTariffDebtSum",
                "Clw.ClaimWork.Individual.CrDebt.CurrDecisionTariffDebtSum",
                "Clw.ClaimWork.Individual.CrDebt.OrigBaseTariffDebtSum",
                "Clw.ClaimWork.Individual.CrDebt.OrigDecisionTariffDebtSum",
                "Clw.ClaimWork.Debtor.Pretension.BaseTariffDebtSum",
                "Clw.ClaimWork.Debtor.Pretension.DecisionTariffDebtSum",
                "Clw.ClaimWork.Debtor.CourtOrderApplication.BaseTariffDebtSum",
                "Clw.ClaimWork.Debtor.CourtOrderApplication.DecisionTariffDebtSum",
                "Clw.ClaimWork.Debtor.ClaimStatement.BaseTariffDebtSum",
                "Clw.ClaimWork.Debtor.ClaimStatement.DecisionTariffDebtSum"
            };
            var baseAndDecisionDebtorPermission = new[]
            {
                "GkhRegOp.PersonalAccountOwner.Debtor.Columns.BaseTariffDebtSum",
                "GkhRegOp.PersonalAccountOwner.Debtor.Columns.DecisionTariffDebtSum"
            };

            var deleteSql = @"
DELETE FROM b4_role_permission
WHERE permission_id in (
    :permission_list
);
";
            var insertSql = @"
INSERT INTO b4_role_permission (permission_id, role_id) (
    SELECT DISTINCT
        unnest(array[
            :claimwork_permission_list
        ]),
        role_id
    FROM b4_role_permission
    WHERE permission_id LIKE 'Clw.ClaimWork%'
    UNION
    SELECT DISTINCT
        unnest(array[
            :debtor_permission_list
        ]),
        role_id
    FROM b4_role_permission
    WHERE permission_id LIKE 'GkhRegOp.PersonalAccountOwner.Debtor%'
);
";
            var updateRows = 0;
            var isDetail = this.ExecutionParams.Params.GetAs("IsDetail", false, true);
            this.Container.UsingForResolved<ISessionProvider>((container, provider) =>
            {
                using (var session = provider.OpenStatelessSession())
                {
                    var allPermission = defaultClaimWorkPermission
                        .Union(defaultDebtorPermission)
                        .Union(baseAndDecisionClaimWorkPermission)
                        .Union(baseAndDecisionDebtorPermission);

                    session.CreateSQLQuery(deleteSql)
                        .SetParameterList("permission_list", allPermission)
                        .ExecuteUpdate();

                    IQuery insertQuery = session.CreateSQLQuery(insertSql);
                    if (isDetail)
                    {
                        insertQuery = insertQuery
                            .SetParameterList("claimwork_permission_list", baseAndDecisionClaimWorkPermission)
                            .SetParameterList("debtor_permission_list", baseAndDecisionDebtorPermission);
                    }
                    else
                    {
                        insertQuery = insertQuery
                            .SetParameterList("claimwork_permission_list", defaultClaimWorkPermission)
                            .SetParameterList("debtor_permission_list", defaultDebtorPermission);
                    }

                    updateRows = insertQuery.ExecuteUpdate();
                }
            });

            return new BaseDataResult(true, $"Обновлено строк: {updateRows}");
        }
    }
}