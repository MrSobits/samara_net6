namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014093001
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014093001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014093000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            }

            Database.RenameTable("REGOP_SALDO_CHANGE", "REGOP_SUMMARY_SALDO_CHANGE");

            Database.AddRefColumn("REGOP_SUMMARY_SALDO_CHANGE", new RefColumn("SUMMARY_ID", "REGOP_SUMMARY_SALDO_CHANGE_SM", "REGOP_PERS_ACC_PERIOD_SUMM", "ID"));

            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("BALANCE_CHANGE", DbType.Decimal, ColumnProperty.NotNull, 0m));

            Database.AddColumn("REGOP_RO_CHARGE_ACC_CHARGE", new Column("BALANCE_CHANGE", DbType.Decimal, ColumnProperty.NotNull, 0m));

            UpdateSaldoChangesValidDate();

            UpdateSaldoChangesNotValidDate();

            UpdateSaldoChangeSetGuid();

            Database.RemoveColumn("regop_summary_saldo_change", "account_id");
        }

        public override void Down()
        {
            Database.AddRefColumn("regop_summary_saldo_change", new RefColumn("ACCOUNT_ID", "regop_summ_saldo_change_acc", "regop_base_pers_acc", "id"));

            Database.RemoveColumn("REGOP_RO_CHARGE_ACC_CHARGE", "BALANCE_CHANGE");

            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "BALANCE_CHANGE");

            Database.RemoveColumn("REGOP_SALDO_CHANGE", "SUMMARY_ID");

            Database.RenameTable("REGOP_SUMMARY_SALDO_CHANGE", "REGOP_SALDO_CHANGE");
        }

        private void UpdateSaldoChangesValidDate()
        {
            //обновление записей, дата которых принадлежит какому-либо из PeriodSummary
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"
update regop_summary_saldo_change t
set summary_id = tt.id, oper_date = t.object_create_date::timestamp::date
from  (
	select
		aps.id,
		aps.account_id,
		p.cstart,
		p.cend
	from REGOP_PERS_ACC_PERIOD_SUMM aps
		join regop_period p on p.id = aps.period_id
) tt
where tt.account_id = t.account_id
	and tt.cstart <= t.object_create_date
	and (tt.cend is null or tt.cend >= t.object_create_date)
	and (t.summary_id is null or t.oper_date is null)");
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"
update regop_summary_saldo_change t
set summary_id = tt.id, oper_date = to_date(to_char(t.object_create_date, 'dd.mm.yyyy'), 'dd.mm.yyyy')
from  (
	select 
        aps.id, 
        aps.account_id,
        p.cstart,
        p.cend
	from REGOP_PERS_ACC_PERIOD_SUMM aps
		join regop_period p on p.id = aps.period_id
) tt
where tt.account_id = t.account_id
	and tt.cstart <= t.object_create_date
	and (tt.cend is null or tt.cend >= t.object_create_date)
	and (t.summary_id is null or t.oper_date is null)");
            }
        }

        private void UpdateSaldoChangesNotValidDate()
        {
            //обновление записей, которые находятся до даты первого PeriodSummary лицевого счета (мало ли)
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"
update regop_summary_saldo_change t
set summary_id = ttt.summary_id, oper_date=ttt.cstart::date + 1
from (
	select
		sp.summary_id,
		sp.cstart
	from regop_summary_saldo_change ssc
		join (
			select 
				k.account_id,
				asp.id as summary_id,
				p.cstart
			from (
				select 
					pa.id as account_id,
					(select aps1.id from REGOP_PERS_ACC_PERIOD_SUMM aps1 where aps1.account_id=pa.id order by id limit 1) as summary_id
				from regop_pers_acc pa
			) k
				join REGOP_PERS_ACC_PERIOD_SUMM asp on asp.id = k.summary_id
				join regop_period p on p.id = asp.period_id
		) sp on sp.account_id = ssc.account_id
) ttt
where t.summary_id is null");
            }

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"
update regop_summary_saldo_change t
set summary_id = ttt.summary_id, oper_date=ttt.cstart
from (
	select
		sp.summary_id,
		sp.cstart
	from regop_summary_saldo_change ssc
		join (
			select 
				k.account_id,
				asp.id as summary_id,
				p.cstart
			from (
				select 
					pa.id as account_id,
					(select x.id from(
                        select 
                            aps1.id,
                            aps1.account_id
                        from REGOP_PERS_ACC_PERIOD_SUMM aps1
                        order by id) x
                    where x.account_id=pa.id and rownum<=1) as summary_id
				from regop_pers_acc pa
			) k
				join REGOP_PERS_ACC_PERIOD_SUMM asp on asp.id = k.summary_id
				join regop_period p on p.id = asp.period_id
		) sp on sp.account_id = ssc.account_id
) ttt
where t.summary_id is null;");
            }
        }

        private void UpdateSaldoChangeSetGuid()
        {
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("update regop_summary_saldo_change set GUID = uuid_generate_v4()::text where GUID is null or GUID = ''");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("update regop_summary_saldo_change set GUID = RAWTOHEX(sys_guid()) where GUID is null");
            }
        }
    }
}
