namespace Bars.Gkh.Migrations.Version_2014030601
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014030600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Database.AddRefColumn("GKH_OBJ_D_PROTOCOL", new RefColumn("STATE_ID", "GKH_OBJ_D_PROT_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            //Database.RemoveRefColumn("GKH_OBJ_D_PROTOCOL", "STATE_ID");
        }
    }
}