namespace Bars.Gkh.Regions.Msk.Migration.Version_2015011300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015011300")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("MSK_RO_INFO",
                new Column("UID", DbType.String, 100, ColumnProperty.NotNull),
                new Column("OKRUG", DbType.String, 100),
                new Column("RAION", DbType.String, 100),
                new Column("ADDRESS", DbType.String, 250),
                new Column("UNOM_CODE", DbType.String, 50),
                new Column("MZI_CODE", DbType.String, 50),
                new Column("SERIAL", DbType.String, 50),
                new Column("BUILDING_YEAR", DbType.Int32),
                new Column("TOTAL_AREA", DbType.Decimal),
                new Column("LIVING_AREA", DbType.Decimal),
                new Column("NOLIVING_AREA", DbType.Decimal),
                new Column("FLOOR_COUNT", DbType.Int32),
                new Column("PORCH_COUNT", DbType.Int32),
                new Column("FLAT_COUNT", DbType.Int32),
                new Column("ALL_DELAY", DbType.Decimal),
                new Column("POINTS", DbType.Decimal),
                new Column("INDEX_NUMBER", DbType.Int32),
                new Column("ES_PERIOD", DbType.String, 50),
                new Column("GAS_PERIOD", DbType.String, 50),
                new Column("HVS_PERIOD", DbType.String, 50),
                new Column("HVSM_PERIOD", DbType.String, 50),
                new Column("GVS_PERIOD", DbType.String, 50),
                new Column("GVSM_PERIOD", DbType.String, 50),
                new Column("KAN_PERIOD", DbType.String, 50),
                new Column("KANM_PERIOD", DbType.String, 50),
                new Column("OTOP_PERIOD", DbType.String, 50),
                new Column("OTOPM_PERIOD", DbType.String, 50),
                new Column("MUS_PERIOD", DbType.String, 50),
                new Column("PPIADU_PERIOD", DbType.String, 50),
                new Column("PV_PERIOD", DbType.String, 50),
                new Column("FAS_PERIOD", DbType.String, 50),
                new Column("KROV_PERIOD", DbType.String, 50),
                new Column("VDSK_PERIOD", DbType.String, 50),
                new Column("LIFT_PERIOD", DbType.String, 100));

            Database.AddEntityTable("MSK_DPKR_INFO",
                new RefColumn("RO_INFO_ID", "MSK_DPKR_INFO_ROI", "MSK_RO_INFO", "ID"),
                new Column("CEO_TYPE", DbType.Int32, ColumnProperty.NotNull, 1),
                new Column("CEO_STATE", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("DELAY", DbType.Decimal),
                new Column("LIFETIME", DbType.Int32),
                new Column("LAST_REPAIR_YEAR", DbType.Int32),
                new Column("PERIOD", DbType.String, 100));

            Database.AddEntityTable("MSK_LIFT_INFO",
                new RefColumn("RO_INFO_ID", "MSK_LIFT_INFO_ROI", "MSK_RO_INFO", "ID"),
                new Column("CAPACITY", DbType.Int32),
                new Column("STOP_COUNT", DbType.Int32),
                new Column("LIFETIME", DbType.String, 50),
                new Column("INSTALL_YEAR", DbType.Int32),
                new Column("PORCH", DbType.String, 50),
                new Column("PERIOD", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveTable("MSK_LIFT_INFO");
            Database.RemoveTable("MSK_DPKR_INFO");
            Database.RemoveTable("MSK_RO_INFO");
        }
    }
}