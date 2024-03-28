namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2023040400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023040400")]
    [MigrationDependsOn(typeof(Version_2023031100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("ACCESSGUID", DbType.String, 100));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("FIZ_INN", DbType.String, 30));
            Database.AddRefColumn("GJI_APPCIT_ADMONITION", new RefColumn("ERKNM_REASON", "GJI_APPCADMON_ERKNM_REASON", "GJI_DICT_DECISION_REASON_ERKNM", "ID"));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("FIZ_ADDR", DbType.String, 250));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("TYPE_RISK", DbType.Int16, ColumnProperty.NotNull, 0));
            Database.AddRefColumn("GJI_APPCIT_ADMONITION", new RefColumn("SURVEY_SUBJECT_ID", "GJI_APPCADMON_SURV_SUBJ_ID", "GJI_DICT_SURVEY_SUBJ", "ID"));

            Database.AddRefColumn("GJI_CH_GIS_ERKNM", new RefColumn("ADMON_ID", "GJI_CH_GIS_ERKNM_ADMONITION", "GJI_APPCIT_ADMONITION", "ID"));
            Database.AddColumn("GJI_CH_GIS_ERKNM", new Column("DOC_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("ERKNMID", DbType.String, 100));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("KIND_KND", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("ERKNMGUID", DbType.String, 100));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("IS_SENT", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("PAYER_TYPE_APPCIT_ADMONITION", DbType.Int32, ColumnProperty.NotNull, 30));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("INN_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("KPP_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("FIO_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("DOCUMENT_NUMB_FIZ_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_APPCIT_ADMONITION", new Column("DOCUMENT_SERIAL_APPCIT_ADMONITION", DbType.String));
            Database.AddRefColumn("GJI_APPCIT_ADMONITION",
                new RefColumn("PP_DOC_TYPE_APPCIT_ADMONITION_ID", ColumnProperty.None, "DOC_TYPE_ADMONITION_PHYSICAL_PERSON_DOC_TYPE", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddRefColumn("GJI_APPCIT_ADMONITION", new RefColumn("REALITY_OBJECT_ID", ColumnProperty.None, "GJI_APP_ADMON_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable(
        "GJI_CH_APPCIT_ADMON_APPEAL",
        new RefColumn("APPCIT_ADMONITION_ID", ColumnProperty.None, "GJI_CH_APPEAL_APPCIT_ADMONITION_ID", "GJI_APPCIT_ADMONITION", "ID"),
        new RefColumn("APPCIT_ID", ColumnProperty.None, "GJI_CH_APPEAL_APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID"));

            this.Database.AddEntityTable(
        "GJI_CH_APPCIT_ADMONITION_LTEXT",
        new RefColumn("ADMONITION_ID", ColumnProperty.NotNull, "GJI_APPCIT_ADMONITION_LTEXT_ADMON", "GJI_APPCIT_ADMONITION", "ID"),
        new Column("MEASURES", DbType.Binary),
        new Column("VIOLATION", DbType.Binary));

            Database.AddEntityTable(
               "GJI_CH_APPCIT_ADMON_ANNEX",
               new Column("FILE_ID", DbType.Int64, 22),
               new Column("DOCUMENT_DATE", DbType.DateTime),
               new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
               new Column("DESCRIPTION", DbType.String, 500),
               new Column("EXTERNAL_ID", DbType.String, 36),
               new RefColumn("APPCIT_ADMONITION_ID", "GJI_CH_APPCIT_ADMONANNEX_ADMON_ID", "GJI_APPCIT_ADMONITION", "ID"),
               new RefColumn("SIGNED_FILE_ID", "GJI_CH_APPCIT_ADMON_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"),
               new RefColumn("SIGNATURE_FILE_ID", "GJI_CH_APPCIT_ADMON_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"),
               new RefColumn("CERTIFICATE_FILE_ID", "GJI_CH_APPCIT_ADMON_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"),
               new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));



        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_APPCIT_ADMON_ANNEX");
            this.Database.RemoveTable("GJI_CH_APPCIT_ADMONITION_LTEXT");
            Database.RemoveTable("GJI_CH_APPCIT_ADMON_APPEAL");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "IS_SENT");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "ERKNMID");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "ERKNMGUID");
            Database.RemoveColumn("GJI_CH_GIS_ERKNM", "DOC_TYPE");
            Database.RemoveColumn("GJI_CH_GIS_ERKNM", "ADMON_ID");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "PP_DOC_TYPE_APPCIT_ADMONITION_ID");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "DOCUMENT_SERIAL_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "DOCUMENT_NUMB_FIZ_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "FIO_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "KPP_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "INN_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "PAYER_TYPE_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "REALITY_OBJECT_ID");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "SURVEY_SUBJECT_ID");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "TYPE_RISK");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "FIZ_ADDR");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "ERKNM_REASON");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "FIZ_INN");
            Database.RemoveColumn("GJI_APPCIT_ADMONITION", "ACCESSGUID");
        }
    }
}