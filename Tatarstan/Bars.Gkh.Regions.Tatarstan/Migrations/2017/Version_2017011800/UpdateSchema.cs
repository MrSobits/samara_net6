namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017011800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017011800
    /// </summary>
	[Migration("2017011800")]
    [MigrationDependsOn(typeof(Version_2017011700.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017011800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_BUDGET_CONTR_PERIOD_SUMM",
                new Column("CHARGED", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("END_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "BUDGET_PERIOD_SUMM_MUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "BUDGET_PERIOD_SUMM_PERIOD_ID", "GKH_CONTRACT_PERIOD", "ID"),
                new RefColumn("SERVICE_ID", ColumnProperty.NotNull, "BUDGET_PERIOD_SUMM_SERVICE_ID", "GKH_RO_PUBRESORG_SERVICE", "ID"),
                new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "BUDGET_PERIOD_SUMM_CONTRACT_ID", "GKH_RSOCONTRACT_BUDGET_ORG", "ID"),
                new RefColumn("PUB_SERV_ORG_ID", ColumnProperty.NotNull, "BUDGET_PERIOD_SUMM_PUB_SERV_ORG_ID", "GKH_PUBLIC_SERVORG", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_BUDGET_CONTR_PERIOD_SUMM");
        }
    }
}