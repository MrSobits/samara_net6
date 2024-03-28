namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023033000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2023033000")]
    [MigrationDependsOn(typeof(Version_2023030700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("ACCESSGUID", DbType.String, 100));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("FIZ_INN", DbType.String, 30));
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION", new RefColumn("ERKNM_REASON", "GJI_APPCADMON_ERKNM_REASON", "GJI_DICT_DECISION_REASON_ERKNM", "ID"));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("FIZ_ADDR", DbType.String, 250));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("TYPE_RISK", DbType.Int16, ColumnProperty.NotNull, 0));
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION", new RefColumn("SURVEY_SUBJECT_ID", "GJI_APPCADMON_SURV_SUBJ_ID", "GJI_DICT_SURVEY_SUBJ", "ID"));
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION", new RefColumn("SIGNED_FILE_ID", "GJI_APPCIT_ADMONITION_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION", new RefColumn("SIGNATURE_FILE_ID", "GJI_APPCIT_ADMONITION_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION", new RefColumn("CERTIFICATE_FILE_ID", "GJI_APPCIT_ADMONITION_CERTIFICATE", "B4_FILE_INFO", "ID"));
         

            this.Database.AddEntityTable(
            "GJI_CH_APPCIT_ADMONITION_LTEXT",
            new RefColumn("ADMONITION_ID", ColumnProperty.NotNull, "GJI_APPCIT_ADMONITION_LTEXT_ADMON", "GJI_CH_APPCIT_ADMONITION", "ID"),
            new Column("MEASURES", DbType.Binary),
            new Column("VIOLATION", DbType.Binary));


        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_CH_APPCIT_ADMONITION_LTEXT");

            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "CERTIFICATE_FILE_ID");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "SIGNED_FILE_ID");

            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "SURVEY_SUBJECT_ID");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "TYPE_RISK");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "FIZ_ADDR");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "ERKNM_REASON");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "FIZ_INN");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "ACCESSGUID");
        }
    }
}