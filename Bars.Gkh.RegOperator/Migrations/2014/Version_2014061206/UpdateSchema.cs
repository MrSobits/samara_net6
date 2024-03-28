namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061206
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061206")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061204.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CREDITORG_SERVICE_COND",
                new RefColumn("CREDITORG_ID", ColumnProperty.NotNull, "CREDIT_SERVE_COND", "OVRHL_CREDIT_ORG", "ID"),

                new Column("CASH_SERVICE_SIZE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CASH_SERVICE_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CAST_SERVICE_TO", DbType.DateTime),
                new Column("OPENING_ACC_PAY", DbType.Decimal, ColumnProperty.NotNull),
                new Column("OPENING_ACC_FROM", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OPENING_ACC_TO", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CREDITORG_SERVICE_CONDITION");
        }
    }
}
