namespace Bars.Gkh.Gis.Migrations._2017.Version_2017101300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2017101300")]
    [MigrationDependsOn(typeof(_2016.Version_2016011400.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016011600.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016040100.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016040200.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016041500.UpdateSchema))]
    [MigrationDependsOn(typeof(_2016.Version_2016110300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_TARIFF_DICTIONARY",
                new Column("EXTERNAL_ID", DbType.String),
                new Column("EIAS_UPLOAD_DATE", DbType.DateTime),
                new Column("EIAS_EDIT_DATE", DbType.DateTime),
                new Column("ACTIVITY_KIND", DbType.String, 1000),
                new Column("CONTRAGENT_NAME", DbType.String, 2000),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("TARIFF_KIND", DbType.Int32, ColumnProperty.NotNull),
                new Column("ZONE_COUNT", DbType.Int32),
                new Column("TARIFF_VALUE", DbType.Decimal),
                new Column("TARIFF_VALUE1", DbType.Decimal),
                new Column("TARIFF_VALUE2", DbType.Decimal),
                new Column("TARIFF_VALUE3", DbType.Decimal),
                new Column("IS_NDS_INCLUDE", DbType.Boolean),
                new Column("IS_SOCIAL_NORM", DbType.Boolean),
                new Column("IS_METER_EXISTS", DbType.Boolean),
                new Column("IS_ELECTRIC_STOVE_EXISTS", DbType.Boolean),
                new Column("FLOOR", DbType.Int32),
                new Column("CONSUMER_TYPE", DbType.Int32),
                new Column("SETTELMENT_TYPE", DbType.Int32),
                new Column("CONSUMER_BY_ELECTRIC_ENERGY_TYPE", DbType.Int32),
                new Column("REG_PERIOD_ATTRIBUTE", DbType.String, 1000),
                new Column("BASE_PERIOD_ATTRIBUTE", DbType.String, 1000),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GIS_TARIFF_DICTIONARY_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("SERVICE_ID", ColumnProperty.NotNull, "GIS_TARIFF_DICTIONARY_SERVICE", "GIS_SERVICE_DICTIONARY", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GIS_TARIFF_DICTIONARY_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

            this.Database.ExecuteNonQuery(@"
INSERT INTO GIS_TARIFF_DICTIONARY (
    OBJECT_VERSION,
    OBJECT_CREATE_DATE,
    OBJECT_EDIT_DATE,
    START_DATE,
    END_DATE,
    TARIFF_KIND,
    TARIFF_VALUE,
    TARIFF_VALUE1,
    MUNICIPALITY_ID,
    SERVICE_ID,
    CONTRAGENT_ID
)
SELECT
    0,
    date_trunc('minute', d.OBJECT_CREATE_DATE),
    date_trunc('minute', d.OBJECT_EDIT_DATE),
    DATE_START,
    DATE_END,
    1,
    CASE WHEN s.NAME !~* 'ЭЛЕК' THEN d.VALUE ELSE NULL END,
    CASE WHEN s.NAME ~* 'ЭЛЕК' THEN d.VALUE ELSE NULL END,
    MU_ID,
    SERVICE_ID,
    CONTRAGENT_ID
FROM GIS_TARIF_DICTIONARY d
JOIN GIS_SERVICE_DICTIONARY s on s.ID = d.SERVICE_ID;
");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_TARIFF_DICTIONARY");
        }
    }
}
