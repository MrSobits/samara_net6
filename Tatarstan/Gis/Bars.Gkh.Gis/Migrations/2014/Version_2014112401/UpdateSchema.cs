namespace Bars.Gkh.Gis.Migrations._2014.Version_2014112401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Gis.Enum;

    [Migration("2014112401")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddTable("GIS_LOADED_FILE_REGISTER",
                new Column("ID", DbType.Int64, (ColumnProperty.NotNull | ColumnProperty.Unique)),
                new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("B4_USER_ID", ColumnProperty.NotNull, "LOA_FIL_REG_B4_USR", "B4_USER", "ID"),
                new Column("FILENAME", DbType.String, 200, ColumnProperty.NotNull),
                new Column("SIZE", DbType.Int64, ColumnProperty.NotNull),
                new Column("TYPESTATUS", DbType.Int64, ColumnProperty.NotNull, (int)TypeStatus.InProgress),
                new Column("SUPPLIER_NAME", DbType.String, 200, ColumnProperty.NotNull),
                new RefColumn("B4_LOG_INFO_ID", "GIS_LOA_FIL_REG_B4_LOG_INF", "B4_FILE_INFO", "ID"),
                new RefColumn("B4_FILE_INFO_ID", "GIS_LOA_FIL_REG_B4_FIL_INF", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_LOADED_FILE_REGISTER");
        }
    }
}