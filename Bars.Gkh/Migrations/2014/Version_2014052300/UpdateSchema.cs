namespace Bars.Gkh.Migrations.Version_2014052300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014041700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_OBJ_CURENT_REPAIR",
                new RefColumn("BUILDER_ID", "GKH_CURR_REP_BUILDER", "GKH_BUILDER", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_CURENT_REPAIR", "BUILDER_ID");
        }
    }
}