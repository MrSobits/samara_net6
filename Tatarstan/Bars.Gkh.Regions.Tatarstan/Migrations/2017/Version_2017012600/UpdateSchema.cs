namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017012600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017012600
    /// </summary>
	[Migration("2017012600")]
    [MigrationDependsOn(typeof(Version_2017011800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017011801.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_SERV_ORG_FUEL_ENERGY_PERIOD_SUMM",
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "GKH_SERV_ORG_FUEL_EN_PER_SUMM_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "GKH_SERV_ORG_FUEL_EN_PER_SUMM_PERIOD_ID", "GKH_CONTRACT_PERIOD", "ID"),
                new RefColumn("PUB_SERV_ORG_ID", ColumnProperty.NotNull, "GKH_SERV_ORG_FUEL_EN_PER_SUMM_PUB_SERV_ORG_ID", "GKH_PUBLIC_SERVORG", "ID"));

            this.Database.AddEntityTable(
                "GKH_FUEL_ENERGY_CONTRACT_DETAIL",
                new Column("CHARGED", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new RefColumn("PER_SUMM_ID", ColumnProperty.NotNull, "GKH_FUEL_ENERG_CONTRT_DET_PER_SUMM_ID", "GKH_SERV_ORG_FUEL_ENERGY_PERIOD_SUMM", "ID"),
                new RefColumn("SERV_ID", ColumnProperty.NotNull, "GKH_FUEL_ENERG_CONTRT_DET_SERV_ID", "GIS_SERVICE_DICTIONARY", "ID"),
                new RefColumn("GAS_PERC_ID", ColumnProperty.NotNull, "GKH_FUEL_ENERG_CONTRT_DET_GAS_PERC_ID", "GKH_PLAN_PAYMENTS_PERCENTAGE", "ID"),
                new RefColumn("EL_PERC_ID", ColumnProperty.NotNull, "GKH_FUEL_ENERG_CONTRT_DET_EL_PERC_ID", "GKH_PLAN_PAYMENTS_PERCENTAGE", "ID"));

            this.Database.AddEntityTable(
                "GKH_FUEL_ENERGY_CONTRACT_INFO",
                new Column("SALDO_IN", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("DEBT_IN", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHARGED", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("SALDO_OUT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("DEBT_OUT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PLAN_PAID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new RefColumn("PER_SUMM_ID", ColumnProperty.NotNull, "GKH_FUEL_EN_CONTR_INF_PER_SUMM_ID", "GKH_SERV_ORG_FUEL_ENERGY_PERIOD_SUMM", "ID"),
                new RefColumn("RESOURCE_ID", ColumnProperty.NotNull, "GKH_FUEL_EN_CONTR_INF_RESOURCE_ID", "GKH_DICT_COMM_RESOURCE", "ID"),
                new RefColumn("CONTR_ID", ColumnProperty.NotNull, "GKH_FUEL_EN_CONTR_INF_CONTR_ID", "GKH_RSOCONTRACT_FUEL_ENERGY_ORG", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_FUEL_ENERGY_CONTRACT_DETAIL");
            this.Database.RemoveTable("GKH_FUEL_ENERGY_CONTRACT_INFO");
            this.Database.RemoveTable("GKH_SERV_ORG_FUEL_ENERGY_PERIOD_SUMM");
        }
    }
}