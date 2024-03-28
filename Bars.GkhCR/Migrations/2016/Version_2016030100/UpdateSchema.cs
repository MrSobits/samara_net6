namespace Bars.GkhCr.Migrations._2016.Version_2016030100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016030100")]
    [MigrationDependsOn(typeof(_2015.Version_2015121500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("CR_DICT_PROGRAM", new Column("ADD_WORK_FROM_LONG_PROG", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "ADD_WORK_FROM_LONG_PROG");
        }
    }
}