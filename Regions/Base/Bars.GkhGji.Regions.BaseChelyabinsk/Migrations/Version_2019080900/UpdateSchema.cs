namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2019080900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019080900")]
    [MigrationDependsOn(typeof(Version_20190206000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_NSO_ACTREMOVAL_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_NSO_ACTREMOVAL_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_NSO_ACTREMOVAL_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_NSO_ACTREMOVAL_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_NSO_ACTREMOVAL_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_NSO_ACTREMOVAL_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PROTOCOL197_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PROTOCOL197_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL197_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PROTOCOL197_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL197_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PROTOCOL197_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_ACTREMOVAL_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_NSO_ACTREMOVAL_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_NSO_ACTREMOVAL_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "CERTIFICATE_FILE_ID");
        }
    }
}