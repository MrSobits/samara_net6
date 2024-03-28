namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_UNACCEPT_PAY_PACKET", new Column("STATE", DbType.Int16));
            Database.AddColumn("REGOP_UNACCEPT_PAY_PACKET", new Column("PACKET_SUM", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_PAY_PACKET", "STATE");
            Database.RemoveColumn("REGOP_UNACCEPT_PAY_PACKET", "PACKET_SUM");
        }
    }
}
