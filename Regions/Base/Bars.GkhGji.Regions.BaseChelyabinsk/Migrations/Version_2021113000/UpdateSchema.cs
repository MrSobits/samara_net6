namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021113000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021113000")]
    [MigrationDependsOn(typeof(Version_2021111800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
         "SMEV_CH_COMPLAINTS",
             new RefColumn("INSPECTOR_ID", ColumnProperty.None, "SMEV_CH_COMPLAINTS_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
             new Column("COMPLAINT_ID", DbType.String, 30),
             new Column("CMP_NUMBER", DbType.String, 20),
             new Column("COMMENT_INFO", DbType.String, 4000),
             new Column("ESIA_OID", DbType.String, 100),
             new Column("REQUESTER_ROLE", DbType.Int32, 4, ColumnProperty.NotNull, 0),
             new RefColumn("CONTRAGENT_ID", "SMEV_CH_COMPLAINTS_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"),
             new Column("REQUESTER_FIO", DbType.String, 500),
             new Column("DOC_FL_TYPE", DbType.Int32, 4, ColumnProperty.None, 0),
             new Column("DOC_FL_SERIES", DbType.String, 8),
             new Column("DOC_FL_NUMBER", DbType.String, 10),
             new Column("INN_FL", DbType.String, 20),
             new Column("SNILS", DbType.String, 30),
             new Column("BIRTH_DATE", DbType.DateTime, ColumnProperty.None),
             new Column("BIRTH_PLACE", DbType.String, 500),
             new Column("GENDER", DbType.Int32, 4, ColumnProperty.None, 0),
             new Column("NATION", DbType.String, 50),
             new Column("REG_ADDRESS", DbType.String, 500),
             new Column("EMAIL", DbType.String, 50),
             new Column("MOBILE", DbType.String, 50),
             new Column("ORGNIP", DbType.String, 30),
             new Column("LEGAL_NAME", DbType.String, 600),
             new Column("OGRN", DbType.String, 30),
             new Column("INN", DbType.String, 20),
             new Column("LEGAL_ADDRESS", DbType.String, 600),
             new Column("POSITION", DbType.String, 200),
             new Column("ORDERID", DbType.String, 50),
             new Column("IS_REVOKE", DbType.Boolean, ColumnProperty.NotNull, false),
             new Column("COMPL_DATE", DbType.String, 20),
             new Column("OKATO", DbType.String, 30),
             new Column("APPEAL_NUMBER", DbType.String, 50),
             new Column("TYPE_APPEAL_DECISION", DbType.String, 500),
             new Column("EVENT", DbType.String, 500),
             new Column("REQUESTID", DbType.String, 500),
             new Column("ENTRY_ID", DbType.String, 500),
             new Column("REQUEST_DATE", DbType.DateTime, ColumnProperty.NotNull),
             new Column("ANSWER", DbType.String, 1000),
             new Column("MESSAGE_ID", DbType.String, 1000),
             new RefColumn("STATE_ID", "SMEV_CH_COMPLAINTS_STATE", "B4_STATE", "ID"),
             new RefColumn("FILE_INFO_ID", ColumnProperty.None, "SMEV_CH_COMPLAINTS_FILE_ID", "B4_FILE_INFO", "ID"));

            this.Database.AddEntityTable("SMEV_CH_COMPLAINTS_EXECUTANT",
                  new Column("ORDER_DATE", DbType.DateTime, ColumnProperty.NotNull),
                  new Column("PERFOM_DATE", DbType.DateTime, ColumnProperty.NotNull),
                  new Column("RESPONSIBLE", DbType.Boolean, ColumnProperty.NotNull, false),
                  new Column("DESCRIPTION", DbType.String, 255),
                  new RefColumn("COMPLAINT_ID", ColumnProperty.NotNull, "COMPLAINTS_EXECUTANT_COMPLAINT", "SMEV_CH_COMPLAINTS", "ID"),
                  new RefColumn("EXECUTANT_ID", ColumnProperty.NotNull, "COMPLAINTS_EXECUTANT_EXEC", "GKH_DICT_INSPECTOR", "ID"),
                  new RefColumn("AUTHOR_ID", ColumnProperty.Null, "COMPLAINTS_EXECUTANT_AUTH", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
             "SMEV_CH_COMPLAINTS_REQUEST",
             new Column("ANSWER", DbType.String, 500),
             new Column("REQUEST_TEXT", DbType.String, 10000),
             new Column("COMPLAINT_ID", DbType.Int64, ColumnProperty.Null),
             new Column("TYPE_REQUEST", DbType.Int32, 4, ColumnProperty.NotNull, 0),
             new Column("REQ_DATE", DbType.DateTime, ColumnProperty.NotNull),
             new Column("REQUEST_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
             new Column("MESSAGE_ID", DbType.String, 500),
           new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "SMEV_CH_COMPLAINTS_REQUEST_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddEntityTable(
                "SMEV_CH_COMPLAINTS_REQUEST_FILE",
                new Column("FILE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull),
                new RefColumn("SMEV_REQUEST_ID", ColumnProperty.None, "SMEV_COMPLAINTS_REQUEST_REQUEST", "SMEV_CH_COMPLAINTS_REQUEST", "ID"),
                new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "SMEV_COMPLAINTS_REQUEST_FILE_INFO_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("SMEV_CH_COMPLAINTS_REQUEST_FILE");
            Database.RemoveTable("SMEV_CH_COMPLAINTS_REQUEST");
            Database.RemoveTable("SMEV_CH_COMPLAINTS_EXECUTANT");
            Database.RemoveTable("SMEV_CH_COMPLAINTS");
        }

    }
}