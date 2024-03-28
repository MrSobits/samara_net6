namespace Bars.GkhGji.Migrations._2020.Version_2020090800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020090800")]
    [MigrationDependsOn(typeof(Version_2020090700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GJI_MKD_LIC_STATEMENT",
               new Column("CONCLUSION_DATE", DbType.DateTime, ColumnProperty.None),
               new Column("CONCLUSION_NUMBER", DbType.String, 255,ColumnProperty.None),
               new Column("DESCRIPTION", DbType.String, 500, ColumnProperty.None),
               new Column("STATEMENT_RESULT", DbType.Int32, 4, ColumnProperty.NotNull, 0),
               new Column("RESULT_COMMENT", DbType.String, 1000, ColumnProperty.None),
               new Column("OBJECTION", DbType.Boolean, false),
               new Column("OBJECTION_RESULT", DbType.Int32, 4, ColumnProperty.NotNull, 0),
               new Column("FIO", DbType.String, 300, ColumnProperty.None),
               new Column("STATEMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
               new Column("STATEMENT_NUMBER", DbType.String, 255, ColumnProperty.None),
               new RefColumn("STATE_ID", "GJI_MKD_LIC_STATEMENT_STATE", "B4_STATE", "ID"),
               new RefColumn("CONTRAGENT_ID", "GJI_MKD_LIC_STATEMENT_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
               new RefColumn("EXECUTANT_TYPE_ID", "GJI_MKD_LIC_STATEMENT_EXECUTANT", "GJI_DICT_EXECUTANT", "ID"),
               new RefColumn("INSPECTOR_ID", "GJI_MKD_LIC_STATEMENT_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
               new RefColumn("TYPE_REQUEST_ID", "GJI_MKD_LIC_STATEMENT_TYPE_REQUEST", "GJI_DICT_MKD_LIC_TYPE_REQUEST", "ID"),
               new RefColumn("RO_ID", "GJI_MKD_LIC_STATEMENT_RO", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("STMT_CONTRAGENT_ID", "GJI_MKD_LIC_STATEMENT_STMNT_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable(
              "GJI_MKD_LIC_STATEMENT_FILE",
              new Column("DOC_NAME", DbType.String, 1500),
              new Column("DOC_DATE", DbType.DateTime, ColumnProperty.None),
              new Column("DESCRIPTION", DbType.String, 1500),
              new Column("DOC_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
              new RefColumn("REQUEST_ID", ColumnProperty.NotNull, "GJI_MKD_LIC_STATEMENT_FILE_STMNT", "GJI_MKD_LIC_STATEMENT", "ID"),
              new RefColumn("FILE_ID", ColumnProperty.NotNull, "GJI_MKD_LIC_STATEMENT_FILE_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_MKD_LIC_STATEMENT_FILE");
            Database.RemoveTable("GJI_MKD_LIC_STATEMENT");
        }
    }
}