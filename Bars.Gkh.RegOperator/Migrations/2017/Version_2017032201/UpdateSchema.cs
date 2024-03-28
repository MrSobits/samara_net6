namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017032201
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017.03.22.01
    /// </summary>
    [Migration("2017032201")]
    [MigrationDependsOn(typeof(Version_2017031500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("PERF_WORK_CHARGE_DEC", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "PERF_WORK_CHARGE_DEC");
        }
    }
}
