namespace Bars.GkhGji.Migrations._2020.Version_2020091700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020091700")]
    [MigrationDependsOn(typeof(Version_2020091400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_DEFINITION", new RefColumn("FILE_ID", "GJI_PROTOCOL_DEFINITION_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_DEFINITION", "FILE_ID");
        }
    }
}