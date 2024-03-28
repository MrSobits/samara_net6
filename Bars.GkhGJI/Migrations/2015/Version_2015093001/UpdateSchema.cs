namespace Bars.GkhGji.Migrations._2015.Version_2015093001
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015093000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("GJI_DICT_COURTVERDICT", "NAME", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GJI_DICT_COURTVERDICT SET NAME = '' WHERE NAME is NULL");

            Database.AlterColumnSetNullable("GJI_DICT_COURTVERDICT", "NAME", false);
        }
    }
}
