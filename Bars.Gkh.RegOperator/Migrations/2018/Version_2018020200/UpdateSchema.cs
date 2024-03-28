namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018020200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018020200")]
    [MigrationDependsOn(typeof(Version_2018011800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            ViewManager.Drop(this.Database, "Regop");
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", new Column("DEBTOR_STATE", DbType.Int32, ColumnProperty.NotNull, 0));
            var sql = @"WITH final as (
                    SELECT d.id FROM clw_debtor_claim_work d
                    JOIN clw_claim_work w ON w.id = d.id
                    JOIN b4_state s ON s.id = w.state_id
                    WHERE s.final_state
                )
                UPDATE clw_debtor_claim_work w
                SET debtor_state = 100
                FROM final WHERE final.id = w.id AND w.debtor_state <> 100;

                WITH start as (
                    SELECT d.id, (SELECT id FROM b4_state WHERE type_id = 'clw_debtor_claim_work' AND start_state limit 1) as state_id
                    FROM clw_debtor_claim_work d
                    JOIN clw_claim_work w ON w.id = d.id
                    JOIN b4_state s ON s.id = w.state_id
                    WHERE s.final_state is not true
                )
                UPDATE clw_claim_work w
                SET state_id = start.state_id
                FROM start WHERE start.id = w.id AND start.state_id <> w.state_id;";

            this.Database.ExecuteNonQuery(sql);

            //ViewManager.Create(this.Database, "Regop");
        }

        /// <inheritdoc/>
        public override void Down()
        {
        }
    }
}