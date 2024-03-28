namespace Bars.GkhGji.Migrations._2021.Version_2021121000
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021121000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021120900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
                "GJI_DICISION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),             
                new Column("TYPE_DICISION", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime),
                new Column("TYPE_AGRPROSECUTOR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0),
                new Column("DOCUMENT_NUMBER_WITH_RESULT_AGREEMENT", DbType.String.WithSize(20)),
                new Column("TYPE_AGRRESULT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOCUMENT_DATE_WITH_RESULT_AGREEMENT", DbType.DateTime, ColumnProperty.Null),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("OBJECT_VISIT_START", DbType.DateTime),
                new Column("OBJECT_VISIT_END", DbType.DateTime),
                new RefColumn("KIND_CHECK_ID", ColumnProperty.NotNull, "GJI_DICISION_KIND_CHECK", "GJI_DICT_KIND_CHECK", "ID"),
                new RefColumn("APROOVE_FILE_ID", "GJI_DICISION_APPROVE_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("ISSUED_DECISION_ID", "GJI_DICISION_ISSUED_INSP_ID", "GKH_DICT_INSPECTOR", "ID"),
                new Column("TIME_VISIT_START", DbType.DateTime, 25),
                new Column("TIME_VISIT_END", DbType.DateTime, 25),
                new Column("APROOVE_NUMBER", DbType.String, 36),
                new Column("APROOVE_DATE", DbType.DateTime, ColumnProperty.None),
                new Column("ERPID", DbType.String, 250),
                new Column("ERKNMID", DbType.String, 250),
                new Column("FIO_DOC_APPROVE", DbType.String, 250),
                new Column("POSITION_DOC_APPROVE", DbType.String, 250));

            Database.AddEntityTable(
                "GJI_DECISION_ADMREG",
                new RefColumn("DECISION_ID", "GJI_DECISION_ADMREG_D", "GJI_DICISION", "ID"),
                new RefColumn("ADMREG_ID", "GJI_DECISION_ADMRG_AR", "GKH_DICT_NORMATIVE_DOC", "ID"));

            Database.AddEntityTable(
              "GJI_DECISION_ANNEX",
              new RefColumn("DECISION_ID", "GJI_DECISION_ANNEX_D", "GJI_DICISION", "ID"),
              new RefColumn("FILE_ID", "GJI_DECISION_ANNEX_FILE", "B4_FILE_INFO", "ID"),              
              new Column("DOCUMENT_DATE", DbType.DateTime),
              new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
              new Column("DESCRIPTION", DbType.String, 2000),
              new Column("EXTERNAL_ID", DbType.String, 36),
              new RefColumn("SIGNED_FILE_ID", "GJI_DECISION_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"),
              new RefColumn("SIGNATURE_FILE_ID", "GJI_DECISION_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"),
              new RefColumn("CERTIFICATE_FILE_ID", "GJI_DECISION_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"),
              new Column("ANNEX_TYPE", DbType.Int32, ColumnProperty.NotNull, 0),
              new Column("GIS_GKH_ATTACHMENT_GUID", DbType.String, 36),
              new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Enums.MessageCheck.NotSet));

            Database.AddEntityTable("GJI_DECISION_CON_MEASURE",
               new RefColumn("CONTROL_MEASURES_ID", ColumnProperty.Null, "GJI_DECISION_CON_MEASURE_MEASURE", "GJI_DICT_CON_ACTIVITY", "ID"),
               new Column("DATE_START", DbType.DateTime, ColumnProperty.Null),
               new Column("DATE_END", DbType.DateTime, ColumnProperty.Null),
               new Column("DESCRIPTION", DbType.String, 1500),
               new RefColumn("DECISION_ID", ColumnProperty.NotNull, "GJI_DECISION_CON_MEASURE_D", "GJI_DICISION", "ID"));

            Database.AddEntityTable("GJI_DECISION_CON_SUBJ",
               new RefColumn("CONTRAGENT_ID", ColumnProperty.Null, "GJI_DECISION_CON_SUBJ_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID"),
               new Column("EXTERNAL_ID", DbType.String, 36),
               new Column("PHYSICAL_PERSON", DbType.String, 300),
               new Column("PHYSICAL_PERSON_POSITION", DbType.String, 300),
               new Column("PERSON_INSPECTION", DbType.Int32, 4, ColumnProperty.NotNull, 20),
               new RefColumn("DECISION_ID", ColumnProperty.NotNull, "GJI_DECISION_CON_SUBJ_D", "GJI_DICISION", "ID"));

            Database.AddEntityTable(
             "GJI_DECISION_EXPERT",
               new RefColumn("DECISION_ID", "GJI_DECISION_EXPERT_D", "GJI_DICISION", "ID"),
               new RefColumn("EXPERT_ID", "GJI_DECISION_EXPERT_EX", "GJI_DICT_EXPERT", "ID"),
               new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddEntityTable(
             "GJI_DECISION_PROVDOC",
               new RefColumn("DECISION_ID", "GJI_DECISION_PROVDOC_D", "GJI_DICISION", "ID"),
               new RefColumn("PROVIDED_DOC_ID", "GJI_DECISION_PROVDOC_PROVDOC", "GJI_DICT_PROVIDEDDOCUMENT", "ID"),
               new Column("DESCRIPTION", DbType.String, 3000));

            Database.AddEntityTable(
            "GJI_DECISION_VERIFSUBJ",
              new RefColumn("DECISION_ID", "GJI_DECISION_VERIFSUBJ_D", "GJI_DICISION", "ID"),
              new RefColumn("SURVEY_SUBJECT_ID", "GJI_DECISION_VERIFSUBJSUBJECT_ID", "GJI_DICT_SURVEY_SUBJ", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DECISION_VERIFSUBJ");
            Database.RemoveTable("GJI_DECISION_PROVDOC");
            Database.RemoveTable("GJI_DECISION_EXPERT");
            Database.RemoveTable("GJI_DECISION_CON_SUBJ");
            Database.RemoveTable("GJI_DECISION_CON_MEASURE");
            Database.RemoveTable("GJI_DECISION_ANNEX");
            Database.RemoveTable("GJI_DECISION_ADMREG");
            Database.RemoveTable("GJI_DICISION");
        }
    }
}