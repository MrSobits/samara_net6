namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_TRANSIT_ACC",
                new RefColumn("PAYMENT_AGENT_ID", ColumnProperty.NotNull, "REGOP_TRANSIT_ACC_PAG", "GKH_PAYMENT_AGENT", "ID"),
                new Column("R_NUMBER", DbType.String, 200),
                new Column("R_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("R_SUM", DbType.Decimal, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_TRANSIT_ACC");
        }
    }
}
