namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122601
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_SPECIAL_ACCOUNT", new RefColumn("DECISION_ID", "OVRHL_SPECIAL_ACCOUNT_DEC", "OVRHL_PR_DEC_SPEC_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SPECIAL_ACCOUNT", "DECISION_ID");
        }
    }
}