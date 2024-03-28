namespace Bars.GkhGji.Migrations._2015.Version_2015093000
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015092400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GJI_DICT_SANCTION", "NAME", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GJI_DICT_SANCTION SET NAME = '' WHERE NAME is NULL");

            this.Database.AlterColumnSetNullable("GJI_DICT_SANCTION", "NAME", false);
        }
    }
}
