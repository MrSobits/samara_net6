namespace Bars.GkhGji.Migrations._2020.Version_2020042800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020042800")]
    [MigrationDependsOn(typeof(Version_2020042400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_APPCIT_REQUEST_ANSWER",
                new Column("DOCUMENT_NAME", DbType.String),
                new Column("DOCUMENT_NUM", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("REQUEST_ID", "FK_REQUESRANSWER_REQUEST", "GJI_APPCIT_REQUEST", "ID"),
                new RefColumn("FILE_ID", "FK_EDS_REQUEST_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNED_FILE_ID", "FK_EDS_REQUEST_SIG_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("SIGNATURE_FILE_ID", "FK_EDS_REQUEST_SIGNATURE", "B4_FILE_INFO", "ID"),
                new RefColumn("CERTIFICATE_FILE_ID", "FK_EDS_REQUEST_CERT", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPCIT_REQUEST_ANSWER");
        }
    }
}