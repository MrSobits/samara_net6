namespace Bars.GkhGji.Migrations.Version_2014022001
{
    using System.Data;
    using Bars.Gkh;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014022000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // в более поздних версиях миграций нактываются въюхи
            //ViewManager.Drop(Database, "GkhGji");

            Database.RenameColumn("GJI_BASESTAT_APPCIT", "GJI_INSP_STAT_ID", "INSPECTION_ID");

            Database.RemoveConstraint("GJI_BASESTAT_APPCIT", "FK_GJI_BASEST_APPCIT_ST");
            Database.AddForeignKey("FK_GJI_BASEST_APPCIT_INSP", "GJI_BASESTAT_APPCIT", "INSPECTION_ID", "GJI_INSPECTION", "ID");

            // в более поздних версиях миграций нактываются въюхи
            //ViewManager.Create(Database, "GkhGji");
        }

        public override void Down()
        {
            
            Database.RenameColumn("GJI_BASESTAT_APPCIT", "INSPECTION_ID", "GJI_INSP_STAT_ID");

            Database.RemoveConstraint("GJI_BASESTAT_APPCIT", "FK_GJI_BASEST_APPCIT_INSP");
            Database.AddForeignKey("FK_GJI_BASEST_APPCIT_ST", "GJI_BASESTAT_APPCIT", "GJI_INSP_STAT_ID", "GJI_INSPECTION_STATEMENT", "ID");
        }
    }
}