namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016032200
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016.03.22.00
    /// </summary>
    [Migration("2016032200")]
    [MigrationDependsOn(typeof(Version_2016012500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("PERF_WORK_CHARGE", DbType.Decimal, ColumnProperty.NotNull, 0m));
            
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "PERF_WORK_CHARGE");
        }
    }
}
