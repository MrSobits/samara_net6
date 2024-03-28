namespace Bars.GkhGji.Migrations._2017.Version_2017032800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017.03.28.00
    /// </summary>
    [Migration("2017032800")]
    [MigrationDependsOn(typeof(Version_2017022100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_FUEL_INFO_PERIOD",
                new RefColumn("MU_ID", ColumnProperty.NotNull, "GJI_FUEL_INFO_PERIOD_MU_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("YEAR", DbType.Int32),
                new Column("MONTH", DbType.Byte));

            this.Database.AddEntityTable(
                "GJI_BASE_FUEL_INFO",
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "GJI_BASE_FUEL_INFO_PERIOD_ID", "GJI_FUEL_INFO_PERIOD", "ID"),
                new Column("MARK", DbType.String),
                new Column("ROW_NUMBER", DbType.Int32));

            this.Database.AddJoinedSubclassTable(
                "GJI_FUEL_AMOUNT_INFO",
                "GJI_BASE_FUEL_INFO",
                "GJI_FUEL_AMOUNT_INFO_BASE_ID",
                new Column("COAL_TOTAL", DbType.Decimal),
                new Column("COAL_PRIMARY", DbType.Decimal),
                new Column("COAL_RESERVE", DbType.Decimal),
                new Column("FIREWOOD_TOTAL", DbType.Decimal),
                new Column("FIREWOOD_PRIMARY", DbType.Decimal),
                new Column("FIREWOOD_RESERVE", DbType.Decimal),
                new Column("MASUT_TOTAL", DbType.Decimal),
                new Column("MASUT_PRIMARY", DbType.Decimal),
                new Column("MASUT_RESERVE", DbType.Decimal),
                new Column("GAS_TOTAL", DbType.Decimal),
                new Column("GAS_PRIMARY", DbType.Decimal),
                new Column("GAS_RESERVE", DbType.Decimal),
                new Column("OTHER_TOTAL", DbType.Decimal),
                new Column("OTHER_PRIMARY", DbType.Decimal),
                new Column("OTHER_RESERVE", DbType.Decimal),
                new Column("PEAT_TOTAL", DbType.Decimal),
                new Column("PEAT_PRIMARY", DbType.Decimal),
                new Column("PEAT_RESERVE", DbType.Decimal),
                new Column("LIQUEFIED_GAS_TOTAL", DbType.Decimal),
                new Column("LIQUEFIED_GAS_PRIMARY", DbType.Decimal),
                new Column("LIQUEFIED_GAS_RESERVE", DbType.Decimal),
                new Column("WOOD_WASTE_TOTAL", DbType.Decimal),
                new Column("WOOD_WASTE_PRIMARY", DbType.Decimal),
                new Column("WOOD_WASTE_RESERVE", DbType.Decimal),
                new Column("DIESEL_TOTAL", DbType.Decimal),
                new Column("DIESEL_PRIMARY", DbType.Decimal),
                new Column("DIESEL_RESERVE", DbType.Decimal),
                new Column("ELECTRIC_TOTAL", DbType.Decimal),
                new Column("ELECTRIC_PRIMARY", DbType.Decimal),
                new Column("ELECTRIC_RESERVE", DbType.Decimal));

            this.Database.AddJoinedSubclassTable(
               "GJI_FUEL_EXTRACT_DIST_INFO",
               "GJI_BASE_FUEL_INFO",
               "GJI_FUEL_EXTRACT_DIST_INFO_BASE_ID",
                new Column("DISTANCE", DbType.Decimal));

            this.Database.AddJoinedSubclassTable(
                "GJI_FUEL_CONTRACT_OBLIG_INFO",
                "GJI_BASE_FUEL_INFO",
                "GJI_FUEL_CONTRACT_OBLIG_INFO_BASE_ID",
                new Column("COAL_TOTAL", DbType.Decimal),
                new Column("COAL_DIRECT_CONTRACT", DbType.Decimal),
                new Column("COAL_INTERMEDIATOR", DbType.Decimal),
                new Column("FIREWOOD_TOTAL", DbType.Decimal),
                new Column("FIREWOOD_DIRECT_CONTRACT", DbType.Decimal),
                new Column("FIREWOOD_INTERMEDIATOR", DbType.Decimal),
                new Column("MASUT_TOTAL", DbType.Decimal),
                new Column("MASUT_DIRECT_CONTRACT", DbType.Decimal),
                new Column("MASUT_INTERMEDIATOR", DbType.Decimal),
                new Column("GAS_TOTAL", DbType.Decimal),
                new Column("GAS_DIRECT_CONTRACT", DbType.Decimal),
                new Column("GAS_INTERMEDIATOR", DbType.Decimal),
                new Column("OTHER_TOTAL", DbType.Decimal),
                new Column("OTHER_DIRECT_CONTRACT", DbType.Decimal),
                new Column("OTHER_INTERMEDIATOR", DbType.Decimal),
                new Column("PEAT_TOTAL", DbType.Decimal),
                new Column("PEAT_DIRECT_CONTRACT", DbType.Decimal),
                new Column("PEAT_INTERMEDIATOR", DbType.Decimal),
                new Column("LIQUEFIED_GAS_TOTAL", DbType.Decimal),
                new Column("LIQUEFIED_GAS_DIRECT_CONTRACT", DbType.Decimal),
                new Column("LIQUEFIED_GAS_INTERMEDIATOR", DbType.Decimal),
                new Column("WOOD_WASTE_TOTAL", DbType.Decimal),
                new Column("WOOD_WASTE_DIRECT_CONTRACT", DbType.Decimal),
                new Column("WOOD_WASTE_INTERMEDIATOR", DbType.Decimal),
                new Column("DIESEL_TOTAL", DbType.Decimal),
                new Column("DIESEL_DIRECT_CONTRACT", DbType.Decimal),
                new Column("DIESEL_INTERMEDIATOR", DbType.Decimal),
                new Column("ELECTRIC_TOTAL", DbType.Decimal),
                new Column("ELECTRIC_DIRECT_CONTRACT", DbType.Decimal),
                new Column("ELECTRIC_INTERMEDIATOR", DbType.Decimal));

            this.Database.AddJoinedSubclassTable(
                "GJI_FUEL_ENERGY_DEBT_INFO",
                "GJI_BASE_FUEL_INFO",
                "GJI_FUEL_ENERGY_DEBT_INFO_BASE_ID",
                new Column("TOTAL", DbType.Decimal));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveTable("GJI_FUEL_ENERGY_DEBT_INFO");
            this.Database.RemoveTable("GJI_FUEL_CONTRACT_OBLIG_INFO");
            this.Database.RemoveTable("GJI_FUEL_EXTRACT_DIST_INFO");
            this.Database.RemoveTable("GJI_FUEL_AMOUNT_INFO");
            this.Database.RemoveTable("GJI_BASE_FUEL_INFO");
            this.Database.RemoveTable("GJI_FUEL_INFO_PERIOD");
        }
    }
}