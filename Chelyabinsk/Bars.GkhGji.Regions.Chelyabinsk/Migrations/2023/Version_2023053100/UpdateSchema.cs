namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023053100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023053100")]
    [MigrationDependsOn(typeof(Version_2023041100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable("GJI_MKD_LIC_REQUEST_ANSWER",
                new RefColumn("MKD_LIC_REQUEST_ID", "GJI_ANSW_MKD_LIC_REQUEST", "GJI_MKD_LIC_STATEMENT", "ID"),
                new RefColumn("ANSWER_CONTENT_ID", "GJI_ANSW_MKD_LIC_REQUEST_CONT", "GJI_DICT_ANSWER_CONTENT", "ID"),
                new RefColumn("REVENUE_SOURCE_ID", "GJI_ANSW_MKD_LIC_REQUEST_REV", "GJI_DICT_REVENUESOURCE", "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_ANSW_MKD_LIC_REQUEST_INSP", "GKH_DICT_INSPECTOR", "ID"),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUM", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.Date),
                new Column("DESCRIPTION", DbType.String, 2000),
                new RefColumn("FILE_INFO_ID", "GJI_ANSW_MKD_LIC_REQUEST_FI", "B4_FILE_INFO", "ID"),
                new Column("EXTERNAL_ID", DbType.String, 36),

                new Column("IS_MOVED", DbType.Boolean),
                new Column("YEAR", DbType.Int32),
                new Column("EXEC_DATE", DbType.Date),
                new Column("EXTEND_DATE", DbType.Date),
                new Column("IS_UPLOADED", DbType.Boolean),
                new Column("ADDITIONAL_INFO", DbType.String, 2000),
                new Column("ANSWER_TYPE", DbType.Int16, ColumnProperty.NotNull, (int)GkhGji.Enums.TypeAppealAnswer.NotSet),
                new Column("ANSWER_FINAL_TYPE", DbType.Int32, 2, ColumnProperty.NotNull, 2),
                new Column("GIS_GKH_GUID", DbType.String, 36),
                new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36),
                new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36),
                new Column("HASH", DbType.String),
                new Column("ADDRESS", DbType.String, 2000),
                new Column("SERIAL_NUMBER", DbType.String, 50),

                new RefColumn("SIGNER_ID", "GJI_MKD_LIC_REQUEST_ANSW_SIGNER_ID", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("FILE_DOC_INFO_ID", "GJI_MKD_LIC_REQUEST_ANSW_FI_DOC", "B4_FILE_INFO", "ID"),
                new RefColumn("STATE_ID", "GJI_MKD_LIC_REQUEST_ANSWER_ST", "B4_STATE", "ID"),
                new RefColumn("CONCED_RESULT_ID", "GJI_MKD_LIC_REQUEST_ANSWER_CONCEDRESULT_ID", "GJI_DICT_CONCEDERATION_RESULT", "ID"),
                new RefColumn("FACT_CHECK_TYPE_ID", "GJI_MKD_LIC_REQUEST_ANSWER_FACTCHECKTYPE_ID", "GJI_DICT_FACT_CHECKING_TYPE", "ID"),
                new RefColumn("REDIRECT_CONTRAGENT_ID", "GJI_MKD_LIC_REQUEST_ANSWER_REDIRECT_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

            Database.AddEntityTable("GJI_MKD_LIC_REQUEST_HEADINSP",
                new RefColumn("MKD_LIC_REQUEST_ID", "GJI_MKD_LIC_REQUEST_HEADINSP_ID", "GJI_MKD_LIC_STATEMENT", "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_MKD_LIC_REQUEST_HEADINSP_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable("GJI_MKD_LIC_REQUEST_SOURCES",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("REVENUE_DATE", DbType.DateTime),
                new Column("SSTU_DATE", DbType.DateTime),
                new Column("REVENUE_SOURCE_NUMBER", DbType.String, 50),
                new Column("REVENUE_SOURCE_ID", DbType.Int64, 22),
                new Column("REVENUE_FORM_ID", DbType.Int64, 22),
                new RefColumn("MKD_LIC_REQUEST_ID", "GJI_MKD_LIC_REQUEST_SOURCE_ID", "GJI_MKD_LIC_STATEMENT", "ID"));

            Database.AddEntityTable("GJI_MKD_LIC_REQUEST_EXECUTANT",
                   new Column("ORDER_DATE", DbType.DateTime, ColumnProperty.NotNull),
                   new Column("PERFOM_DATE", DbType.DateTime, ColumnProperty.NotNull),
                   new Column("RESPONSIBLE", DbType.Boolean, ColumnProperty.NotNull, false),
                   new Column("DESCRIPTION", DbType.String, 255),
                   new Column("ONAPPROVAL", DbType.Boolean, ColumnProperty.NotNull, false),
                   new RefColumn("MKD_LIC_REQUEST_ID", "GJI_MKD_LIC_REQUEST_EXEC_LIC", "GJI_MKD_LIC_STATEMENT", "ID"),
                   new RefColumn("EXECUTANT_ID", "GJI_MKD_LIC_REQUEST_EXEC_EXEC", "GKH_DICT_INSPECTOR", "ID"),
                   new RefColumn("AUTHOR_ID", "GJI_MKD_LIC_REQUEST_EXEC_AUTH", "GKH_DICT_INSPECTOR", "ID"),
                   new RefColumn("STATE_ID", "GJI_MKD_LIC_REQUEST_EXEC_STATE", "B4_STATE", "ID"),
                   new RefColumn("CONTROLLER_ID", "GJI_MKD_LIC_REQUEST_EXEC_CTRL", "GKH_DICT_INSPECTOR", "ID"),
                   new RefColumn("RESOLUTION_ID", "GJI_MKD_LIC_REQUEST_EXEC_RESOLUTION", "B4_FILE_INFO", "ID"),
                   new RefColumn("ZONAINSP_ID", "GJI_MKD_LIC_REQUEST_EXEC_ZONAINSP", "GKH_DICT_ZONAINSP", "ID"));

            Database.AddEntityTable("GJI_MKD_LIC_REQUEST_ANSWER_FILES",
                new Column("NAME", DbType.String, 250, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new RefColumn("MKD_LIC_REQUEST_ANSWER_ID", "MKD_LIC_REQUEST_ANSWER_FILE_ANSWER_ID", "GJI_MKD_LIC_REQUEST_ANSWER", "ID"),
                new RefColumn("FILE_INFO_ID", "MKD_LIC_REQUEST_ANSWER_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_ANSWER_FILES");
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_EXECUTANT");
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_SOURCES");
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_HEADINSP");
            Database.RemoveTable("GJI_MKD_LIC_REQUEST_ANSWER");
        }
    }
}