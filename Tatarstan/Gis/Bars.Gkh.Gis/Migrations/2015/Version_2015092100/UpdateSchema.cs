namespace Bars.Gkh.Gis.Migrations._2015.Version_2015092100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015092100")]
    [MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015092000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GIS_NORMATIV_DICTIONARY", "MEASURE", DbType.String, 50);

            this.Database.AddRefColumn("GIS_TARIF_DICTIONARY", new RefColumn("CONTRAGENT_ID", "GIS_NORM_DICT_CONTR", "GKH_CONTRAGENT", "ID"));
            this.Database.RemoveColumn("GIS_TARIF_DICTIONARY", "SUPPL_NAME");
        }

        public override void Down()
        {
            this.Database.AddColumn("GIS_TARIF_DICTIONARY", new Column("SUPPL_NAME", DbType.String, 200));
            this.Database.RemoveColumn("GIS_TARIF_DICTIONARY", "CONTRAGENT_ID");

            this.Database.RemoveColumn("GIS_NORMATIV_DICTIONARY", "MEASURE");
        }
    }
}