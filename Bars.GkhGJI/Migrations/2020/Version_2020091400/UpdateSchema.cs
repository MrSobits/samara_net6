namespace Bars.GkhGji.Migrations._2020.Version_2020091400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020091400")]
    [MigrationDependsOn(typeof(Version_2020090800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION_DEFINITION", new RefColumn("FILE_ID", "GJI_RESOLUTION_DEFINITION_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION_DEFINITION", "FILE_ID");
        }
    }
}