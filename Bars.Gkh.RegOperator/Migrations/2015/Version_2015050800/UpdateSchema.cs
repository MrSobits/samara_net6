namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015050800
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015050701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("RAA_WALLET_ID", ColumnProperty.Null, "REGOP_PACC_W_R", "REGOP_WALLET", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "RAA_WALLET_ID");
        }
    }
}
