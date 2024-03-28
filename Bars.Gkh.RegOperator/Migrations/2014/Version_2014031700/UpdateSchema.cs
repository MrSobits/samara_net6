namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014031700
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014031400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("ind_persacc_charge_chd", false, "REGOP_PERS_ACC_CHARGE", "CHARGE_DATE");
        }

        public override void Down()
        {
            Database.RemoveIndex("ind_persacc_charge_chd", "REGOP_PERS_ACC_CHARGE");
        }
    }
}
