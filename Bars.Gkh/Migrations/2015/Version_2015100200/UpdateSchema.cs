namespace Bars.Gkh.Migrations._2015.Version_2015100200
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("GKH_DICT_LIC_PROVDOC", "NAME", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GKH_DICT_LIC_PROVDOC SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("GKH_DICT_LIC_PROVDOC", "NAME", false);
        }
    }
}
