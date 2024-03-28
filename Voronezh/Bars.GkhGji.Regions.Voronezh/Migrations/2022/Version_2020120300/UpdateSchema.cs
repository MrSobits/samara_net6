namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020120300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020120300")]
    [MigrationDependsOn(typeof(Version_2020120201.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
        "GJI_APPCIT_EXECUTION_TYPE",
        new RefColumn("APPCIT_ID", "GJI_APPCIT_EXECUTION_TYPE_APP_ID", "GJI_APPEAL_CITIZENS", "ID"),
        new RefColumn("TYPE_ID", "GJI_APPCIT_EXECUTION_TYPE_TYPE_ID", "GJI_DICT_APPEAL_EXECUTION_TYPE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_APPCIT_EXECUTION_TYPE");
        }

    }
}
