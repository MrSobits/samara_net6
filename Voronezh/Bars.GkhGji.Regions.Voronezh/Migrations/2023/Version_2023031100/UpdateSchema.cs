namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023031100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023031100")]
    [MigrationDependsOn(typeof(Version_2023013000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL_OSP_REQUEST", new Column("ROOM", DbType.String, ColumnProperty.None));          
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL_OSP_REQUEST", "ROOM");         
        }
    }
}