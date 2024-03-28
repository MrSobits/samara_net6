namespace Bars.GkhGji.Migrations.Version_2014021900
{
    using System.Data;
    using Bars.Gkh;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014021800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // в более поздних версиях миграций нактываются въюхи
            //ViewManager.Drop(Database, "GkhGji", "DeleteViewDisposal");
            //ViewManager.Create(Database, "GkhGji", "CreateViewDisposal");
        }

        public override void Down()
        {
            
        }
    }
}