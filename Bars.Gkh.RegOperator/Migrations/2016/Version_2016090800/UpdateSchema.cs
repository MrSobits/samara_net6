namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016090800
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016090800
    /// </summary>
    [Migration("2016090800")]
    [MigrationDependsOn(typeof(Version_2016090100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_CHARGE_OPERATION_BASE",
                new Column("DOC_NUM", DbType.String, 200),
                new Column("OP_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("FACT_OP_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("REASON", DbType.String, 200),
                new Column("USER_NAME", DbType.String, 100),
                new Column("CHARGE_SOURCE", DbType.Int32, ColumnProperty.NotNull),
                new Column("GUID", DbType.String, 250));
       
            this.Database.AddTable("REGOP_CHARGE_CANCEL_SOURCE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.Database.AddForeignKey("FK_REGOP_CHARGE_CANCEL_SOURCE", "REGOP_CHARGE_CANCEL_SOURCE", "ID", "REGOP_CHARGE_OPERATION_BASE", "ID");

            this.Database.AddEntityTable("REGOP_CANCEL_CHARGE",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "CANCELED_CHARGE_PERS_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("CANCEL_PERIOD_ID", ColumnProperty.NotNull, "CANCELED_CHARGE_CANCEL_PER", "REGOP_PERIOD", "ID"),
                new RefColumn("CHARGE_OP_ID", ColumnProperty.NotNull, "CANCELED_CHARGE_OPER", "REGOP_CHARGE_OPERATION_BASE", "ID"),
                new Column("CANCEL_SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("CANCEL_TYPE", DbType.Int16, ColumnProperty.NotNull, 0));

            //меняем знак у отмен начислений и tarfet_coef на (-1)
            const string updateCancelChargeTransfersQuery = @"
                    UPDATE regop_transfer set target_coef=-1, amount=-amount
                    where reason='Отмена начислений по базовому тарифу' or reason='Отмена начислений по тарифу решений'";

            this.Database.ExecuteNonQuery(updateCancelChargeTransfersQuery);
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_CANCEL_CHARGE");
            this.Database.RemoveTable("REGOP_CHARGE_CANCEL_SOURCE");
            this.Database.RemoveTable("REGOP_CHARGE_OPERATION_BASE");
        }
    }
}
