namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014081900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014081900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014080900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddIndex("IDX_REGOP_CALC_TRACE_GUID", false, "REGOP_CALC_PARAM_TRACE", "CALC_GUID");
            Database.AddIndex("IDX_REGOP_P_CHARGE_GUID", false, "REGOP_PERS_ACC_CHARGE", "GUID");
        }

        public override void Down()
        {
            Database.RemoveIndex("IDX_REGOP_P_CHARGE_GUID", "REGOP_PERS_ACC_CHARGE");
            Database.RemoveIndex("IDX_REGOP_CALC_TRACE_GUID", "REGOP_CALC_PARAM_TRACE");
        }
    }
}
