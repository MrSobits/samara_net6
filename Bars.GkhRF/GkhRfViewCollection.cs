namespace Bars.GkhRf
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;

    using Gkh;

    public class GkhRfViewCollection : BaseGkhViewCollection
    {
        public override int Number
        {
            get
            {
                return 3;
            }
        }

        #region Вьюхи
        #region Create

        /// <summary>
        /// вьюха договоров рег фонда
        /// view_rf_contract
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRfContract(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE VIEW view_rf_contract AS
                SELECT 
                    contr.id,               --0
                    contr.document_num,     --1
                    contr.document_date,    --2
                    c.id as contragent_id,  --3
                    c.name AS man_org_name, --4
                    mu.id as mu_id,         --5
                    mu.name AS municipality_name,   --6
                    (
                        select
                            count(1)
                        from rf_contract_object cro 
                        where cro.type_condition = 10 and cro.contract_rf_id=contr.id
                    ) as contract_objs_count,   --7
                    (
                        select
                            sum(ro.area_mkd)
                        from rf_contract_object cro 
                         left join gkh_reality_object ro on ro.id = cro.reality_obj_id
                         where cro.type_condition = 10 and cro.contract_rf_id=contr.id
                    ) as sum_area_mkd,          --8
                    (
                        select
                            sum(ro.area_living_owned)
                        from rf_contract_object cro 
                         left join gkh_reality_object ro on ro.id = cro.reality_obj_id
                         where cro.type_condition = 10 and cro.contract_rf_id=contr.id
                    ) as sum_area_owned,          --8
                    man_org.id AS man_org_id    --9
                FROM rf_contract contr
                    JOIN gkh_managing_organization man_org ON contr.manag_org_id = man_org.id
                    JOIN gkh_contragent c ON man_org.contragent_id = c.id
                    LEFT JOIN gkh_dict_municipality mu ON c.municipality_id = mu.id";
        }


        /// <summary>
        /// Вьюха оплат кап ремонта
        /// view_rf_payment
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRfPayment(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_rf_payment AS
SELECT 
    pay.id,             --0
    ro.id as ro_id,     --1
    ro.address,         --2
    mu.id as mu_id,     --3
    mu.name as mu_name, --4

    (
        select 
            coalesce(sum(coalesce(pay_it.charge_population, 0)) + sum(coalesce(pay_it.recalculation, 0)), 0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 10
    ) as cr_charge, --5
    
    (
        select 
            coalesce(sum(coalesce(pay_it.paid_population, 0)),0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 10
    ) as cr_paid,   --6
    
    (
        select 
            coalesce(sum(coalesce(pay_it.charge_population, 0)) + sum(coalesce(pay_it.recalculation, 0)), 0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 20
    ) as hire_rf_charge,    --7
    
    (
        select 
            coalesce(sum(coalesce(pay_it.paid_population, 0)),0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 20
    ) as hire_rf_paid,  --8
    
    (
        select 
            coalesce(sum(coalesce(pay_it.charge_population, 0)) + sum(coalesce(pay_it.recalculation, 0)), 0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 30
    ) as cr185_charge,  --9
    
    (
        select 
            coalesce(sum(coalesce(pay_it.paid_population, 0)),0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 30
    ) as cr185_paid,    --10
    
    (
        select 
            coalesce(sum(coalesce(pay_it.charge_population, 0)) + sum(coalesce(pay_it.recalculation, 0)), 0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 80
    ) as bld_repair_charge, --11
    
    (
        select 
            coalesce(sum(coalesce(pay_it.paid_population, 0)),0)
        from rf_payment_item pay_it
        where pay_it.payment_id = pay.id and pay_it.type_payment = 80
    ) as bld_repair_paid    --12
    
FROM rf_payment pay
    LEFT JOIN gkh_reality_object ro ON ro.id = pay.reality_obj_id
    left join gkh_dict_municipality mu on mu.id = ro.municipality_id";
        }

        /// <summary>
        /// Вьюха заявой перечислений
        /// view_rf_request_transfer
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRfRequestTransfer(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_rf_request_transfer AS 
SELECT 
    req_tr.id,      --0
    st.id AS state_id,     --1
    req_tr.document_num,    --2
    req_tr.date_from,       --3
    req_tr.type_program_request,    --4
    ctragent.id as contragent_id,   --5
    ctragent.name as manorg_name,  --6
    municipality.id as municipality_id, --7 
    municipality.name as municipality_name, --8 
    (
        select
            count(*)
        from rf_transfer_funds tr_fnd
        where tr_fnd.request_transfer_rf_id = req_tr.id
    ) as transfer_funds_count,  --9
    (
        select
            coalesce(sum(tr_fnd.sum), 0)
        from rf_transfer_funds tr_fnd
        where tr_fnd.request_transfer_rf_id = req_tr.id
    ) as transfer_funds_sum  --10
FROM rf_request_transfer req_tr
    LEFT JOIN gkh_managing_organization man_org ON req_tr.managing_organization_id = man_org.id
    LEFT JOIN gkh_contragent ctragent ON man_org.contragent_id = ctragent.id
    LEFT JOIN gkh_dict_municipality municipality ON ctragent.municipality_id = municipality.id
    LEFT JOIN b4_state st ON req_tr.state_id = st.id";
        }

        /// <summary>
        /// вьюха перечислений
        /// view_rf_transfer
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRfTransfer(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_rf_transfer AS 
SELECT
    transfer.id,    --0
    contr.document_num,     --1
    contr.document_date,    --2
    ctragent.id as contragent_id,   --3
    ctragent.name AS man_org_name,  --4
    municipality.id as mu_id,   --5
    municipality.name AS municipality_name,     --6
    (
	    select 
		    count(cro.id) 
	    from rf_contract_object cro 
	    where cro.contract_rf_id = contr.id and cro.type_condition=10
    ) AS contract_objs_count   --7
FROM rf_transfer transfer
    LEFT JOIN rf_contract contr ON transfer.contract_rf_id = contr.id
    LEFT JOIN gkh_managing_organization man_org ON contr.manag_org_id = man_org.id
    LEFT JOIN gkh_contragent ctragent ON man_org.contragent_id = ctragent.id
    LEFT JOIN gkh_dict_municipality municipality ON ctragent.municipality_id = municipality.id";
        }

        /// <summary>
        /// вьюха перечислений
        /// view_rf_transfer
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewRfTransferRecord(DbmsKind dbmsKind)
        {
            return @"
CREATE OR REPLACE VIEW view_rf_transfer_record AS 
SELECT 
	tr.ID,
	tr.TRANSFER_RF_ID,
	tr.DESCRIPTION,
	tr.DATE_FROM,
	tr.DOCUMENT_NAME,
	tr.DOCUMENT_NUM,
	tr.STATE_ID,
	tr.TRANSFER_DATE,
	tr.FILE_ID,
	(select count(distinct tro.id) from RF_TRANSFER_REC_OBJ tro where tro.TRANSFER_RF_RECORD_ID = tr.id) as COUNT,
	(select sum(tro.SUM) from RF_TRANSFER_REC_OBJ tro where tro.TRANSFER_RF_RECORD_ID = tr.id) as SUM
	
from RF_TRANSFER_RECORD tr";
        }

        #endregion Create
        #region Delete

        /// <summary>
        /// view_rf_contract
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewRfContract(DbmsKind dbmsKind)
        {
            var viewName = "view_rf_contract";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        /// <summary>
        /// view_rf_payment
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewRfPayment(DbmsKind dbmsKind)
        {
            var viewName = "view_rf_payment";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        /// <summary>
        /// view_rf_request_transfer
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewRfRequestTransfer(DbmsKind dbmsKind)
        {
            var viewName = "view_rf_request_transfer";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        /// <summary>
        /// view_rf_transfer
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewRfTransfer(DbmsKind dbmsKind)
        {
            var viewName = "view_rf_transfer";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        /// <summary>
        /// view_rf_transfer
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteViewRfTransferRecord(DbmsKind dbmsKind)
        {
            var viewName = "view_rf_transfer_record";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }

        #endregion Delete
        #endregion Вьюхи
    }
}