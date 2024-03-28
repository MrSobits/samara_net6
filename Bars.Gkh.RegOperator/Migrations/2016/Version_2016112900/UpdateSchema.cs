namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016112900
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016112900
    /// </summary>
    [Migration("2016112900")]
    [MigrationDependsOn(typeof(Version_2016111500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddTable("REGOP_PERF_WORK_CHARGE_SOURCE",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("IS_DISTRIBUTED", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_PERF_WORK_CHARGE_PERS_ACC", "REGOP_PERS_ACC", "ID"));

            this.Database.AddForeignKey("FK_REGOP_PERF_WORK_CHARGE_SOURCE", "REGOP_PERF_WORK_CHARGE_SOURCE", "ID", "REGOP_CHARGE_OPERATION_BASE", "ID");
            this.Database.AddIndex("PERF_WORK_CHARGE_SRC_IND", false, "REGOP_PERF_WORK_CHARGE_SOURCE", "ID");

            this.Database.AddEntityTable(
                "REGOP_PERF_WORK_CHARGE",
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0m),               
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "REGOP_PERF_WORK_CHARGE_CANCEL_PER", "REGOP_PERIOD", "ID"),
                new RefColumn("CHARGE_OP_ID", ColumnProperty.NotNull, "REGOP_PERF_WORK_CHARGE_OPER", "REGOP_CHARGE_OPERATION_BASE", "ID"));
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PERF_WORK_CHARGE");
            this.Database.RemoveTable("REGOP_PERF_WORK_CHARGE_SOURCE");
        }
    }
}
