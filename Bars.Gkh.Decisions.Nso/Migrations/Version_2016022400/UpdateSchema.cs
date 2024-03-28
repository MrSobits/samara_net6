namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2016022400
{
    /// <summary>
    /// Миграция по обновлению поля GKH_REALITY_OBJECT.acc_form_variant
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016022400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2015050400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            // 0 - Спец счет регопа
            // 1 - Регоп
            // 2 - Спец счет
            // 3 - Не задано
            if (this.CheckDecisionTables())
            {
                this.Database.ExecuteNonQuery(""
                + "update GKH_REALITY_OBJECT ro"
                + "   set acc_form_variant = coalesce("
                + "        (select (case when ss = 2 then 2"
                + "                      when (cr_f = 1 or p1 = 1) then 1"
                + "                      else 0"
                + "                 end)"
                + "           from (select t.*,"
                + "                        (select decision_value"
                + "                           from DEC_ULTIMATE_DECISION dud, DEC_CR_FUND dcf"
                + "                          where dcf.id = dud.id"
                + "                            and dud.protocol_id = t.p_id"
                + "                          order by dcf.id limit 1"
                + "                        ) cr_f,"
                + "                        (select decision_type"
                + "                           from DEC_ULTIMATE_DECISION dud, DEC_ACCOUNT_OWNER dao"
                + "                          where dao.id= dud.id"
                + "                            and dud.protocol_id = t.p_id"
                + "                          order by dao.id limit 1"
                + "                        ) ss"
                + "                   from (select d.id p_id, protocol_date, date_start, d.state_id, 0 p1, ro_id"
                + "                           from GKH_OBJ_D_PROTOCOL d"
                + "                          where ro_id = ro.id"
                + "                          union all"
                + "                         select -1 p_id, protocol_date, date_start, g.state_id, 1 p1, ro_id"
                + "                           from DEC_GOV_DECISION g"
                + "                          where ro_id = ro.id"
                + "                        ) t"
                + "                   join (select b4_state.id as st_id"
                + "                           from B4_STATE"
                + "                          where type_id in ('gkh_real_obj_gov_dec', 'gkh_real_obj_dec_protocol')"
                + "                            and final_state = 't'"
                + "                        ) t2 on t2.st_id = t.state_id"
                + "                  where coalesce(date_start, protocol_date) <= now()"
                + "                ) t3"
                + "          order by coalesce(date_start, protocol_date) desc"
                + "          limit 1), 3)");
            }
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
        }

        private bool CheckDecisionTables() => 
            this.Database.TableExists("DEC_ULTIMATE_DECISION") 
                && this.Database.TableExists("GKH_OBJ_D_PROTOCOL") 
                && this.Database.TableExists("DEC_GOV_DECISION");
    }
}
