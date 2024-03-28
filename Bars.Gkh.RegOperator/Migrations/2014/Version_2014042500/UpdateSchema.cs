namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014042500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014042300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_RENT_PAYMENT_IN",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_RENT_PAYMENTIN_ACC", "REGOP_PERS_ACC", "ID"),
                new Column("OPERATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("P_GUID", DbType.String, 40, ColumnProperty.NotNull));

            Database.AddEntityTable("REGOP_ACCUMULATED_FUNDS",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_ACCUM_FUNDS_ACC", "REGOP_PERS_ACC", "ID"),
                new Column("OPERATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACCUMULATED_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("P_GUID", DbType.String, 40, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_RENT_PAYMENT_IN");

            Database.RemoveTable("REGOP_ACCUMULATED_FUNDS");
        }
    }
}
