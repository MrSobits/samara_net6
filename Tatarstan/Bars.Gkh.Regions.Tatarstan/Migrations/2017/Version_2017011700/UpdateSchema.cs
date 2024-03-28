namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017011700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017011700
    /// </summary>
	[Migration("2017011700")]
    [MigrationDependsOn(typeof(_2016.Version_2016121500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RemoveColumn("GKH_CONTR_PERIOD_SUMM_RSO", "BILL_PUB_SERV_ORG");
            this.Database.RemoveColumn("GKH_CONTR_PERIOD_SUMM_RSO", "PAID_PUB_SERV_ORG");
            this.Database.RemoveColumn("GKH_CONTR_PERIOD_SUMM_DETAIL", "PAID_PUB_SERV_ORG");
            this.Database.RemoveColumn("GKH_CONTR_PERIOD_SUMM_DETAIL", "PAID_PUB_SERV_ORG");
        }

        public override void Down()
        {
            this.Database.AddColumn("GKH_CONTR_PERIOD_SUMM_RSO", new Column("BILL_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));
            this.Database.AddColumn("GKH_CONTR_PERIOD_SUMM_RSO", new Column("PAID_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));
            this.Database.AddColumn("GKH_CONTR_PERIOD_SUMM_DETAIL", new Column("BILL_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));
            this.Database.AddColumn("GKH_CONTR_PERIOD_SUMM_DETAIL", new Column("PAID_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }
    }
}