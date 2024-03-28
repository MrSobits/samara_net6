namespace Bars.GkhGji.Migration.Version_2014061500
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014061001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_BUISNES_NOTIF", "STATE_ID");
            Database.AddRefColumn("GJI_BUISNES_NOTIF", new RefColumn("STATE_ID", "GJI_BUIS_NOTIF_ST", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            
        }
    }
}