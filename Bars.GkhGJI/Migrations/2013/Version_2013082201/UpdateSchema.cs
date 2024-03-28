namespace Bars.GkhGji.Migrations.Version_2013082201
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013082201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013082200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_REMINDER", new RefColumn("INSPECTOR_ID", "GJI_REMINDER_INSR", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {

            Database.RemoveColumn("GJI_REMINDER", "INSPECTOR_ID");
        }
    }
}