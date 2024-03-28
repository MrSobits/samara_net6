namespace Bars.Gkh.Migrations._2015.Version_2015093001
{
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015092900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("OVRHL_DICT_CEO_GROUP_TYPE", "GROUP_TYPE_CODE", true);
            Database.AlterColumnSetNullable("OVRHL_DICT_CEO_GROUP_TYPE", "NAME", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GKH_PUBLIC_SERV SET GROUP_TYPE_CODE = '' WHERE GROUP_TYPE_CODE is NULL");
            Database.ExecuteNonQuery("UPDATE GKH_PUBLIC_SERV SET NAME = '' WHERE NAME is NULL");

            Database.AlterColumnSetNullable("OVRHL_DICT_CEO_GROUP_TYPE", "GROUP_TYPE_CODE", false);
            Database.AlterColumnSetNullable("OVRHL_DICT_CEO_GROUP_TYPE", "NAME", false);
        }
    }
}
