namespace Bars.Gkh.Gis.Migrations._2015.Version_2015071600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Gis.Enum;

    [Migration("2015071600")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015070700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GIS_WASTE_COLL_PLACE",
                new RefColumn("REAL_OBJ", "WASTE_PLACE_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("CUSTOMER", "WASTE_PLACE_CUST", "GKH_CONTRAGENT", "ID"),
                new Column("TYPE_WASTE", DbType.Int32, ColumnProperty.NotNull, (int)TypeWaste.Solid),
                new Column("TYPE_WASTE_PLACE", DbType.Int32, ColumnProperty.NotNull, (int)TypeWasteCollectionPlace.SingleContainer),
                new Column("PEOPLE_COUNT", DbType.Int32),
                new Column("CONTAINERS_COUNT", DbType.Int32),
                new Column("ACCUM_DAILY", DbType.Int32),
                new Column("LANDFILL_DIST", DbType.Int32),
                new Column("COMMENT", DbType.String, 1000),
                new Column("EXP_DAYS_WINTER", DbType.String, 2000, ColumnProperty.Null, "'[]'"),
                new Column("EXP_DAYS_SUMMER", DbType.String, 2000, ColumnProperty.Null, "'[]'"),
                new RefColumn("CONTRACTOR", "WASTE_PLACE_CONTR", "GKH_CONTRAGENT", "ID"),
                new Column("NUMBER_CONTRACT", DbType.String, 200),
                new Column("DATE_CONTRACT", DbType.DateTime),
                new RefColumn("FILE_CONTRACT", "WASTE_PLACE_CTRFILE", "B4_FILE_INFO", "ID"),
                new Column("LANDFILL_ADDRESS", DbType.String, 2000));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_WASTE_COLL_PLACE");
        }
    }
}