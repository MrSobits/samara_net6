namespace Bars.Gkh.Migrations._2017.Version_2017040400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017040400
    /// </summary>
    [Migration("2017040400")]
    [MigrationDependsOn(typeof(Version_2017032701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_HOUSING_FUND_MONITOR_PERIOD",
                new RefColumn("MU_ID", ColumnProperty.NotNull, "GKH_HOUSING_FUND_MONITOR_PERIOD_MU_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("YEAR", DbType.Int32));

            this.Database.AddEntityTable(
                "GKH_HOUSING_FUND_MONITOR_INFO",
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "GKH_HOUSING_FUND_MONITOR_INFO_PERIOD_ID", "GKH_HOUSING_FUND_MONITOR_PERIOD", "ID"),
                new Column("ROW_NUMBER", DbType.String),
                new Column("MARK", DbType.String),
                new Column("UNIT_MEASURE", DbType.String),
                new Column("VALUE", DbType.Decimal),
                new Column("DATA_PROVIDER", DbType.String));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_HOUSING_FUND_MONITOR_INFO");
            this.Database.RemoveTable("GKH_HOUSING_FUND_MONITOR_PERIOD");
        }
    }
}