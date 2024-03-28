namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016121300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2016121300
    /// </summary>
    [Migration("2016121300")]
    [MigrationDependsOn(typeof(Version_2016110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_FIX_PER_CALC_PENALTIES", 
                new Column("START_DAY", DbType.Int32, ColumnProperty.NotNull),
                new Column("END_DAY", DbType.Int32, ColumnProperty.NotNull),
                new Column("DATE_START", DbType.Date, ColumnProperty.NotNull),
                new Column("DATE_END", DbType.Date));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_FIX_PER_CALC_PENALTIES");
        }
    }
}
