namespace Bars.Gkh.Migrations._2015.Version_2015100500
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //WallMaterial
            Database.AlterColumnSetNullable("GKH_DICT_WALL_MATERIAL", "NAME", true);

            //TypeOwnership
            Database.AlterColumnSetNullable("GKH_DICT_TYPE_OWNERSHIP", "NAME", true);
        }

        public override void Down()
        {
            //WallMaterial
            Database.ExecuteNonQuery("UPDATE GKH_DICT_WALL_MATERIAL SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("GKH_DICT_WALL_MATERIAL", "NAME", false);

            //TypeOwnership
            Database.ExecuteNonQuery("UPDATE GKH_DICT_TYPE_OWNERSHIP SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("GKH_DICT_TYPE_OWNERSHIP", "NAME", false);
        }
    }
}
