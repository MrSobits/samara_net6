namespace Bars.Gkh.Gis.Migrations._2015.Version_2015072200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015072200")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2015.Version_2015071600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_INTEGR_METHOD",
                new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("DATE_INTEG", DbType.DateTime));

            this.Database.AddEntityTable("GIS_INTEGR_DICT",
                new Column("GIS_DICT_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("GIS_DICT_NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("GKH_DICT_NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("CNT_REF_RECS", DbType.Int32),
                new Column("CNT_NOT_REF_RECS", DbType.Int32),
                new Column("DATE_INTEG", DbType.DateTime));

            this.Database.AddEntityTable("GIS_INTEGR_LOG",
                new Column("LINK", DbType.String, 2000),
                new Column("METHOD_NAME", DbType.String, 300),
                new Column("USER_NAME", DbType.String, 300),
                new Column("COUNT_OBJECTS", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("PORTION_OBJECTS", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("RESULT_TEXT", DbType.String, 2000),
                new Column("TYPE_OPERATION", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("TYPE_COMPLETE", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("PROCESSED_OBJECTS", DbType.Int32),
                new Column("PROCESSED_PORTIONS", DbType.Int32),
                new Column("PROCESSED_PERCENT", DbType.Decimal));

            this.Database.AddEntityTable("GIS_INTEGR_REF_DICT",
                new Column("CLASS_NAME", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("GIS_REC_ID", DbType.Int64),
                new Column("GIS_REC_NAME", DbType.String, 1000),
                new Column("GKH_REC_ID", DbType.Int64),
                new Column("GKH_REC_NAME", DbType.String, 1000));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_INTEGR_REF_DICT");

            this.Database.RemoveTable("GIS_INTEGR_LOG");

            this.Database.RemoveTable("GIS_INTEGR_DICT");

            this.Database.RemoveTable("GIS_INTEGR_METHOD");
        }
    }
}