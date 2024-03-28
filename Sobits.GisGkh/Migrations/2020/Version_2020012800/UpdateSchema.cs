namespace Sobits.GisGkh.Migrations._2020.Version_2020012800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2020012800")]
    [MigrationDependsOn(typeof(Version_2020012100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn(
                GisGkhPayDocMap.TableName,
                new Column("GIS_GKH_GUID", DbType.String, 36)
                );
        }

        public override void Down()
        {
            Database.RemoveColumn(GisGkhPayDocMap.TableName, "GIS_GKH_GUID");
        }
    }
}