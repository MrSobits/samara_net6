namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014040800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Enums;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014040400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_UNACCEPT_C_PACKET",
                new Column("PACKET_STATE", DbType.Int32, ColumnProperty.NotNull,
                    PaymentOrChargePacketState.Pending.GetHashCode().ToString()));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_C_PACKET", "PACKET_STATE");
        }
    }
}
