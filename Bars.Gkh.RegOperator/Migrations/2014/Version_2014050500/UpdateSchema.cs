namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014050500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014042901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery("update REGOP_UNACCEPT_PAY_PACKET set STATE = 20 where state = 0 or state is null");

            Database.ExecuteNonQuery("update REGOP_UNACCEPT_C_PACKET set packet_state = 20 where packet_state=0 or packet_state is null");
        }

        public override void Down()
        {
            
        }
    }
}
