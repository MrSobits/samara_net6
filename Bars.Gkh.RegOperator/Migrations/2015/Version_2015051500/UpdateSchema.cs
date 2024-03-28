namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015051500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015051500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations.V_2015051400))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_SUSPENSE_ACCOUNT", new Column("REMAIN_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("REMAIN_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.ChangeColumn("REGOP_SUSPENSE_ACCOUNT", new Column("DCODE", DbType.String, 250));
            Database.ChangeColumn("REGOP_BANK_ACC_STMNT", new Column("DISTR_CODE", DbType.String, 250));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_SUSPENSE_ACCOUNT", "REMAIN_SUM");
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "REMAIN_SUM");
        }
    }
}
