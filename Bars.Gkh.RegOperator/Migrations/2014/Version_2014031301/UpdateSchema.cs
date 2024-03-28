namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031301
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "REGOP_IMPORTED_PAYMENT",
                new RefColumn("BANK_DOC_ID", ColumnProperty.NotNull, "REGOP_PAYMENT_DOC_IMP", "REGOP_BANK_DOC_IMPORT", "ID"),
                new Column("PAYMENT_ACCOUNT", DbType.String, 50, ColumnProperty.Null),
                new Column("PAYMENT_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("PAYMENT_SUM", DbType.Decimal),
                new Column("PAYMENT_DATE", DbType.Date),
                new Column("PAYMENT_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_IMPORTED_PAYMENT");
        }
    }
}
