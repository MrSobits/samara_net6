namespace Bars.GkhGji.Migrations._2020.Version_2020120701
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020120701")]
    [MigrationDependsOn(typeof(Version_2020120700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_MKD_LIC_STATEMENT_FILE", new RefColumn("SIGNED_FILE_ID", "GJI_MKD_LIC_STATEMENT_FILE_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_MKD_LIC_STATEMENT_FILE", new RefColumn("SIGNATURE_FILE_ID", "GJI_MKD_LIC_STATEMENT_FILE_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_MKD_LIC_STATEMENT_FILE", new RefColumn("CERTIFICATE_FILE_ID", "GJI_MKD_LIC_STATEMENT_FILE_CERTIFICATE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_MKD_LIC_STATEMENT_FILE", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_MKD_LIC_STATEMENT_FILE", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_MKD_LIC_STATEMENT_FILE", "CERTIFICATE_FILE_ID");
        }
    }
}