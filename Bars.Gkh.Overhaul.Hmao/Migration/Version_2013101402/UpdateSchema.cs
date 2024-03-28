namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101402
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101402")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_ACCRUALS_ACCOUNT", "OBJECT_VERSION");
            Database.RemoveColumn("OVRHL_ACCRUALS_ACCOUNT", "OBJECT_CREATE_DATE");
            Database.RemoveColumn("OVRHL_ACCRUALS_ACCOUNT", "OBJECT_EDIT_DATE");

            Database.RemoveColumn("OVRHL_REAL_ACCOUNT", "OBJECT_VERSION");
            Database.RemoveColumn("OVRHL_REAL_ACCOUNT", "OBJECT_CREATE_DATE");
            Database.RemoveColumn("OVRHL_REAL_ACCOUNT", "OBJECT_EDIT_DATE");

            Database.RemoveColumn("OVRHL_SPECIAL_ACCOUNT", "OBJECT_VERSION");
            Database.RemoveColumn("OVRHL_SPECIAL_ACCOUNT", "OBJECT_CREATE_DATE");
            Database.RemoveColumn("OVRHL_SPECIAL_ACCOUNT", "OBJECT_EDIT_DATE");
        }

        public override void Down()
        {
#warning почему пусто?
        }
    }
}