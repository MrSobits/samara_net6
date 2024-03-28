namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014013000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_RO_SUPP_ACC",
                new Column("BALANCE", DbType.Decimal, ColumnProperty.Null),
                new Column("CLOSE_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("OPEN_DATE", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "RO_SUPP_ACC_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable("REGOP_RO_SUPP_ACC_OP",
                new Column("OP_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("CREDIT", DbType.Decimal, ColumnProperty.Null),
                new Column("DEBT",DbType.Decimal, ColumnProperty.Null),
                new Column("TYPE", DbType.String, 500, ColumnProperty.NotNull),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "REGOP_RO_SOP_ACC", "REGOP_RO_SUPP_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_RO_SUPP_ACC");
            Database.RemoveTable("REGOP_RO_SUPP_ACC_OP");
        }
    }
}
