namespace Bars.Gkh.Gis.Migrations._2015.Version_2015120100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015120100")]
    [MigrationDependsOn(typeof(Version_2015102300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("GIS_KP50_BASE");

            this.Database.RemoveTable("GIS_KP50_DISTRICT");

            this.Database.AddEntityTable("GIS_DATABANK",
                new RefColumn("CONTRAGENT_ID", "GIS_DATABANK_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                new RefColumn("MUNICIPALITY_ID", "GIS_DATABANK_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("NAME", DbType.String, 200),
                new Column("KEY", DbType.String, 200));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_DATABANK");

            this.Database.AddPersistentObjectTable("GIS_KP50_DISTRICT",
                new Column("KP50_CODE", DbType.Int32, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull));

            this.Database.AddPersistentObjectTable("GIS_KP50_BASE",
                new Column("KP50_CODE", DbType.Int32, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 200, ColumnProperty.NotNull),
                new Column("PREFIX", DbType.String, 20, ColumnProperty.NotNull),
                new RefColumn("DISTRICT_ID", "GIS_KP50_BASE_DIST_ID", "GIS_KP50_DISTRICT", "ID"));
        }
    }
}