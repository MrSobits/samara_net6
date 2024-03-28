namespace Bars.Gkh.Migrations.Version_2013081300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013081300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013080200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_REALITY_OBJECT", new RefColumn("STATE_ID", "GKH_REAL_OBJ_ST", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "STATE_ID");
        }
    }
}