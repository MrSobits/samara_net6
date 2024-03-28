namespace Bars.Gkh.Overhaul.Tat
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    public class GkhOvrhlViewCollection : BaseGkhViewCollection
    {
        public override int Number => 1;

        /// <summary>
        /// Вьюха объектов кап ремонта
        /// view_cr_object
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewCrObject(DbmsKind dbmsKind)
        {
            return @"
DROP VIEW IF EXISTS view_cr_object;
CREATE OR REPLACE VIEW view_cr_object AS
SELECT 
    cr.id,
    cr.program_num,
    cr.program_id,
    cr.before_delete_program_id,
    prog.name as program_name,
    beforedelprog.name as before_del_program_name,
    mu.id AS municipality_id,
    mu.name AS municipality_name,
    stl.id AS settlement_id,
    stl.name AS settlement_name,
    ro.id AS reality_object_id,
    ro.address,
    cr.date_accept_gji,
    cr.allow_reneg,
    smr.id as smr_id,
    st.id AS state_id,
    smr_st.id as smr_state_id,
    per.name as period_name,
    coalesce((SELECT b.METHOD_FORM_FUND AS METHOD_FORM_FUND
FROM OVRHL_PROP_OWN_DECISION_BASE b
INNER JOIN OVRHL_PROP_OWN_PROTOCOLS pr ON b.PROP_OWNER_PROTOCOL_ID=pr.Id
WHERE b.REALITY_OBJECT_ID=cr.reality_object_id
  AND (pr.TYPE_PROTOCOL=0
       OR pr.TYPE_PROTOCOL=20)
  AND b.DECISION_TYPE=10
ORDER BY pr.DOCUMENT_DATE DESC
LIMIT 1
  ),0) as method_form_fund
FROM cr_object cr
    inner join gkh_reality_object ro ON ro.id=cr.reality_object_id 
    inner join gkh_dict_municipality mu ON ro.municipality_id =  mu.id
    left join gkh_dict_municipality stl ON ro.stl_municipality_id =  stl.id
    left join cr_dict_program prog ON cr.program_id = prog.id
    left join gkh_dict_period per on per.id = prog.PERIOD_ID
    left join cr_dict_program beforedelprog ON cr.before_delete_program_id = beforedelprog.id
    left join b4_state st on st.id = cr.state_id
    left join CR_OBJ_MONITORING_CMP smr on smr.object_id = cr.id
    left join b4_state smr_st on smr_st.id = smr.state_id";
        }

        private string DeleteViewCrObject(DbmsKind dbmsKind)
        {
            var viewName = "view_cr_object";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropViewOracleQuery(viewName);
            }

            return this.DropViewPostgreQuery(viewName);
        }
    }
}