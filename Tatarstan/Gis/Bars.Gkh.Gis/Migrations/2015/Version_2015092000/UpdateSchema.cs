namespace Bars.Gkh.Gis.Migrations._2015.Version_2015092000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015092000")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015091702.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_NORMATIV_DICTIONARY",
                new RefColumn("MU_ID", "GIS_NORM_DICT_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("SERVICE_ID", "GIS_NORM_DICT_SERV", "GIS_SERVICE_DICTIONARY", "ID"),
                new Column("VALUE", DbType.Decimal),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("DOC_NAME", DbType.String, 100),
                new RefColumn("DOC_FILE_ID", "GIS_NORM_DICT_DOC", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable("GIS_TARIF_DICTIONARY",
                new RefColumn("MU_ID", "GIS_TARIF_DICT_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("SERVICE_ID", "GIS_TARIF_DICT_SERV", "GIS_SERVICE_DICTIONARY", "ID"),
                new Column("SUPPL_NAME", DbType.String, 200),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.DateTime),
                new Column("VALUE", DbType.Decimal),
                new Column("PURCH_VOLUME", DbType.Decimal),
                new Column("PURCH_COST", DbType.Decimal),
                new Column("REQUISITES", DbType.String, 500));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_TARIF_DICTIONARY");

            this.Database.RemoveTable("GIS_NORMATIV_DICTIONARY");
        }
    }
}