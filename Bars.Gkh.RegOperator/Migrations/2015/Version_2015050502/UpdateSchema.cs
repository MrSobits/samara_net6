namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015050502
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050502")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015050501.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            FillColumns();
        }

        public override void Down()
        {
        }

        private void FillColumns()
        {
            //проставляется значение задолженностей на начало периода
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
