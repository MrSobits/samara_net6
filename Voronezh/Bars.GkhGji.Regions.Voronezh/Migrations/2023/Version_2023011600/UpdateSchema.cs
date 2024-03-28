namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023011600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023011600")]
    [MigrationDependsOn(typeof(Version_2023011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("RESOLUTION_CONTENT", DbType.String, 500));
          
        }

        public override void Down()
        {         
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "RESOLUTION_CONTENT");
        }
    }
}