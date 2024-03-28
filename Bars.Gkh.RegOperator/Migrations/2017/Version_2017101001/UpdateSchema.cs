namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017101001
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017101001")]
    [MigrationDependsOn(typeof(Version_2017100400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "IS_BASE", DbType.Boolean, ColumnProperty.NotNull, true);
            this.Database.AddColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "ACCOUNT_COUNT", DbType.Int32, ColumnProperty.NotNull, 0);

            this.Database.ExecuteNonQuery(@"
                      update REGOP_PAYMENT_DOC_SNAPSHOT s
                      set account_count = p.count
                      from (select snapshot_id,count(*) count
                      from REGOP_PERS_PAYDOC_SNAP
                      group by snapshot_id) p
                      where p.snapshot_id=s.id;");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "IS_BASE");
            this.Database.RemoveColumn("REGOP_PAYMENT_DOC_SNAPSHOT", "ACCOUNT_COUNT");
        }
    }
}