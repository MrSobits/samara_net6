namespace Bars.Gkh.Gis.Migrations._2015.Version_2015083000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015083000")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015082200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (this.Database.TableExists("GIS_INTEGR_METHOD"))
            {
                this.Database.RemoveTable("GIS_INTEGR_METHOD");
            }
            if (this.Database.TableExists("GIS_INTEGR_DICT"))
            {
                this.Database.RemoveTable("GIS_INTEGR_DICT");
            }
            if (this.Database.TableExists("GIS_INTEGR_LOG"))
            {
                this.Database.RemoveTable("GIS_INTEGR_LOG");
            }
            if (this.Database.TableExists("GIS_INTEGR_REF_DICT"))
            {
                this.Database.RemoveTable("GIS_INTEGR_REF_DICT");
            }
        }

        public override void Down()
        {
            this.Database.AddEntityTable("GIS_INTEGR_METHOD",
                new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 1000),
                new Column("DATE_INTEG", DbType.DateTime),
                new Column("METHODCODE", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("SERVICE_ADDRESS", DbType.String, 1000, ColumnProperty.NotNull));

            this.Database.AddEntityTable("GIS_INTEGR_DICT",
                new Column("NAME", DbType.String, 500),
                new Column("ACTION_CODE", DbType.String, 500),
                new Column("REGISTRY_NUMBER", DbType.String, 50),
                new Column("DATE_INTEG", DbType.DateTime));

            this.Database.AddEntityTable("GIS_INTEGR_LOG",
                new RefColumn("FILELOG_ID", "GIS_FILE_LOG", "B4_FILE_INFO", "ID"),
                new Column("LINK", DbType.String, 2000),
                new Column("METHOD_NAME", DbType.String, 300),
                new Column("USER_NAME", DbType.String, 300),
                new Column("COUNT_OBJECTS", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("PROCESSED_OBJECTS", DbType.Int32),
                new Column("PROCESSED_PERCENT", DbType.Decimal));

            this.Database.AddEntityTable("GIS_INTEGR_REF_DICT",
                new RefColumn("DICT_ID", "GIS_INTEGR_REF_DICT", "GIS_INTEGR_DICT", "ID"),
                new Column("CLASS_NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("GIS_REC_ID", DbType.Int64),
                new Column("GIS_REC_NAME", DbType.String, 1000),
                new Column("GKH_REC_ID", DbType.Int64),
                new Column("GKH_REC_NAME", DbType.String, 1000));
        }
    }
}