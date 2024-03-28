namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016040400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016.04.04.00
    /// </summary>
    [Migration("2016040400")]
    [MigrationDependsOn(typeof(Version_2016032200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "REGOP_RECALC_HISTORY",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "RECALC_HISTORY_PERS_ACC", "REGOP_PERS_ACC", "ID"),
                new RefColumn("CALC_PERIOD_ID", ColumnProperty.NotNull, "RECALC_HISTORY_CALC_PER", "REGOP_PERIOD", "ID"),
                new RefColumn("RECALC_PERIOD_ID", ColumnProperty.NotNull, "RECALC_HISTORY_RECALC_PER", "REGOP_PERIOD", "ID"),
                new Column("RECALC_PENALTY", DbType.Decimal, ColumnProperty.NotNull, 0m),
                new Column("UNACCEPTED_GUID", DbType.String, 50, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_RECALC_HISTORY");
        }
    }
}
