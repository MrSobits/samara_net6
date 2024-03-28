namespace Bars.GkhGji.Migrations.Version_2014040800
{
    using System.Data;
    using Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014040700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("BECAME_LEGAL", DbType.Boolean,false));
            // в более поздних версиях миграций нактываются въюхи
            //ViewManager.Drop(Database, "GkhGji", "DeleteViewResolution");
            //ViewManager.Create(Database, "GkhGji", "CreateViewResolution");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "BECAME_LEGAL");
        }
    }
}