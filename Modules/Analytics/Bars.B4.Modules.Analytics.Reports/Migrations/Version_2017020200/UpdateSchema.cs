namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2017020200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017020200
    /// </summary>
    [Migration("2017020200")]
    [MigrationDependsOn(typeof(Version_2015101200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("AL_STORED_REPORT", "ON_CALC_SERVER", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("AL_STORED_REPORT", "ON_CALC_SERVER");
        }
    }
}