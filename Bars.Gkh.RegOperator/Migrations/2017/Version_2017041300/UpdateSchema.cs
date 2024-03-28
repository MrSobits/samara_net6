namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017041300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция RegOperator 2017041300
    /// </summary>
    [Migration("2017041300")]
    [MigrationDependsOn(typeof(Version_2017041200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERF_WORK_CHARGE", new Column("IS_ACTIVE", DbType.Boolean));
            this.Database.ExecuteNonQuery("UPDATE REGOP_PERF_WORK_CHARGE set IS_ACTIVE = true where IS_ACTIVE is null");
            this.Database.AlterColumnSetNullable("REGOP_PERF_WORK_CHARGE", "IS_ACTIVE", false);
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERF_WORK_CHARGE", "IS_ACTIVE");
        }
    }
}
