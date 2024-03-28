namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112400
{
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112400")][global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "COEFODN", DbType.Double);
            this.Database.ExecuteQuery("UPDATE GIS_HOUSE_SERVICE_REGISTER SET COEFODN = KOEFODN::float8 WHERE KOEFODN IS NOT NULL");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "KOEFODN");
        }

        public override void Down()
        {
            this.Database.AddColumn("GIS_HOUSE_SERVICE_REGISTER", "KOEFODN", DbType.String, 500);
            this.Database.ExecuteQuery("UPDATE GIS_HOUSE_SERVICE_REGISTER SET KOEFODN = COEFODN::varchar WHERE KOEFODN IS NOT NULL");
            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "COEFODN");
        }
    }
}