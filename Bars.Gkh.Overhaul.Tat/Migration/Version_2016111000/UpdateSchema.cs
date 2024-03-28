namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2016111000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016111000
    /// </summary>
    [Migration("2016111000")]
    [MigrationDependsOn(typeof(Version_2014110500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("OVRHL_DICT_YEAR_CORR", new Column("YEAR", DbType.Int32, ColumnProperty.NotNull));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_DICT_YEAR_CORR");
        }
    }
}