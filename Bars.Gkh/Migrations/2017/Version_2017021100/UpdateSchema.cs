namespace Bars.Gkh.Migrations._2017.Version_2017021100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция Gkh.2017021100
    /// </summary>
    [Migration("2017021100")]
    [MigrationDependsOn(typeof(Version_2017020100.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017012500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017012501.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_PERSON_CERTIFICATE", new Column("RECIEVE_DATE", DbType.DateTime));

            this.Database.AddEntityTable(
                "GKH_CERTIFICATE_DOCUMENT",
                new Column("DOC_TYPE", DbType.Int32),
                new Column("DOC_NUMBER", DbType.String),
                new Column("STMNT_NUMBER", DbType.String),
                new Column("ISSUE_DATE", DbType.DateTime),
                new Column("NOTE", DbType.String, 500),
                new FileColumn("FILE_ID", "CERTIFICATE_DOCUMENT_FILE_ID"),
                new RefColumn("CERTIFICATE_ID", ColumnProperty.NotNull, "CERTIFICATE_DOCUMENT_CERT_ID", "GKH_PERSON_CERTIFICATE", "ID"));

            // смигрируем старые данные
            this.Database.ExecuteNonQuery(@"insert into GKH_CERTIFICATE_DOCUMENT (certificate_id, doc_type, doc_number, issue_date, file_id, object_version, object_create_date, object_edit_date)
                select id, 10, duplicate_number, duplicate_issued_date, duplicate_file_id, 0, now()::date, now()::date from GKH_PERSON_CERTIFICATE where has_duplicate");

            this.Database.AddEntityTable(
                "GKH_TECHNICAL_MISTAKE",
                new Column("STMNT_NUMBER", DbType.String),
                new Column("FIX_INFO", DbType.String),
                new Column("FIX_DATE", DbType.DateTime),
                new Column("ISSUE_DATE", DbType.DateTime),
                new Column("DECISION_NUMBER", DbType.String),
                new Column("DECISION_DATE", DbType.DateTime),
                new RefColumn("CERTIFICATE_ID", ColumnProperty.NotNull, "TECHNICAL_MISTAKE_CERT_ID", "GKH_PERSON_CERTIFICATE", "ID"),
                new RefColumn("PERSON_ID", ColumnProperty.Null, "TECHNICAL_MISTAKE_PERSON_ID", "GKH_PERSON", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "RECIEVE_DATE");
            this.Database.RemoveTable("GKH_CERTIFICATE_DOCUMENT");
            this.Database.RemoveTable("GKH_TECHNICAL_MISTAKE");
        }
    }
}