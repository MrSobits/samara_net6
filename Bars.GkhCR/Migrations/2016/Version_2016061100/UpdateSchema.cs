namespace Bars.GkhCr.Migrations._2016.Version_2016061100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015061100
    /// </summary>
    [Migration("2015061100"), MigrationDependsOn(typeof(Version_2016030100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_PROTOCOL", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_DEFECT_LIST", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_DESIGN_ASSIGNMENT", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_ESTIMATE_CALC", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_CONTRACT", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_DOCUMENT_WORK", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("USED_IN_EXPORT", DbType.Int32, 20, ColumnProperty.NotNull, 20));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_PROTOCOL", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_DEFECT_LIST", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_DESIGN_ASSIGNMENT", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_ESTIMATE_CALC", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_CONTRACT", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_DOCUMENT_WORK", "USED_IN_EXPORT");
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "USED_IN_EXPORT");
        }
    }
}