namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_RO_SPEC_ACC",
                new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull),
                new Column("REG_OPERATOR", DbType.String, 1000, ColumnProperty.Null),
                new RefColumn("CA_ID", ColumnProperty.Null, "REGOP_ROSPECACC_CA", "GKH_CONTRAGENT", "ID"),
                new RefColumn("RO_CHARGE_ACC_ID", ColumnProperty.NotNull, "REGOP_ROSPECACC_CACC", "REGOP_RO_CHARGE_ACCOUNT", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_RO_SPEC_ACC");
        }
    }
}
