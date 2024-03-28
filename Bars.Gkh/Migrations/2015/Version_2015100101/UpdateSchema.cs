namespace Bars.Gkh.Migrations._2015.Version_2015100101
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("GKH_DICT_BUILDING_FEATURE", "CODE", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GKH_DICT_BUILDING_FEATURE SET CODE = '' WHERE CODE is NULL");
            Database.AlterColumnSetNullable("GKH_DICT_BUILDING_FEATURE", "CODE", false);
        }
    }
}
