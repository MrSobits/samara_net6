namespace Bars.Gkh.Gis.Migrations._2016.Version_2016011400
{
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016011400")]
    [MigrationDependsOn(typeof(_2015.Version_2015120100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000"))
            {
                this.Database.AddColumn("GIS_SERVICE_DICTIONARY", new Column("TYPE_COMM_RESOURCE", DbType.Int32));
                this.Database.AddColumn("GIS_SERVICE_DICTIONARY", new Column("FOR_ALL_HOUSE_NEEDS", DbType.Boolean));
                this.Database.AddColumn("GIS_SERVICE_DICTIONARY", new RefColumn("UNIT_MEASURE_ID", "GIS_SERVICE_UNIT_MEASURE", "GKH_DICT_UNITMEASURE", "ID"));
            }
        }

        public override void Down()
        {
            if (!this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000"))
            {
                this.Database.RemoveColumn("GIS_SERVICE_DICTIONARY", "TYPE_COMM_RESOURCE");
                this.Database.RemoveColumn("GIS_SERVICE_DICTIONARY", "FOR_ALL_HOUSE_NEEDS");
                this.Database.RemoveColumn("GIS_SERVICE_DICTIONARY", "UNIT_MEASURE_ID");
            }
        }
    }
}
