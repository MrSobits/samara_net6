namespace Bars.Gkh.Migrations.Version_2014122400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2014122400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014122300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddRefColumn("GKH_PERSON_REQUEST_EXAM", new RefColumn("PERSONAL_DATA_CONSENT_FILE_ID", "GKH_PERS_REQ_EX_PDCF", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
			Database.RemoveColumn("GKH_PERSON_REQUEST_EXAM", "PERSONAL_DATA_CONSENT_FILE_ID");
        }
    }
}