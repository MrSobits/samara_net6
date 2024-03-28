namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("OVRHL_SM_RECORD_VERSION");
            Database.RemoveTable("OVRHL_SUBSIDY_MU_REC");
            Database.RemoveTable("OVRHL_SUBSIDY_MU");
        }

        public override void Down()
        {
        }
    }
}