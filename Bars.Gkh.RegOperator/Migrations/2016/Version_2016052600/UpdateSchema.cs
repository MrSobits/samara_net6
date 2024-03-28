namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016052600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016.05.26.00
    /// </summary>
    [Migration("2016052600")]
    [MigrationDependsOn(typeof(Version_2016041200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn(
                "REGOP_RECALC_HISTORY",
                new Column("RECALC_TYPE", DbType.Int16, ColumnProperty.NotNull, 0));

            this.Database.RenameColumn("REGOP_RECALC_HISTORY", "RECALC_PENALTY", "RECALC_SUM");

            this.Database.ExecuteNonQuery("update REGOP_RECALC_HISTORY set RECALC_TYPE = 30");

        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_RECALC_HISTORY", "RECALC_TYPE");

            this.Database.RenameColumn("REGOP_RECALC_HISTORY", "RECALC_SUM", "RECALC_PENALTY");
        }
    }
}
