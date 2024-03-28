namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015032501
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015032500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "penalty_change");
        }

        public override void Down()
        {
        }

        #endregion
    }
}
