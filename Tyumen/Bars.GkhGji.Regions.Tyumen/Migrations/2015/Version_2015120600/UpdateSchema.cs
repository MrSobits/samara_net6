namespace Bars.GkhGji.Regions.Tyumen.Migrations.Version_2015120600
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120600")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_APPLICANT_NOTIFY", new RefColumn("STATE_ID", "GKH_APPLICANT_NOTIFY_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_APPLICANT_NOTIFY", "STATE_ID");
        }
    }
}