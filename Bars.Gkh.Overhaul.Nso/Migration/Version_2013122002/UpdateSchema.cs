namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122002
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_ACC_BANK_STATEMENT", new RefColumn("STATE_ID", "OVRHL_BANK_STAT_ST", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_ACC_BANK_STATEMENT", "STATE_ID");
        }
    }
}