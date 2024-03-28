namespace Bars.Gkh.Reforma.Migrations.Version_2016022600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция [2016022600]
    /// </summary>
    [Migration("2016022600")]
    [MigrationDependsOn(typeof(Bars.Gkh.Reforma.Migrations.Version_2015101000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("RFRM_FILE", new RefColumn("REPORTING_PERIOD_ID", ColumnProperty.None, "REPORTING_PERIOD", "RFRM_REPORTING_PERIOD", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("RFRM_FILE", "REPORTING_PERIOD_ID");
        }
    }
}
