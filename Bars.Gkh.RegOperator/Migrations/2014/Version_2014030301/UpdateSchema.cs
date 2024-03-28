namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030301
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030301")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("CHARGE_BASE_TARIFF", DbType.Decimal, ColumnProperty.NotNull, "0"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "CHARGE_BASE_TARIFF");
        }
    }
}
