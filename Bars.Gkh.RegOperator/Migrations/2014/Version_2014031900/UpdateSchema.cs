namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031802.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REG_OP_SUSPEN_ACCOUNT", new Column("REASON", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_PAY_PACKET", "REASON");
        }
    }
}
