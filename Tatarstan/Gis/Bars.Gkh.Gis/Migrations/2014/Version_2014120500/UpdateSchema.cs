namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014120500")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014120402.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_SERVICE_SUBSIDY_REGISTER", new[]
            {
                new Column("PSS", DbType.String),
                new Column("CALCULATIONMONTH", DbType.DateTime),
                new Column("MANAGEMENTORGANIZATIONACCOUNT", DbType.Int64),
                new RefColumn("SERVICE", "GIS_SRV_SBS_SRV_ID", "GIS_SERVICE_DICTIONARY", "ID"),
                new Column("ACCRUEDBENEFITSUM", DbType.Double),
                new Column("ACCRUEDEDVSUM", DbType.Double), 
                new Column("RECALCULATEDBENEFITSUM", DbType.Double), 
                new Column("RECALCULATEDEDVSUM", DbType.Double), 
                new Column("ORGANIZATIONUNIT", DbType.String),
                new RefColumn("LOADEDFILE", "GIS_SRV_SBS_LOADFILE_ID", "GIS_LOADED_FILE_REGISTER", "ID"),
                new Column("ADDRESS", DbType.String),
                new Column("PERSONALACCOUNTID", DbType.Int64)
            });
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_TENANT_SUBSIDY_REGISTER");
        }
    }
}