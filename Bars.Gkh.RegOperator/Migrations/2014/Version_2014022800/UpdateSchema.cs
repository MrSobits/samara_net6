namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014022800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014022700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_UNACCEPT_PAY_PACKET",
                new Column("CREATE_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String));

            Database.AddEntityTable("REGOP_UNACCEPT_PAY",
                new RefColumn("PACKET_ID", ColumnProperty.NotNull, "REGOP_UNACCEPT_PAY_PCKT", "REGOP_UNACCEPT_PAY_PACKET", "ID"),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "REGOP_UNACCEPT_PAY_ACC", "REGOP_PERS_ACC", "ID"),
                new Column("ACCEPTED", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("PGUID", DbType.String, 40, ColumnProperty.NotNull),
                new Column("PAYMENT_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAYMENT_TYPE", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("DOC_NUMBER", DbType.String, 300),
                new Column("DOC_DATE", DbType.Date),
                new Column("PENALTY_SUM", DbType.Decimal));

            Database.AddColumn("REGOP_PERS_ACC_PAYMENT", new Column("PGUID", DbType.String, 40));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_UNACCEPT_PAY");

            Database.RemoveTable("REGOP_UNACCEPT_PAY_PACKET");

            Database.RemoveColumn("REGOP_PERS_ACC_PAYMENT", "PGUID");
        }
    }
}
