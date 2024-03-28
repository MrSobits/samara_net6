namespace Bars.GkhCr.Migration.Version_2015082400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015082400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015062900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // из-за переименования функции gjiGetDocParentPlaceByViolStage (oracle требует <= 30 символов)
            ViewManager.Drop(Database, "GkhCr");
            ViewManager.Create(Database, "GkhCr");
        }

        public override void Down()
        {
        }
    }
}