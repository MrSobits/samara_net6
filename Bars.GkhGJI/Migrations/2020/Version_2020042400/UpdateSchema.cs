namespace Bars.GkhGji.Migrations._2020.Version_2020042400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020042400")]
    [MigrationDependsOn(typeof(Version_2020042000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_APPCIT_REQUEST", new RefColumn("SIGNED_FILE_ID", "GJI_APPCIT_REQUEST_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_APPCIT_REQUEST", new RefColumn("SIGNATURE_FILE_ID", "GJI_APPCIT_REQUEST_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_APPCIT_REQUEST", new RefColumn("CERTIFICATE_FILE_ID", "GJI_APPCIT_REQUEST_CERTIFICATE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_REQUEST", "CERTIFICATE_FILE_ID");         
            Database.RemoveColumn("GJI_APPCIT_REQUEST", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_APPCIT_REQUEST", "SIGNED_FILE_ID");
        }
    }
}