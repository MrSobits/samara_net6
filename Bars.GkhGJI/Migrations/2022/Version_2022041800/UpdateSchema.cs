namespace Bars.GkhGji.Migrations._2022.Version_2022041800
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022041800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022030800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {           

            Database.AddRefColumn("GJI_DICISION", new RefColumn("PROSECUTOR_ID", "GJI_DICISION_PROSECUTOR_ID", "GJI_DICT_PROSECUTOR_OFFICE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICISION", "PROSECUTOR_ID");
        }
    }
}