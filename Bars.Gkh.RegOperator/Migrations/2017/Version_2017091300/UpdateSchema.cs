namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017091300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017091300")]
    [MigrationDependsOn(typeof(Version_2017090800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            var sql = @"
DELETE FROM b4_role_permission
WHERE permission_id in (
    'Clw.ClaimWork.Legal.Columns.DebtSum',
    'Clw.ClaimWork.Legal.CrDebt.CurrDebtSum',
    'Clw.ClaimWork.Legal.CrDebt.OrigDebtSum',
    'Clw.ClaimWork.Individual.Columns.DebtSum',
    'Clw.ClaimWork.Individual.CrDebt.CurrDebtSum',
    'Clw.ClaimWork.Individual.CrDebt.OrigDebtSum',
    'Clw.ClaimWork.Debtor.Pretension.DebtSum',
    'Clw.ClaimWork.Debtor.CourtOrderApplication.DebtSum',
    'Clw.ClaimWork.Debtor.ClaimStatement.DebtSum',
    'GkhRegOp.PersonalAccountOwner.Debtor.Columns.DebtSum'
);
INSERT INTO b4_role_permission (permission_id, role_id) (
    SELECT DISTINCT
        unnest(array[
            'Clw.ClaimWork.Legal.Columns.DebtSum',
            'Clw.ClaimWork.Legal.CrDebt.CurrDebtSum',
            'Clw.ClaimWork.Legal.CrDebt.OrigDebtSum',
            'Clw.ClaimWork.Individual.Columns.DebtSum',
            'Clw.ClaimWork.Individual.CrDebt.CurrDebtSum',
            'Clw.ClaimWork.Individual.CrDebt.OrigDebtSum',
            'Clw.ClaimWork.Debtor.Pretension.DebtSum',
            'Clw.ClaimWork.Debtor.CourtOrderApplication.DebtSum',
            'Clw.ClaimWork.Debtor.ClaimStatement.DebtSum'
        ]),
        role_id
    FROM b4_role_permission
    WHERE permission_id LIKE 'Clw.ClaimWork%'
    UNION
    SELECT DISTINCT
        unnest(array[
            'GkhRegOp.PersonalAccountOwner.Debtor.Columns.DebtSum'
        ]),
        role_id
    FROM b4_role_permission
    WHERE permission_id LIKE 'GkhRegOp.PersonalAccountOwner.Debtor%'
);
";
            this.Database.ExecuteNonQuery(sql);
        }

        /// <inheritdoc/>
        public override void Down()
        {
        }
    }
}