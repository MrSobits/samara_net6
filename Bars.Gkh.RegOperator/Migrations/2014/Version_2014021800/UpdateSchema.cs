namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "REGOP_PAYMENT_DOC_PRINT",
                new Column("DOC_YEAR", DbType.Int64, ColumnProperty.NotNull),
                new Column("DOC_NUMBER", DbType.Int64, ColumnProperty.NotNull),
                new Column("ACCOUNT_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("PERIOD_ID", DbType.Int64, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PAYMENT_DOC_PRINT");
        }
    }
}
