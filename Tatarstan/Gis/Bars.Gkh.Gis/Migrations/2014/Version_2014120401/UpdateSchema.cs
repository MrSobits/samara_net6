namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2014120401")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", new Column("LAST_FORM_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GIS_MULTIPLE_ANALYSIS_TEMPLATE", "LAST_FORM_DATE");
        }
    }
}