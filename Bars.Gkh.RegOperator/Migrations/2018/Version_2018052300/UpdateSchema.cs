namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018052300
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018052300")]
    [MigrationDependsOn(typeof(Version_2018020200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            var sql = @"
CREATE TEMP TABLE regop_pers_acc_ownership_history_to_delete as (
    SELECT max(id) as id
    FROM regop_pers_acc_ownership_history
    GROUP BY account_id, date
);

DELETE FROM regop_pers_acc_ownership_history h WHERE NOT EXISTS (
    SELECT id FROM regop_pers_acc_ownership_history_to_delete d WHERE d.id = h.id
);
";
            ViewManager.Drop(this.Database, "Regop");
            this.Database.ExecuteNonQuery(sql);
            ViewManager.Create(this.Database, "Regop");

        }
        public override void Down()
        {
        }
    }
}