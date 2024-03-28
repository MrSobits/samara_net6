namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017102000
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017102000")]
    [MigrationDependsOn(typeof(Version_2017101500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable("CLW_DOCUMENT_ACC_DETAIL",
                new Column("DEBT_BASE_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DEBT_DECISION_TARIFF_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PENALTY_CALC_FORMULA", DbType.String, int.MaxValue, ColumnProperty.Null),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "CLW_DOCUMENT_ACC_DETAIL_ACCOUNT", "REGOP_PERS_ACC", "ID"),
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "CLW_DOCUMENT_ACC_DETAIL_DOCUMENT", "CLW_DOCUMENT", "ID")
            );

            ViewManager.Drop(this.Database, "Regop");
            //ViewManager.Create(this.Database, "Regop");

            this.MigrateData();
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("CLW_DOCUMENT_ACC_DETAIL");
        }

        private void MigrateData()
        {
            var sql = @"-- Заполнение детальной информации документов ПИР
INSERT INTO CLW_DOCUMENT_ACC_DETAIL (
    object_version,
    object_create_date,
    object_edit_date,
    document_id,
    account_id,
    debt_base_tariff_sum,
    debt_decision_tariff_sum,
    debt_sum,
    penalty_sum
)
SELECT
    0,
    now()::date,
    now()::date,
    doc.id as document_id,
    clw.account_id as account_id,
    clw.cur_charge_base_tariff_debt,
    clw.cur_charge_decision_tariff_debt,
    clw.cur_charge_debt,
    clw.cur_penalty_debt
FROM CLW_CLAIM_WORK_ACC_DETAIL clw
JOIN CLW_DOCUMENT doc ON doc.claimwork_id = clw.claim_work_id
WHERE TYPE_DOCUMENT IN (20, 30, 60); -- Bars.Gkh.Modules.ClaimWork.Enums.ClaimWorkDocumentType
";
            this.Database.ExecuteNonQuery(sql);
        }
    }
}