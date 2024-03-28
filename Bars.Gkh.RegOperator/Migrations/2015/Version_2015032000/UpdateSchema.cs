namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015032000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015031300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM",
                new Column("RECALC_PENALTY", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.AddColumn("REGOP_PERS_ACC_CHARGE",
                new Column("RECALC_PENALTY", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.AddColumn("REGOP_UNACCEPT_CHARGE",
                new Column("RECALC_PENALTY", DbType.Decimal, ColumnProperty.NotNull, 0m));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "RECALC_PENALTY");
            Database.RemoveColumn("REGOP_PERS_ACC_CHARGE", "RECALC_PENALTY");
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "RECALC_PENALTY");
        }
    }
}
