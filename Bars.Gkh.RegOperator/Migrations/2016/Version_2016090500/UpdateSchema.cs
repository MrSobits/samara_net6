namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016090500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2016090500
    /// </summary>
    [Migration("2016090500")]
    [MigrationDependsOn(typeof(Version_2016082700.UpdateSchema))]
    public class UpdateSchema : Migration
    {

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_PERS_ACC_BAN_RECALC",
                new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "PERS_ACC_BAN", "REGOP_PERS_ACC","ID"),
                new RefColumn("FILE_ID", ColumnProperty.None, "PERS_ACC_BAN_FILE", "B4_FILE_INFO", "ID"),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("REASON", DbType.String, 500));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_PERS_ACC_BAN_RECALC");
        }
    }
}
