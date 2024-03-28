namespace Bars.GkhGji.Migrations._2021.Version_2021120601
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021120601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_ACTCHECK_DEFINITION", new RefColumn("FILE_ID", "ACTCHECK_DEFINITION_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTCHECK_DEFINITION", new RefColumn("SIGNED_FILE_ID", "ACTCHECK_DEFINITION_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTCHECK_DEFINITION", new RefColumn("SIGNATURE_FILE_ID", "ACTCHECK_DEFINITION_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTCHECK_DEFINITION", new RefColumn("CERTIFICATE_FILE_ID", "ACTCHECK_DEFINITION_CERTIFICATE", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK_DEFINITION", "FILE_ID");
            Database.RemoveColumn("GJI_ACTCHECK_DEFINITION", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_ACTCHECK_DEFINITION", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_ACTCHECK_DEFINITION", "CERTIFICATE_FILE_ID");
        }
    }
}