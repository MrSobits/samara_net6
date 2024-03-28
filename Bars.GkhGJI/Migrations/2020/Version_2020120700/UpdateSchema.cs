namespace Bars.GkhGji.Migrations._2020.Version_2020120700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020120700")]
    [MigrationDependsOn(typeof(Version_2020120200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_PLANJURPERSON", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GJI_DICT_PLANJURPERSON", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));

            Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "GIS_GKH_GUID");

            Database.RemoveColumn("GJI_DICT_PLANJURPERSON", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GJI_DICT_PLANJURPERSON", "GIS_GKH_GUID");
        }
    }
}