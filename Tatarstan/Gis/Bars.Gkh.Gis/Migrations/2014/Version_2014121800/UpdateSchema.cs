namespace Bars.Gkh.Gis.Migrations._2014.Version_2014121800
{
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121800")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "MUNICIPAL_AREA_GUID", DbType.String, 36);
            this.Database.AddColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "SETTLEMENT_GUID", DbType.String, 36);
            this.Database.AddColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "STREET_GUID", DbType.String, 36);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "MUNICIPAL_AREA_GUID");
            this.Database.RemoveColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "SETTLEMENT_GUID");
            this.Database.RemoveColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "STREET_GUID");
        }
    }
}