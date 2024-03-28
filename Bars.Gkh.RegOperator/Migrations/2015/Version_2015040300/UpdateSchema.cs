namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015040300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015040300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015032700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("BASE_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("DEC_TARIFF_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("PENALTY_DEBT", DbType.Decimal, ColumnProperty.NotNull, 0m));

            FillColumns();
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "BASE_TARIFF_DEBT");
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "DEC_TARIFF_DEBT");
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "PENALTY_DEBT");
        }

        private void FillColumns()
        {
            Database.ExecuteNonQuery(@"
update regop_pers_acc_period_summ ps

set base_tariff_debt = coalesce((
	select sum(ps1.charge_base_tariff + ps1.balance_change + ps1.recalc - ps1.tariff_payment)
	from regop_pers_acc_period_summ ps1
		join regop_period p1 on p1.id = ps1.period_id
	where ps1.account_id = ps.account_id 
		and p1.cstart<p.cstart), 0),

dec_tariff_debt = coalesce((
	select sum(ps1.charge_tariff - ps1.charge_base_tariff + ps1.recalc_decision - ps1.tariff_desicion_payment)
	from regop_pers_acc_period_summ ps1
		join regop_period p1 on p1.id = ps1.period_id
	where ps1.account_id = ps.account_id 
		and p1.cstart<p.cstart), 0),

penalty_debt = coalesce((
	select sum(ps1.penalty + ps1.recalc_penalty - ps1.penalty_payment)
	from regop_pers_acc_period_summ ps1
		join regop_period p1 on p1.id = ps1.period_id
	where ps1.account_id = ps.account_id 
		and p1.cstart<p.cstart), 0)
from regop_period p
where ps.period_id = p.id");
        }
    }
}
