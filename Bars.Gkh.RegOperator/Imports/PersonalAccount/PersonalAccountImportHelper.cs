using System.Collections.Generic;

namespace Bars.Gkh.RegOperator.Imports
{
    using NHibernate;

    public static class PersonalAccountImportHelper
    {
		public static IEnumerable<string> GetSqlCommandsNeedToExecute(ISession session)
		{
			const string sqlGetNeedToExecute = @"select * from
                (SELECT 
                UNNEST(ARRAY[
	                'CREATE TABLE IF NOT EXISTS public.'||q.tablename||'_period_'||per.id||'(LIKE '||q.tablename||' INCLUDING ALL, CONSTRAINT '||q.tablename||'_constraint CHECK ('||(case when tablename = 'regop_recalc_history' then 'recalc_period_id' else 'period_id' end)||' = '||per.id||'))INHERITS ('||q.tablename||');',
	
	                (case when q.tablename = 'regop_transfer' then 'CREATE TRIGGER delete_transfer_tr BEFORE DELETE ON public.'||q.tablename||'_period_'||per.id||' FOR EACH ROW EXECUTE PROCEDURE public.delete_from_transfer();' else null end),
	                (case when q.tablename = 'regop_transfer' then 'ALTER TABLE regop_transfer_period_'||per.id||' ADD CONSTRAINT fk_regop_transfer_period_'||per.id||'_op FOREIGN KEY (op_id) REFERENCES public.regop_money_operation (id);' else null end),
	                (case when q.tablename = 'regop_transfer' then 'ALTER TABLE regop_transfer_period_'||per.id||' ADD CONSTRAINT fk_regop_transfer_period_'||per.id||'_owner_id FOREIGN KEY (owner_id) REFERENCES public.regop_pers_acc (id);' else null end),

	                (case when q.tablename = 'regop_charge_transfer' then 'CREATE TRIGGER delete_transfer_tr BEFORE DELETE ON public.'||q.tablename||'_period_'||per.id||' FOR EACH ROW EXECUTE PROCEDURE public.delete_from_transfer();' else null end),
	                (case when q.tablename = 'regop_charge_transfer' then 'ALTER TABLE regop_charge_transfer_period_'||per.id||' ADD CONSTRAINT fk_regop_ch_transfer_period_'||per.id||'_op FOREIGN KEY (op_id) REFERENCES public.regop_money_operation (id);' else null end),
	                (case when q.tablename = 'regop_charge_transfer' then 'ALTER TABLE regop_charge_transfer_period_'||per.id||' ADD CONSTRAINT fk_regop_ch_transfer_period_'||per.id||'_owner_id FOREIGN KEY (owner_id) REFERENCES public.regop_pers_acc (id);' else null end),

			        (case when q.tablename = 'regop_reality_transfer' then 'CREATE TRIGGER delete_transfer_tr BEFORE DELETE ON public.'||q.tablename||'_period_'||per.id||' FOR EACH ROW EXECUTE PROCEDURE public.delete_from_transfer();' else null end),
	                (case when q.tablename = 'regop_reality_transfer' then 'ALTER TABLE regop_reality_transfer_period_'||per.id||' ADD CONSTRAINT fk_regop_ro_transfer_period_'||per.id||'_op FOREIGN KEY (op_id) REFERENCES public.regop_money_operation (id);' else null end),
	                (case when q.tablename = 'regop_reality_transfer' then 'ALTER TABLE regop_reality_transfer_period_'||per.id||' ADD CONSTRAINT fk_regop_ro_transfer_period_'||per.id||'_owner_id FOREIGN KEY (owner_id) REFERENCES public.regop_ro_payment_account (id);' else null end),
	
	                (case WHEN q.tablename = 'regop_pers_acc_charge' THEN 'ALTER TABLE public.regop_pers_acc_charge_period_'||per.id||' ADD CONSTRAINT fk_regop_pers_acc_charge_'||per.id||'_period FOREIGN KEY (period_id) REFERENCES public.regop_period (id);'  else null end),
	                (case WHEN q.tablename = 'regop_pers_acc_charge' THEN 'ALTER TABLE public.regop_pers_acc_charge_period_'||per.id||' ADD CONSTRAINT fk_regop_pers_acc_charge_'||per.id||'_pa FOREIGN KEY (pers_acc_id) REFERENCES public.regop_pers_acc (id);'  else null end),
	
	                (case WHEN q.tablename = 'regop_pers_acc_change' THEN 'ALTER TABLE public.regop_pers_acc_change_period_'||per.id||' ADD CONSTRAINT fk_regop_pers_acc_change_'||per.id||'_period FOREIGN KEY (doc_id) REFERENCES public.b4_file_info (id);'  else null end),
	                (case WHEN q.tablename = 'regop_pers_acc_change' THEN 'ALTER TABLE public.regop_pers_acc_change_period_'||per.id||' ADD CONSTRAINT fk_regop_pers_acc_change_'||per.id||'_pa FOREIGN KEY (acc_id) REFERENCES public.regop_pers_acc (id);'  else null end),
	
	                (case WHEN q.tablename = 'regop_pers_acc_period_summ' THEN 'ALTER TABLE public.regop_pers_acc_period_summ_period_'||per.id||' ADD CONSTRAINT fk_regop_pers_acc_period_summ_'||per.id||'_pa FOREIGN KEY (account_id) REFERENCES public.regop_pers_acc (id);'  else null end),
	                (case WHEN q.tablename = 'regop_pers_acc_period_summ' THEN 'ALTER TABLE public.regop_pers_acc_period_summ_period_'||per.id||' ADD CONSTRAINT fk_regop_pers_acc_period_summ_'||per.id||'_period FOREIGN KEY (period_id) REFERENCES public.regop_period (id);'  else null end),

	                (case WHEN q.tablename = 'regop_recalc_history' THEN 'ALTER TABLE public.regop_recalc_history_period_'||per.id||' ADD CONSTRAINT fk_regop_recalc_history_'||per.id||'_pa FOREIGN KEY (account_id) REFERENCES public.regop_pers_acc (id);'  else null end),
	                (case WHEN q.tablename = 'regop_recalc_history' THEN 'ALTER TABLE public.regop_recalc_history_period_'||per.id||' ADD CONSTRAINT fk_regop_recalc_history_'||per.id||'_per FOREIGN KEY (calc_period_id) REFERENCES public.regop_period (id);'  else null end),
	                (case WHEN q.tablename = 'regop_recalc_history' THEN 'ALTER TABLE public.regop_recalc_history_period_'||per.id||' ADD CONSTRAINT fk_regop_recalc_history_'||per.id||'_recalc_per FOREIGN KEY (recalc_period_id) REFERENCES public.regop_period (id);'  else null end)

                ]) n
                FROM
                (select UNNEST(ARRAY['regop_pers_acc_charge', 'regop_pers_acc_change', 'regop_calc_param_trace', 'regop_transfer', 'regop_charge_transfer', 'regop_reality_transfer', 'regop_pers_acc_period_summ', 'regop_recalc_history']) tablename) q
                join regop_period per on 1=1
                where not exists (select null from pg_tables where schemaname = 'public' and tablename = q.tablename||'_period_'||per.id)) q
                where q.n is not null";

			return session.CreateSQLQuery(sqlGetNeedToExecute).List<string>();
		}
	}
}
