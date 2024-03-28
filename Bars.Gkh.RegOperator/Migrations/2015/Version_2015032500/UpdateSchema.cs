namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015032500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015032002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_CHARGE", new Column("penalty_change", DbType.Decimal, ColumnProperty.NotNull, "0"));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("penalty_change", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "penalty_change");
            Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "penalty_change");
        }

        #endregion
    }
}
