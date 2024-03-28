namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016112800
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016112800")]
    [MigrationDependsOn(typeof(Version_2016041900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_TAT_PERIOD_NORM_CONSUMPTION",
                new Column("NAME", DbType.String, 200),
                new Column("START_DATE", DbType.Date),
                new Column("END_DATE", DbType.Date));

            this.Database.AddEntityTable(
                "GKH_TAT_NORM_CONSUMPTION",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "NORM_CONS_MUN_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "NORM_CONS_PERIOD_ID", "GKH_TAT_PERIOD_NORM_CONSUMPTION", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "NORM_CONS_STATE_ID", "B4_STATE", "ID"),
                new Column("TYPE", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "GKH_TAT_NORM_CONS_REC",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "NORM_CONS_RO_ID", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("NORM_CONS_ID", ColumnProperty.NotNull, "NORM_CONS_REC_CONS_ID", "GKH_TAT_NORM_CONSUMPTION", "ID"));

            this.Database.AddTable("GKH_TAT_NORM_CONS_COLD_WATTER",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("IS_IPU_NOT_LIV_PERMISES", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("AREA_IPU_NOT_LIV_PERMISES", DbType.Decimal),
                new Column("VOL_COLD_WATER_NOT_LIV_ISIPU", DbType.Decimal),
                new Column("VOL_WATER_OPU_ON_PERIOD", DbType.Decimal),
                new Column("HEATING_PERIOD", DbType.Int32),
                new Column("IS_BATH_1200", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_BATH_1500_WITH_1550", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_BATH_1650_WITH_1700", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_BATH_NOT_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HVS_IS_BATH_1200", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HVS_IS_BATH_1500_WITH_1550", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HVS_IS_BATH_NOT_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HVS_IS_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_NOT_BOILER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HVS_IS_NOT_BOILER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_HVS_BATH_IS_NOT_CNTRL_SEW", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_HVS_IS_NOT_CNTRL_SEW", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_STANDPIPES", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_HOSTEL_NOSHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_HOSTEL_SHARED_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_HOSTEL_SHOWER_LIV_PER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("SHOWER_IN_HOSTEL_IN_SECT", DbType.Int32, ColumnProperty.NotNull, 0));

            this.Database.AddForeignKey("FK_NORM_CONS_COLDWATER_ID", "GKH_TAT_NORM_CONS_COLD_WATTER", "ID", "GKH_TAT_NORM_CONS_REC", "ID");

            this.Database.AddTable("GKH_TAT_NORM_CONS_HOT_WATTER",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("IS_IPU_NOT_LIV_PERMISES", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("AREA_IPU_NOT_LIV_PERMISES", DbType.Decimal),
                new Column("VOL_HOT_WATER_NOT_LIV_ISIPU", DbType.Decimal),
                new Column("VOL_WATER_OPU_ON_PERIOD", DbType.Decimal),
                new Column("HEATING_PERIOD", DbType.Int32),
                new Column("IS_BATH_1200", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_BATH_1500_WITH_1550", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_BATH_1650_WITH_1700", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_BATH_NOT_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_SHOWER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("SHARED_SHOWER_INHOSTEL", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("SHARED_SHOWER_HSTL_ALL_LIV_PRM", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("SHOWER_IN_HOSTEL_IN_SECT", DbType.Int32, ColumnProperty.NotNull, 0));

            this.Database.AddForeignKey("FK_NORM_CONS_HOTWATER_ID", "GKH_TAT_NORM_CONS_HOT_WATTER", "ID", "GKH_TAT_NORM_CONS_REC", "ID");

            this.Database.AddTable("GKH_TAT_NORM_CONS_FIRING",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TECH_CAPABILITY_OPU", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_IPU_NOT_LIV_PERMISES", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("AREA_NOT_LIV_IPU", DbType.Decimal),
                new Column("AMOUNT_HEAT_NOT_LIV_IPU", DbType.Decimal),
                new Column("AMOUNT_HEAT_NOT_LIV_PERIOD", DbType.Decimal),
                new Column("HEATING_PERIOD", DbType.Int32),
                new Column("HOURLY_HEAT_LOAD_FOR_PASS", DbType.Decimal),
                new Column("HOURLY_HEAT_LOAD_FOR_DOC", DbType.Decimal));

            this.Database.AddForeignKey("FK_NORM_CONS_FIRING_ID", "GKH_TAT_NORM_CONS_FIRING", "ID", "GKH_TAT_NORM_CONS_REC", "ID");

            this.Database.AddTable("GKH_TAT_NORM_CONS_HEATING",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TECH_CAPABILITY_OPU", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_IPU_NOT_LIV_PERMISES", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HOT_WATER_CONS_IN_PERIOD", DbType.Decimal),
                new Column("TYPE_HOT_WATTER_SYSTEM", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("IS_HEATED_TOWEL_RAIL", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("RISERS", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("HEAT_CONS_NOT_LIV_PERIOD", DbType.Decimal),
                new Column("HOT_WATER_CONS_NOT_LIV_PER", DbType.Decimal),
                new Column("HEATING_PERIOD", DbType.Int32),
                new Column("AVG_TEMP_COLD_WATER", DbType.Decimal),
                new Column("WEAR_INTRAHOUSE_UTIL", DbType.Decimal));

            this.Database.AddForeignKey("FK_NORM_CONS_HEATING_ID", "GKH_TAT_NORM_CONS_HEATING", "ID", "GKH_TAT_NORM_CONS_REC", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_TAT_NORM_CONS_COLD_WATTER");
            this.Database.RemoveTable("GKH_TAT_NORM_CONS_HOT_WATTER");
            this.Database.RemoveTable("GKH_TAT_NORM_CONS_FIRING");
            this.Database.RemoveTable("GKH_TAT_NORM_CONS_HEATING");
            this.Database.RemoveTable("GKH_TAT_NORM_CONS_REC");
            this.Database.RemoveTable("GKH_TAT_NORM_CONSUMPTION");
            this.Database.RemoveTable("GKH_TAT_PERIOD_NORM_CONSUMPTION");
        }
    }
}
