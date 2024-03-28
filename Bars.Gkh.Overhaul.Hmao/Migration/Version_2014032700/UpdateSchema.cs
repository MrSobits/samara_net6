namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014032700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014030600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в Overhaul
            // Database.AddColumn("OVRHL_CREDIT_ORG", new Column("OKTMO", DbType.String));
        }

        public override void Down()
        {
            // Database.RemoveColumn("OVRHL_CREDIT_ORG", "OKTMO");
        }
    }
}