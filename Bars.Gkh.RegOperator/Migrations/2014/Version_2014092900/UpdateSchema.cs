namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092900
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_RO_PAYMENT_ACCOUNT", "PWA_WALLET_ID");
        }

        public override void Down()
        {
            Database.AddRefColumn("REGOP_RO_PAYMENT_ACCOUNT", new RefColumn("PWA_WALLET_ID", ColumnProperty.Null, "REGOP_ROPACC_W_AF", "REGOP_WALLET", "ID"));
        }
    }
}
