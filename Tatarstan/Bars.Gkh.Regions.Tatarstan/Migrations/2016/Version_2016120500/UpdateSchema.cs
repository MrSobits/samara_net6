namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016120500
{
    using System.Data;

    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016120500
    /// </summary>
    [Migration("2016120500")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016120500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2016041900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накат
        /// </summary>
        public override void Up()
        {
            //Отчетный период договора
            this.Database.AddEntityTable(
                "GKH_CONTRACT_PERIOD",
                new Column("START_DATE", DbType.DateTime),
                new Column("END_DATE", DbType.DateTime),
                new Column("NAME", DbType.String),
                new Column("UO_NUMBER", DbType.Int32),
                new Column("RSO_NUMBER", DbType.Int32),
                new Column("RO_NUMBER", DbType.Int32));

            //Агрегация по Периоду для РСО
            this.Database.AddEntityTable(
                "GKH_CONTR_PERIOD_SUMM_RSO",
                new RefColumn("PUB_SERV_ORG_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_RSO_ORG_ID", "GKH_PUBLIC_SERVORG", "ID"),
                new RefColumn("STATE_ID", "GKH_CONTR_PERIOD_SUMM_RSO_STATE_ID", "B4_STATE", "ID"),
                new Column("CHARGED_MAN_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID_MAN_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("SALDO_OUT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("BILL_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));

            //Агрегация по Периоду для УО
            this.Database.AddEntityTable(
                "GKH_CONTR_PERIOD_SUMM_UO",
                new RefColumn("MAN_ORG_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_UO_MAN_ORG_ID", "GKH_MANAGING_ORGANIZATION", "ID"),
                new RefColumn("STATE_ID", "GKH_CONTR_PERIOD_SUMM_UO_STATE_ID", "B4_STATE", "ID"),
                new Column("START_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHARGED_RESID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("RECALC_PREV_PERIOD", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHANGE_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("NO_DELIV_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID_RESID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("END_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHARGED_TO_PAY", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("TRANSFER_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));

            //Информация по периоду для Договора УО с РСО на предоставление услуги
            this.Database.AddEntityTable(
                "GKH_CONTR_PERIOD_SUMM",
                new RefColumn("MU_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_MU_ID", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("PER_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_PER_ID", "GKH_CONTRACT_PERIOD", "ID"),
                new RefColumn("PER_SUMM_RSO_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_PER_SUMM_RSO_ID", "GKH_CONTR_PERIOD_SUMM_RSO", "ID"),
                new RefColumn("PER_SUMM_UO_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_PER_SUMM_UO_ID", "GKH_CONTR_PERIOD_SUMM_UO", "ID"),
                new RefColumn("PUB_SERV_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_PER_SUMM_PUB_SERV_ID", "GKH_RO_PUBRESORG_SERVICE", "ID"));

            //Детализация для РСО и УО по Дому в Периоде
            this.Database.AddEntityTable(
                "GKH_CONTR_PERIOD_SUMM_DETAIL",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_DETAIL_RO_ID", "GKH_PUB_SERV_ORG_CONTR_REAL_OBJ", "ID"),
                new RefColumn("PER_SUMM_ID", ColumnProperty.NotNull, "GKH_CONTR_PERIOD_SUMM_DETAIL_PER_SUMM_ID", "GKH_CONTR_PERIOD_SUMM", "ID"),
                new Column("START_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHARGED_RESID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("RECALC_PREV_PERIOD", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHANGE_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("NO_DELIV_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID_RESID", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("END_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHARGED_TO_PAY", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("TRANSFER_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CHARGED_MAN_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID_MAN_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("SALDO_OUT", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("BILL_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("PAID_PUB_SERV_ORG", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        /// <summary>
        /// Откат
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_CONTR_PERIOD_SUMM_DETAIL");
            this.Database.RemoveTable("GKH_CONTR_PERIOD_SUMM");
            this.Database.RemoveTable("GKH_CONTR_PERIOD_SUMM_UO");
            this.Database.RemoveTable("GKH_CONTR_PERIOD_SUMM_RSO");
            this.Database.RemoveTable("GKH_CONTRACT_PERIOD");
        }
    }
}