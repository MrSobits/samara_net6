namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122603
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122603")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122602.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в модуль overhaul
            // Database.AddColumn("OVRHL_ACCOUNT", new Column("CREDIT_LIMIT", DbType.Decimal));
        }

        public override void Down()
        {
            // Database.RemoveColumn("OVRHL_ACCOUNT", "CREDIT_LIMIT");
        }
    }
}