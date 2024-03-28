namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016101500
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция RegOperator 2016101500
    /// </summary>
    [Migration("2016101500")]
    [MigrationDependsOn(typeof(Version_2016101100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        #region Sql
        private const string SqlSaldoChange = @"SELECT DISTINCT ON (change.id)
             change.oper_date,
             change.reason,
             coalesce(ch.operator, 'anonymous') as user,
             change.doc_id,
             ps.period_id,
             change.guid,
             ps.account_id,
             change.current_val,
             change.new_val
            into changes_temp
            FROM REGOP_SUMMARY_SALDO_CHANGE change
             JOIN regop_pers_acc_period_summ ps ON ps.id = change.summary_id
             LEFT JOIN regop_pers_acc_change ch ON
                                                 ch.change_type = 8 AND
                                                 ch.acc_id = ps.account_id AND
                                                 replace(ch.new_value, ',', '.') :: NUMERIC = change.new_val AND
                                                 replace(ch.old_value, ',', '.') :: NUMERIC = change.current_val
            WHERE NOT exists(SELECT NULL -- проверка, что уже не добавляли этих записей
                             FROM REGOP_CHARGE_OPERATION_BASE cb
                             WHERE cb.guid = change.guid);

            with result as (insert into REGOP_CHARGE_OPERATION_BASE
            (object_version, object_create_date, object_edit_date, op_date, fact_op_date, reason, user_name, charge_source, guid, doc_id)
             select 0, now(), now(), t.oper_date,t.oper_date, t.reason, t.user, 50, t.guid, t.doc_id  from changes_temp t
            RETURNING id, guid)

            insert into REGOP_SALDO_CHANGE_SOURCE(id, period_id)
                select r.id, t.period_id
                from result r
                join changes_temp t on t.guid = r.guid;

                    insert into REGOP_SALDO_CHANGE_DETAIL
                    (object_version, object_create_date, object_edit_date, change_type, old_value, new_value, charge_op_id, acc_id)
                select 0, now(), now(), 0, t.current_val, t.new_val, base.id, t.account_id
                from REGOP_CHARGE_OPERATION_BASE base
                join changes_temp t on t.guid = base.guid;

            DROP table changes_temp;";
        #endregion

        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddIndex("REGOP_PA_GROUP_RELATION_PA_ID", false, "REGOP_PA_GROUP_RELATION", "PA_ID");
            this.Database.AddIndex("REGOP_PA_GROUP_RELATION_GROUP_ID", false, "REGOP_PA_GROUP_RELATION", "GROUP_ID");

            // запрос, заполняющий новые таблицы по изменению сальдо из старых данных
            this.Database.ExecuteNonQuery(UpdateSchema.SqlSaldoChange);
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
        }
    }
}
