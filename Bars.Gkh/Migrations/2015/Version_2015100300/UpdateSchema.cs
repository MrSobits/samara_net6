namespace Bars.Gkh.Migrations._2015.Version_2015100300
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //TypeService
            Database.AlterColumnSetNullable("GKH_DICT_TYPE_SERVICE", "NAME", true);

            //TypeProject
            Database.AlterColumnSetNullable("GKH_DICT_TYPE_PROJ", "NAME", true);
        }

        public override void Down()
        {
            //TypeService
            Database.ExecuteNonQuery("UPDATE GKH_DICT_TYPE_SERVICE SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("GKH_DICT_TYPE_SERVICE", "NAME", false);

            //TypeProject
            Database.ExecuteNonQuery("UPDATE GKH_DICT_TYPE_PROJ SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("GKH_DICT_TYPE_PROJ", "NAME", false);
        }
    }
}

