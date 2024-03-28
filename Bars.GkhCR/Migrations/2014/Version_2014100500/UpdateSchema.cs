namespace Bars.GkhCr.Migrations.Version_2014100500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014082700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // колонки в виде работ объекта кР
            Database.AddColumn("CR_OBJ_TYPE_WORK", new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull, true));
            Database.AddColumn("CR_OBJ_TYPE_WORK", new Column("IS_DPKR_CREATED", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("CR_OBJ_TYPE_WORK", new Column("YEAR_REPAIR", DbType.Int16, 4));

            //----- История изменения вида работ
            Database.AddEntityTable(
                "CR_OBJ_TYPE_WORK_HIST",
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "CR_OBJ_TYPE_WORK_HIST_TW", "CR_OBJ_TYPE_WORK", "ID"),
                new RefColumn("FIN_SOURCE_ID", ColumnProperty.Null, "CR_OBJ_TYPE_WORK_HIST_FS", "CR_DICT_FIN_SOURCE", "ID"),
                new Column("TYPE_ACTION", DbType.Int16, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_REASON", DbType.Int16, 4, ColumnProperty.NotNull, 0),
                new Column("VOLUME", DbType.Decimal),
                new Column("SUM", DbType.Decimal),
                new Column("YEAR_REPAIR", DbType.Int16, 4),
                new Column("NEW_YEAR_REPAIR", DbType.Int16, 4));

            //----- Вид работы
            Database.AddEntityTable(
                "CR_OBJ_TYPE_WORK_REMOVAL",
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "CR_OBJ_TYPE_WORK_REM_TW", "CR_OBJ_TYPE_WORK", "ID"),
                new Column("TYPE_REASON", DbType.Int16, 4, ColumnProperty.NotNull, 0),
                new Column("FILE_DOC_ID", DbType.Int64, 22),
                new Column("NUM_DOC", DbType.String, 100),
                new Column("DATE_DOC", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 2000),
                new Column("YEAR_REPAIR", DbType.Int16, 4),
                new Column("NEW_YEAR_REPAIR", DbType.Int16, 4));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "IS_ACTIVE");
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "YEAR_REPAIR");
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "IS_DPKR_CREATED");

            Database.RemoveTable("CR_OBJ_TYPE_WORK_HIST");
            Database.RemoveTable("CR_OBJ_TYPE_WORK_REMOVAL");
        }
    }
}