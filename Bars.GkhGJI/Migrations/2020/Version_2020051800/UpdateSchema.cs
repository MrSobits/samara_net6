namespace Bars.GkhGji.Migrations._2020.Version_2020051800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020051800")]
    [MigrationDependsOn(typeof(Version_2020050700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_RESOLPROS", new  RefColumn("PROS_ID", "FK_GJI_RESOLPROS_PROS", "GJI_DICT_PROSECUTOR_OFFICE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLPROS", "PROS_ID");
        }
    }
}