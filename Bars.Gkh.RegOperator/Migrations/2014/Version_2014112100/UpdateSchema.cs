namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014112100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014111800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_FS_IMPORT_INFO",
                new Column("CODE", DbType.String, 100, ColumnProperty.NotNull),
                new Column("DATA_HEAD_INDEX", DbType.Int32, ColumnProperty.NotNull),
                new Column("DESCR", DbType.String, 3000, ColumnProperty.Null),
                new Column("NAME", DbType.String, 200, ColumnProperty.NotNull));

            Database.AddEntityTable("REGOP_FS_IMPORT_MAP_ITEM",
                new Column("ERROR_TEXT", DbType.String, 500, ColumnProperty.Null),
                new Column("GET_VAL_FROM_REGEX", DbType.Boolean, ColumnProperty.NotNull),
                new Column("ITEM_INDEX", DbType.Int32, ColumnProperty.NotNull),
                new Column("IS_META", DbType.Boolean, ColumnProperty.NotNull),
                new Column("PROPERTY_NAME", DbType.String, 20, ColumnProperty.NotNull),
                new Column("REGEX_SUCC_VAL", DbType.String, 100, ColumnProperty.Null),
                new RefColumn("INFO_ID", ColumnProperty.NotNull, "REGOP_FS_IMP_MAP_INF", "REGOP_FS_IMPORT_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_FS_IMPORT_MAP_ITEM");
            Database.RemoveTable("REGOP_FS_IMPORT_INFO");
        }
    }
}
