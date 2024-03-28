namespace Bars.Gkh.Gis.Migrations._2015.Version_2015091500
{
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015091500")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015091400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "MONTH_YEAR", DbType.DateTime);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "MONTH_YEAR");
        }
    }
}