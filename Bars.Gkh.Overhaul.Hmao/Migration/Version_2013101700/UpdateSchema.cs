namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101700
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013101602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "CREDIT_ORG_ID");

            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "ACCOUNT_ID");
            Database.AddRefColumn("OVRHL_PROP_OWN_DECISION",
                new RefColumn("ACCOUNT_ID", "OVRHL_PROPOWNDEC_ACCOUNT", "OVRHL_SPECIAL_ACCOUNT", "ID"));
        }

        public override void Down()
        {
            Database.AddRefColumn("OVRHL_PROP_OWN_DECISION",
                new RefColumn("CREDIT_ORG_ID", "OVRHL_DECISION_CREDIT_ORG", "OVRHL_CREDIT_ORG", "ID"));

            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "ACCOUNT_ID");
            Database.AddRefColumn("OVRHL_PROP_OWN_DECISION",
                new RefColumn("ACCOUNT_ID", "OVRHL_PROPOWNDEC_ACCOUNT", "OVRHL_ACCOUNT", "ID"));
        }
    }
}