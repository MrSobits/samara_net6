namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092501
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("update regop_suspense_account set c_guid = uuid_generate_v4()::text where c_guid is null or c_guid = ''");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("update regop_suspense_account set c_guid = RAWTOHEX(sys_guid()) where c_guid is null");
            }
        }

        public override void Down()
        {
        }
    }
}
