namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020022000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020022000")]
    [MigrationDependsOn(typeof(Version_2019121700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {       
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("INSPECTION_GUID", DbType.String));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("INSPECTOR_GUID", DbType.String));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("OBJECT_GUID", DbType.String));
            Database.AddColumn("GJI_CH_GIS_ERP", new Column("RESULT_GUID", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_ERP", "RESULT_GUID");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "OBJECT_GUID");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "INSPECTOR_GUID");
            Database.RemoveColumn("GJI_CH_GIS_ERP", "INSPECTION_GUID");
        }
    }
}


