namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014112900
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014112801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("RF_TRANSFER_CTR", "CALC_ACCOUNT_ID");
            Database.AddRefColumn("RF_TRANSFER_CTR", new RefColumn("CALC_ACCOUNT_ID", "RF_TR_CTR_CALC_ACC", "REGOP_CALC_ACC_REGOP", "ID"));
        }

        public override void Down()
        {
        }
    }
}
