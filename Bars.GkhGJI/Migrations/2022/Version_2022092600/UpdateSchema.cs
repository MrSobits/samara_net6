namespace Bars.GkhGji.Migrations._2022.Version_2022092600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022092600")]
    [MigrationDependsOn(typeof(Version_2022082500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_SPECIAL_ACCOUNT_REPORT", new RefColumn("SIGNED_FILE_ID", "GJI_SPECIAL_ACCOUNT_REPORT_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_SPECIAL_ACCOUNT_REPORT", new RefColumn("SIGNATURE_FILE_ID", "GJI_SPECIAL_ACCOUNT_REPORT_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_SPECIAL_ACCOUNT_REPORT", new RefColumn("CERTIFICATE_FILE_ID", "GJI_SPECIAL_ACCOUNT_REPORT_CERTIFICATE", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_SPECIAL_ACCOUNT_REPORT", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_SPECIAL_ACCOUNT_REPORT", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_SPECIAL_ACCOUNT_REPORT", "CERTIFICATE_FILE_ID");
        }
    }
}