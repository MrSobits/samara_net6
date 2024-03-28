namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112100
{
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014112100")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("HOUSESERVICEREGISTER", new Column("TOTALVOLUME", DbType.Double));
            this.Database.RenameTable("HOUSEREGISTER", "GIS_HOUSE_REGISTER");

            if (this.Database.TableExists("GIS_SERVICE_DICTIONARY"))
            {
                this.Database.RenameTable("SERVICEDICTIONARY", "GIS_SERVICE_DICTIONARY");
            }
            
            this.Database.RenameTable("HOUSESERVICEREGISTER", "GIS_HOUSE_SERVICE_REGISTER");

            this.Database.AddEntityTable("GIS_INDICATOR_GROUPING",
                new Column("TYPE_GROUP_INDICATORS", DbType.Int32, ColumnProperty.NotNull),
                new Column("TYPE_INDICATORS", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GIS_MULTIPLE_ANALYSIS_TEMPLATE",
                new RefColumn("REAL_ESTATE_TYPE_ID", ColumnProperty.NotNull, "GIS_MUL_ANA_TEM_REAL_EST_ID", "GIS_REAL_ESTATE_TYPE", "ID"),
                new Column("TYPE_CONDITION", DbType.Int32, ColumnProperty.NotNull),
                new Column("FORMDAY", DbType.Int16, ColumnProperty.NotNull),
                new Column("EMAIL", DbType.String, ColumnProperty.NotNull, 200));

            this.Database.AddEntityTable("GIS_MULTIPLE_ANALYSIS_INDICATOR",
                new RefColumn("INDICATOR_GROUPING_ID", ColumnProperty.NotNull, "GIS_MUL_ANA_IND_IND_GRO_ID", "GIS_INDICATOR_GROUPING", "ID"),
                new RefColumn("MULTIPLE_ANALYSIS_TEMPLATE_ID", ColumnProperty.NotNull, "GIS_MUL_ANA_IND_MUL_ANA_TEM_ID", "GIS_MULTIPLE_ANALYSIS_TEMPLATE", "ID"),
                new Column("MINVALUE", DbType.Double),
                new Column("MAXVALUE", DbType.Double),
                new Column("DEVIATIONPERCENT", DbType.Double),
                new Column("EXACTVALUE", DbType.Double));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_MULTIPLE_ANALYSIS_INDICATOR");
            this.Database.RemoveTable("GIS_MULTIPLE_ANALYSIS_TEMPLATE");
            this.Database.RemoveTable("GIS_INDICATOR_JUXTAPOSE");

            this.Database.RemoveColumn("GIS_HOUSE_SERVICE_REGISTER", "TOTALVOLUME");
            this.Database.RenameTable("GIS_HOUSE_SERVICE_REGISTER", "HOUSESERVICEREGISTER");

            if (!this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000"))
            {
                this.Database.RenameTable("GIS_SERVICE_DICTIONARY", "SERVICEDICTIONARY");
            }

            this.Database.RenameTable("GIS_HOUSE_REGISTER", "HOUSEREGISTER");
        }
    }
}