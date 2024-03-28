namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021301
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameColumn("REGOP_PERS_ACC_PERIOD_SUMM", "PAYMENT", "TARIFF_PAYMENT");
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("PENALTY_PAYMENT", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "PENALTY_PAYMENT");
            Database.RenameColumn("REGOP_PERS_ACC_PERIOD_SUMM", "TARIFF_PAYMENT", "PAYMENT");
        }
    }
}
