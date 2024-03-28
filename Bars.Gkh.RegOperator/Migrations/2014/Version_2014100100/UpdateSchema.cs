namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014100100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014093003.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // создаем колонки для PenltyChange
            Database.AddRefColumn("REGOP_PENALTY_CHANGE", new RefColumn("SUMMARY_ID", "REGOP_PENALTY_CHANGE_APS", "REGOP_PERS_ACC_PERIOD_SUMM", "ID"));
            Database.AddColumn("REGOP_PENALTY_CHANGE", new Column("C_GUID", DbType.String, 40, ColumnProperty.Null));

            UpdateSaldoChangesValidDate();

            // скарипт проставления гуида для тех записей, которые 
            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("update REGOP_PENALTY_CHANGE set C_GUID = uuid_generate_v4()::text where C_GUID is null or C_GUID = ''");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("update REGOP_PENALTY_CHANGE set C_GUID = RAWTOHEX(sys_guid()) where C_GUID is null");
            }
        }

        private void UpdateSaldoChangesValidDate()
        {
            //обновление записей, дата которых принадлежит какому-либо из PeriodSummary
            Database
                .ExecuteNonQuery(@"
update REGOP_PENALTY_CHANGE t
set SUMMARY_ID = tt.id
from (
	select 
		aps.id,
		aps.account_id,
		p.cstart,
		p.cend
 from REGOP_PERS_ACC_PERIOD_SUMM aps
  join regop_period p on p.id = aps.period_id
) tt
where t.SUMMARY_ID is null
	and tt.account_id = t.account_id
	and tt.cstart <= t.object_create_date
	and (tt.cend is null or tt.cend >= t.object_create_date)");
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PENALTY_CHANGE", "SUMMARY_ID");
            Database.RemoveColumn("REGOP_PENALTY_CHANGE", "C_GUID");
        }
    }
}
