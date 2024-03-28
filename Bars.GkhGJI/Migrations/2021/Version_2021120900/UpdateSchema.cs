namespace Bars.GkhGji.Migrations._2021.Version_2021120900
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021120900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021120601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_APPCIT_REQUEST", new RefColumn("SENDER_INSPECTOR_ID", ColumnProperty.None, "GJI_APPCIT_REQUEST_SENDER_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_REQUEST", "SENDER_INSPECTOR_ID");
        }
    }
}