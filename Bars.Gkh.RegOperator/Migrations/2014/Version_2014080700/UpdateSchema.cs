namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014080700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014080102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_CANCEL_PAYMENT",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_CANCEL_PAYMENT_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "REGOP_CANCEL_PAYMENT_DOC", "B4_FILE_INFO", "ID"),
                new Column("DOWN_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("OPERATION_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, 500, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_CANCEL_PAYMENT");
        }
    }
}
