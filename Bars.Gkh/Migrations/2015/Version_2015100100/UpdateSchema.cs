namespace Bars.Gkh.Migrations._2015.Version_2015100100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015093003.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GKH_DICT_ROOFING_MATERIAL", "NAME", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GKH_DICT_ROOFING_MATERIAL SET NAME = '' WHERE NAME is NULL");
            this.Database.AlterColumnSetNullable("GKH_DICT_ROOFING_MATERIAL", "NAME", false);
        }
    }
}
