namespace Bars.Gkh.Migrations._2022.Version_2022102800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022102800")]
    
    [MigrationDependsOn(typeof(Version_2022101200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("TOTAL_BUILDING_VOL", DbType.Decimal));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("AREA_MKD", DbType.Decimal));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("AREA_LIVING_NOT_LIVING", DbType.Decimal));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("AREA_LIVING", DbType.Decimal));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("AREA_NOT_LIVING", DbType.Decimal));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("AREA_NOT_LIVING_FUNC", DbType.Decimal));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("FLOORS", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("NUMBER_APARTMENTS", DbType.Int32));
            Database.AddRefColumn("GKH_OBJ_TECHNICAL_MONITORING", new RefColumn("WALL_MATERIAL", "GKH_OBJ_TECHNICAL_MONITORING_WALLMAT", "GKH_DICT_WALL_MATERIAL", "ID"));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("PHYSICAL_WEAR", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("TOTAL_WEAR", DbType.Int32));
            Database.AddRefColumn("GKH_OBJ_TECHNICAL_MONITORING", new RefColumn("CAPITAL_GROUP", "GKH_OBJ_TECHNICAL_MONITORING_CAPGROUP", "GKH_DICT_CAPITAL_GROUP", "ID"));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_FOUNDATION", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_WALLS", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_ROOF", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_INNER_SYSTEMS", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_HEATING", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_WATER", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_WATER_COLD", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_WATER_HOT", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_SEWERE", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_ELECTRIC", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_LIFT", DbType.Int32));
            Database.AddColumn("GKH_OBJ_TECHNICAL_MONITORING", new Column("WEAR_GAS", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "TOTAL_BUILDING_VOL");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "AREA_MKD");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "AREA_LIVING_NOT_LIVING");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "AREA_LIVING");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "AREA_NOT_LIVING");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "AREA_NOT_LIVING_FUNC");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "FLOORS");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "NUMBER_APARTMENTS");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WALL_MATERIAL");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "PHYSICAL_WEAR");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "TOTAL_WEAR");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "CAPITAL_GROUP");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_ALL");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_FOUNDATION");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_WALLS");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_ROOF");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_INNER_SYSTEMS");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_HEATING");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_WATER");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_WATER_COLD");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_WATER_HOT");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_SEWERE");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_ELECTRIC");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_LIFT");
            Database.RemoveColumn("GKH_OBJ_TECHNICAL_MONITORING", "WEAR_GAS");
        }
    }
}