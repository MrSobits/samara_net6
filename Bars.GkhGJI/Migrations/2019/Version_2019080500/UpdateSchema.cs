namespace Bars.GkhGji.Migrations._2019.Version_2019080500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019080500")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019072300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_ACTCHECK_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_ACTCHECK_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTCHECK_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_ACTCHECK_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTCHECK_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_ACTCHECK_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_ACTSURVEY_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_ACTSURVEY_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTSURVEY_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_ACTSURVEY_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_ACTSURVEY_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_ACTSURVEY_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_DISPOSAL_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_DISPOSAL_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_DISPOSAL_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_DISPOSAL_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_DISPOSAL_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_DISPOSAL_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PRESCRIPTION_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PRESCRIPTION_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PRESCRIPTION_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PRESCRIPTION_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PRESCRIPTION_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PRESCRIPTION_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PRESENTATION_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PRESENTATION_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PRESENTATION_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PRESENTATION_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PRESENTATION_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PRESENTATION_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PROTOCOL_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PROTOCOL_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PROTOCOL_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PROTOCOL_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PROTOCOLMHC_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PROTOCOLMHC_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOLMHC_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PROTOCOLMHC_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOLMHC_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PROTOCOLMHC_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PROT_MVD_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PROT_MVD_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROT_MVD_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PROT_MVD_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROT_MVD_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PROT_MVD_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_PROTOCOLRSO_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_PROTOCOLRSO_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOLRSO_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_PROTOCOLRSO_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_PROTOCOLRSO_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_PROTOCOLRSO_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_RESOLPROS_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_RESOLPROS_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_RESOLPROS_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_RESOLPROS_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_RESOLPROS_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_RESOLPROS_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_RESOLUTION_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_RESOLUTION_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_RESOLUTION_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_RESOLUTION_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_RESOLUTION_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_RESOLUTION_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

            Database.AddRefColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", new RefColumn("SIGNED_FILE_ID", "GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX_SIGNED_FILE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", new RefColumn("SIGNATURE_FILE_ID", "GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX_SIGNATURE", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", new RefColumn("CERTIFICATE_FILE_ID", "GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX_CERTIFICATE", "B4_FILE_INFO", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_ACTSURVEY_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_ACTSURVEY_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_ACTSURVEY_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PRESENTATION_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PRESENTATION_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PRESENTATION_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PROTOCOLMHC_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOLMHC_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOLMHC_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PROT_MVD_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PROT_MVD_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PROT_MVD_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_PROTOCOLRSO_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOLRSO_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_PROTOCOLRSO_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_RESOLPROS_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_RESOLPROS_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_RESOLPROS_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "CERTIFICATE_FILE_ID");

            Database.RemoveColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", "SIGNED_FILE_ID");
            Database.RemoveColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", "SIGNATURE_FILE_ID");
            Database.RemoveColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", "CERTIFICATE_FILE_ID");
        }
    }
}