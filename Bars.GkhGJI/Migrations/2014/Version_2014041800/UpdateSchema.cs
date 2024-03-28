namespace Bars.GkhGji.Migrations.Version_2014041800
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014041800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014041700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_APPEAL_CITIZENS", new RefColumn("APPROVALCONTRAGENT_ID", "GJI_APPROVALCONTRAGENT_ID", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "APPROVALCONTRAGENT_ID");
        }
    }
}