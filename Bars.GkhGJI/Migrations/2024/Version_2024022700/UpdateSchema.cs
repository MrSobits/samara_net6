namespace Bars.GkhGji.Migrations._2024.Version_2024022700
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024022700")]
    [MigrationDependsOn(typeof(Version_2024022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_MOTIVATED_PRESENTATION",
               new RefColumn("REALITY_OBJECT_ID", ColumnProperty.None, "GJI_MOTIVATED_PRESENTATION_RO", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_MOTIVATED_PRESENTATION_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
               new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.None),
               new Column("DOC_NUMBER", DbType.String, 50),
               new RefColumn("EXECUTOR_ID", ColumnProperty.None, "GJI_MOTIVATED_PRESENTATION_EXECUTOR", "GKH_DICT_INSPECTOR", "ID"),
               new RefColumn("HEADER_ID", ColumnProperty.None, "GJI_MOTIVATED_PRESENTATION_HEADER", "GKH_DICT_INSPECTOR", "ID"),
               new Column("VIOLATOR_TYPE", DbType.Int32, ColumnProperty.NotNull, 30),
               new RefColumn("FILE_INFO_ID", "GJI_MOTIVATED_PRESENTATION_FILE", "B4_FILE_INFO", "ID"),
               new RefColumn("SIGNED_FILE_ID", "GJI_MOTIVATED_PRESENTATION_SIGNED_FILE", "B4_FILE_INFO", "ID"),
               new RefColumn("SIGNATURE_FILE_ID", "GJI_MOTIVATED_PRESENTATION_SIGNATURE", "B4_FILE_INFO", "ID"),
               new RefColumn("CERTIFICATE_FILE_ID", "GJI_MOTIVATED_PRESENTATION_CERTIFICATE", "B4_FILE_INFO", "ID"),
               new Column("TYPE_RISK", DbType.Int16, ColumnProperty.NotNull, 0),
               new Column("MP_TYPE", DbType.Int16, ColumnProperty.NotNull, 10),
               new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0),
               new Column("FIO_APPCIT_ADMONITION", DbType.String, 500),
               new Column("FIZ_INN", DbType.String, 30),
               new Column("FIZ_ADDR", DbType.String, 250),
               new RefColumn("PP_DOC_TYPE_APPCIT_ADMONITION_ID", ColumnProperty.None, "GJI_MOTIVATED_PRESENTATION_PHYSICAL_PERSON_DOC_TYPE", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"),
               new Column("DOCUMENT_NUMB_FIZ_APPCIT_ADMONITION", DbType.String,10),
               new Column("DOCUMENT_SERIAL_APPCIT_ADMONITION", DbType.String,6),
               new RefColumn("SURVEY_SUBJECT", ColumnProperty.None, "GJI_MOTIVATED_PRESENTATION_SURVEY_SUBJ", "GJI_DICT_SURVEY_SUBJ", "ID"),
               new RefColumn("ERKNM_REASON", "GJI_MOTIVATED_PRESENTATION_ERKNM_REASON", "GJI_DICT_DECISION_REASON_ERKNM", "ID"));

            this.Database.AddEntityTable("GJI_MOTIVATED_PRESENTATION_VIOLATION",
               new Column("PLANED_DATE", DbType.DateTime),
               new Column("FACT_DATE", DbType.DateTime),
               new RefColumn("MOTIVPRES_ID", ColumnProperty.NotNull, "GJI_MOTIVATED_PRESENTATION_VIOLATION_MOTIVEPRES", "GJI_MOTIVATED_PRESENTATION", "ID"),
               new RefColumn("VIOLATION_ID", ColumnProperty.NotNull, "GJI_MOTIVATED_PRESENTATION_VIOLATION_VIOL", "GJI_DICT_VIOLATION", "ID"));

            this.Database.AddEntityTable("GJI_MOTIVATED_PRESENTATION_LTEXT",
              new RefColumn("MOTIVPRES_ID", ColumnProperty.NotNull, "GJI_MOTIVATED_PRESENTATION_LTEXT_MOTIVEPRES", "GJI_MOTIVATED_PRESENTATION", "ID"),
              new Column("MEASURES", DbType.Binary),
              new Column("VIOLATION", DbType.Binary));

            Database.AddEntityTable("GJI_MOTIVATED_PRESENTATION_APPEAL",
             new RefColumn("MOTIVPRES_ID", ColumnProperty.NotNull, "GJI_MOTIVATED_PRESENTATION_APPEAL_MOTIVEPRES", "GJI_MOTIVATED_PRESENTATION", "ID"),
             new RefColumn("APPCIT_ID", ColumnProperty.NotNull, "GJI_MOTIVATED_PRESENTATION_APPEAL_APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_MOTIVATED_PRESENTATION_APPEAL");
            this.Database.RemoveTable("GJI_MOTIVATED_PRESENTATION_LTEXT");
            this.Database.RemoveTable("GJI_MOTIVATED_PRESENTATION_VIOLATION");
            this.Database.RemoveTable("GJI_MOTIVATED_PRESENTATION");
        }
    }
}