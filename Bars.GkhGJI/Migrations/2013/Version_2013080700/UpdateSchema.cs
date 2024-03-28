namespace Bars.GkhGji.Migrations.Version_2013080700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013080700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013071800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_INSPECTION", new RefColumn("STATE_ID", "GJI_INSPECTION_ST", "B4_STATE", "ID"));
            
            // в более поздних версиях миграций нактываются въюхи
            //ViewManager.Drop(Database, "GkhGji");
            //ViewManager.Create(Database, "GkhGji");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION", "STATE_ID");
        }
    }
}