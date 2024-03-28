namespace Bars.Gkh.Gis.Migrations._2015.Version_2015091700
{
    using System.Data;
    using System.Linq;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2015091700")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015091500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (this.Database.ColumnExists("GIS_SERVICE_DICTIONARY", "TYPE_SERVICE"))
            {
                this.Database.AddColumn("GIS_SERVICE_DICTIONARY", "TYPE_SERVICE", DbType.Int32, ColumnProperty.NotNull, 0);
            }
        }

        public override void Down()
        {
            if (!this.Database.GetAppliedMigrations().Any(x => x.ModuleId == "Bars.Gkh" && x.Version == "2016052000"))
            {
                this.Database.RemoveColumn("GIS_SERVICE_DICTIONARY", "TYPE_SERVICE");
            }
        }
    }
}