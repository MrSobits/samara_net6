namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014042300
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014042200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameColumn("OVRHL_REG_OP_CALC_ACC", "CA_BANK_ID", "CA_BANK_CR_ORG_ID");
        }

        public override void Down()
        {
            Database.RenameColumn("OVRHL_REG_OP_CALC_ACC", "CA_BANK_CR_ORG_ID", "CA_BANK_ID");
        }
    }
}
