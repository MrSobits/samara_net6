namespace Bars.GkhGji.Migrations.Version_2013100300
{
    using Bars.Gkh;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013100300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013100100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // в более поздних версиях миграций нактываются въюхи
            //ViewManager.Drop(Database, "GkhGji");
            //ViewManager.Create(Database, "GkhGji");
        }

        public override void Down()
        {
            
        }
    }
}