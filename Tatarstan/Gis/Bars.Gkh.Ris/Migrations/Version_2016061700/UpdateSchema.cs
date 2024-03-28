namespace Bars.Gkh.Ris.Migrations.Version_2016061700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016061700")]
    [MigrationDependsOn(typeof(Version_2016061000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.RemoveTable("RIS_INTEGR_LOG");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.AddEntityTable("RIS_INTEGR_LOG",
            //    new RefColumn("FILELOG_ID", "RIS_FILE_LOG", "B4_FILE_INFO", "ID"),
            //    new Column("LINK", DbType.String, 2000),
            //    new Column("METHOD_NAME", DbType.String, 300),
            //    new Column("USER_NAME", DbType.String, 300),
            //    new Column("COUNT_OBJECTS", DbType.Int32, ColumnProperty.NotNull, 0),
            //    new Column("DATE_START", DbType.DateTime),
            //    new Column("DATE_END", DbType.DateTime),
            //    new Column("PROCESSED_OBJECTS", DbType.Int32),
            //    new Column("PROCESSED_PERCENT", DbType.Decimal));
        }
    }
}
