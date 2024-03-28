namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017020700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция 2017020700
    /// </summary>
    [Migration("2017020700")]
    [MigrationDependsOn(typeof(Version_2017020300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERF_WORK_CHARGE_SOURCE", new Column("DISTRIBUTE_FOR_BASE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("REGOP_PERF_WORK_CHARGE_SOURCE", new Column("DISTRIBUTE_FOR_DECISION", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("REGOP_PERF_WORK_CHARGE", new Column("DISTRIBUTE_TYPE", DbType.Int32));

            this.Database.ExecuteNonQuery("UPDATE REGOP_PERF_WORK_CHARGE_SOURCE SET DISTRIBUTE_FOR_BASE = true");
            this.Database.ExecuteNonQuery("UPDATE REGOP_PERF_WORK_CHARGE SET DISTRIBUTE_TYPE = 0");

            this.Database.AlterColumnSetNullable("REGOP_PERF_WORK_CHARGE", "DISTRIBUTE_TYPE", false);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERF_WORK_CHARGE_SOURCE", "DISTRIBUTE_FOR_BASE");
            this.Database.RemoveColumn("REGOP_PERF_WORK_CHARGE_SOURCE", "DISTRIBUTE_FOR_DECISION");

            this.Database.RemoveColumn("REGOP_PERF_WORK_CHARGE", "DISTRIBUTE_TYPE");
        }
    }
}
