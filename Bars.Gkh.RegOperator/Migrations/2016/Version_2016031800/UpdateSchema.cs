namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016031800
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 20160318
    /// </summary>
    [Migration("2016031800")]
    [MigrationDependsOn(typeof(Version_2016012500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "REGOP_PENALTIES_DEFERRED",
                new Column("DATE_START", DbType.DateTime, 512, ColumnProperty.Null),
                new Column("DATE_END", DbType.DateTime, 512, ColumnProperty.Null));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PENALTIES_DEFERRED");
        }
    }
}
