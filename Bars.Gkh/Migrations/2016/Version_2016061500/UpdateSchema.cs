namespace Bars.Gkh.Migrations._2016.Version_2016061500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016.06.15.00
    /// </summary>
    [Migration("2016061500")]
    [MigrationDependsOn(typeof(Version_2016061300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT_DOCUMENTATION", "NAME");
            this.Database.AddColumn("CLW_LAWSUIT_DOCUMENTATION", new Column("TYPE_DOC", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LAWSUIT_DOCUMENTATION", "TYPE_DOC");
            this.Database.AddColumn("CLW_LAWSUIT_DOCUMENTATION", new Column("NAME", DbType.String, 50, ColumnProperty.NotNull));
        }
    }
}
