namespace Bars.GkhGji.Region.Tatarstan
{
	using Bars.Gkh;
	using Bars.B4.Modules.Ecm7.Framework;
	using Bars.Gkh.Enums;
	using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Bars.B4.Utils;
    using System;
    using System.Linq;

    public class GkhGjiTatarstanViewCollection : BaseGkhViewCollection
    {
        public override int Number => 1;

        #region Create
        #region Вьюхи
        
        /// <summary>
        /// Вьюха актов устранения нарушений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActRemoval(DbmsKind dbmsKind)
        {
           return $@"
                CREATE OR REPLACE VIEW view_gji_act_removal AS 
                 SELECT 
                    doc.id, 
                    doc.state_id, 
                    doc.id AS document_id, 
                    doc.document_date, 
                    doc.document_number, 
                    doc.document_num, 
                    docparent.id AS parent_id, 
                    docparent.type_document AS parent_type,
                    parent.ctr_mu_name,
                    parent.ctr_mu_id,
                    parent.contragent_name, 
                    (('№' || docparent.document_number) || ' ') || to_char(docparent.document_date, 'DD.MM.YYYY') AS parent_name, 
                    gjiGetDocCountChildDoc(doc.id) AS count_children, 
                    gjiGetDocCntRealObjByViolStage(doc.id) AS count_robject, 
                    gjiGetDocumentInspectors(doc.id) AS inspector_names, 
                    gjiGetDocRobjectByViolStage(doc.id) AS ro_ids, 
                    gjiGetDocParRoAdrByViolStage(doc.id) AS ro_addresses,
                    gjiGetDocMuByViolStage(doc.id) AS mu_names,
                    ''::text AS mo_names,
                    ''::text AS place_names, 
                    gjiGetDocMuIdByViolStage(doc.id) AS mu_id, 
                    insp.id AS inspection_id, 
                    insp.type_base, 
                    act.type_removal, 
                    doc.type_document AS type_doc
                FROM gji_document doc
                    JOIN gji_actremoval act ON act.id = doc.id
                    JOIN gji_document_children ch ON ch.children_id = doc.id
                    JOIN gji_document docparent ON docparent.type_document <> {(int)TypeDocumentGji.ActCheck} AND docparent.id = ch.parent_id
                    JOIN (
                        SELECT 
                            presc.id,
                            mu.name as ctr_mu_name,
                            c.municipality_id as ctr_mu_id,
                            c.name AS contragent_name
                        FROM gji_prescription presc
                            LEFT JOIN gkh_contragent c ON c.id = presc.contragent_id
                            LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                        UNION 
                        SELECT 
                            prot.id,
                            mu.name as ctr_mu_name,
                            c.municipality_id as ctr_mu_id,
                            c.name AS contragent_name
                        FROM gji_protocol prot
                            LEFT JOIN gkh_contragent c ON c.id = prot.contragent_id
                            LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                        ) parent ON parent.id = docparent.id
                    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id";
        }
        
        /// <summary>
        /// Вьюха актов проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewActcheck(DbmsKind dbmsKind)
        {
            return $@"
				CREATE OR REPLACE VIEW view_gji_actcheck AS 
				SELECT doc.id,
				    doc.state_id,
				    doc.id AS document_id,
				    doc.document_date,
				    doc.document_number,
				    doc.document_num,
				    STRING_AGG(inspector.fio, ', '::text) AS inspector_names,
				    COUNT(DISTINCT ro.id) AS count_ro,
				    COALESCE(NULLIF(MIN(aro.have_violation), {(int)YesNoNotSet.NotSet}), {(int)YesNo.No}) AS has_violation,
				    COUNT(DISTINCT child_doc.id) AS count_exec_doc,
				    STRING_AGG(DISTINCT ro.id::text, '/'::text) AS ro_ids,
				    STRING_AGG(DISTINCT ro.address, ', '::text) AS ro_addresses,
				    STRING_AGG(DISTINCT mun.name, ', '::text) AS mu_names,
				    MIN(mun.id) AS mu_id,
				    insp.id AS inspection_id,
				    insp.type_base,
				    mu.name AS ctr_mu_name,
				    c.municipality_id AS ctr_mu_id,
				    c.name AS contragent_name,
				    doc.type_document AS type_doc
				   FROM gji_document doc
				   JOIN gji_inspection insp ON insp.id = doc.inspection_id
				   LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
				   LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
				   LEFT JOIN
				   (
				    gji_document_inspector insp_link
				      	 JOIN gkh_dict_inspector inspector ON inspector.id = insp_link.inspector_id 
				   ) ON insp_link.document_id = doc.id
				   LEFT JOIN 
				   (
						gji_actcheck_robject aro
						JOIN gkh_reality_object ro ON aro.reality_object_id = ro.id
						JOIN gkh_dict_municipality mun ON mun.id = ro.municipality_id
				   ) ON aro.actcheck_id = doc.id
				   LEFT JOIN
				   (
						gji_document_children child
						JOIN gji_document child_doc ON child_doc.id = child.children_id
				    	     AND child_doc.type_document != {(int)TypeDocumentGji.ActRemoval}
				   ) ON child.parent_id = doc.id
				   WHERE doc.type_document = {(int)TypeDocumentGji.ActCheck}
				   GROUP BY doc.id, insp.id, mu.id, c.id";
        }
        
        /// <summary>
        /// Вьюха распоряжений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewDisposal(DbmsKind dbmsKind)
        {
            return $@"
				CREATE OR REPLACE VIEW public.view_gji_disposal
				 AS
				 SELECT doc.id,
				    doc.state_id,
				    doc.id AS document_id,
				    doc.document_num,
				    doc.document_number,
				    doc.document_date,
				    insp.id AS inspection_id,
				    insp.type_base,
				    mu.name AS ctr_mu_name,
				    c.municipality_id AS ctr_mu_id,
				    c.name AS contragent,
				    disp.date_start,
				    disp.date_end,
				    kind_check.name AS kind_check_name,
				    gjigetdisposaltypesurveys(doc.id::bigint) AS tsurveys_name,
				    gjigetdispcntrobj(doc.id::bigint, doc.inspection_id) AS ro_count,
				    gjigetdispactcheckexist(doc.id::bigint) AS act_check_exist,
				    gjigetdocumentinspectors(doc.id::bigint) AS inspector_names,
				    gjigetdisprealityobj(doc.id::bigint, doc.inspection_id) AS ro_ids,
				    gjigetdisprealityobjaddress(doc.id::bigint, doc.inspection_id) AS ro_addresses,
				    gjigetinsprobjectmuname(doc.inspection_id) AS mu_names,
				    ''::text AS mo_names,
				    ''::text AS place_names,
				    gjigetinsprobjectmuid(doc.inspection_id) AS mu_id,
				    doc.type_document AS type_doc,
				    disp.type_disposal,
				    disp.type_agrprosecutor,
				    insp.control_type,
				    ml.id AS license,
				    child_doc.id IS NOT NULL AS has_act_survey
				   FROM gji_document doc
				     JOIN gji_disposal disp ON disp.id = doc.id
				     JOIN gji_inspection insp ON doc.inspection_id = insp.id
				     JOIN b4_state state ON doc.state_id = state.id
				     LEFT JOIN gji_inspection_lic_app ila ON doc.inspection_id = ila.id
				     LEFT JOIN gkh_manorg_lic_request mlr ON ila.man_org_lic_id = mlr.id
				     LEFT JOIN gkh_contragent c ON
				        CASE
				            WHEN insp.type_base = {(int)TypeBase.LicenseApplicants} THEN mlr.contragent_id
				            ELSE insp.contragent_id
				        END = c.id
				     LEFT JOIN gkh_manorg_license ml ON ml.contragent_id = c.id
				     LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
				     LEFT JOIN gji_dict_kind_check kind_check ON kind_check.id = disp.kind_check_id
				     LEFT JOIN (gji_document_children child
				     JOIN gji_document child_doc ON child_doc.id = child.children_id AND child_doc.type_document = {(int)TypeDocumentGji.ActSurvey}) ON child.parent_id = doc.id
					 WHERE disp.type_disposal <> {(int)TypeDisposalGji.NullInspection} AND doc.type_document <> {(int)TypeDocumentGji.TaskDisposal}";
        }
        
        /// <summary>
        /// Вьюха предписаний
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewPrescription(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE VIEW view_gji_prescription AS 
                  SELECT 
                    doc.id, 
                    doc.state_id, 
                    doc.id AS document_id, 
                    doc.document_date, 
                    doc.document_num, 
                    doc.document_number, 
                    gjiGetDocCntRealObjByViolStage(pr.id) AS count_ro, 
                    gjiGetDocCountViolByViolStage(pr.id) AS count_viol, 
                    gjiGetDocumentInspectors(pr.id) AS inspector_names, 
                    gjiGetDocRobjectByViolStage(pr.id) AS ro_ids,
                    gjiGetDocParRoAdrByViolStage(pr.id) AS ro_addresses,
                    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
                    ''::text AS mo_names,
                    ''::text AS place_names,
                    gjiGetDocMuIdByViolStage(pr.id) AS mu_id, 
                    insp.id AS inspection_id, 
                    insp.type_base,
                    mu.name as ctr_mu_name,
                    c.municipality_id as ctr_mu_id,
                    c.name AS contragent_name, 
                    exec.name AS type_exec_name, 
                    doc.type_document AS type_doc, 
                    v.date_removal,
                    (select count(distinct disp.id) 
                     from GJI_DOCUMENT_CHILDREN prnt_chldr
                     join GJI_DOCUMENT disp on disp.TYPE_DOCUMENT = 10 and disp.id = prnt_chldr.CHILDREN_ID
                     where prnt_chldr.PARENT_ID = doc.ID
                    )  disp_id
                FROM gji_document doc
                    JOIN gji_prescription pr ON pr.id = doc.id
                    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
                    LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
                    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                    LEFT JOIN gji_dict_executant exec ON exec.id = pr.executant_id
                    LEFT JOIN ( SELECT 
                            max(viol.DATE_PLAN_REMOVAL) AS date_removal, 
                            viol.document_id
                        FROM gji_inspection_viol_stage viol
                        GROUP BY viol.document_id) v ON v.document_id = pr.id";
        }

        /// <summary>
        /// Вьюха протоколов
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewProtocol(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE VIEW view_gji_protocol AS 
                 SELECT 
                    doc.id, 
                    doc.state_id, 
                    doc.id AS document_id, 
                    doc.document_date, 
                    doc.document_number, 
                    doc.document_num, 
                    gjiGetDocumentInspectors(pr.id) AS inspector_names, 
                    gjiGetDocCntRealObjByViolStage(pr.id) AS count_ro, 
                    gjiGetDocCountViolByViolStage(pr.id) AS count_viol, 
                    gjiGetDocRobjectByViolStage(pr.id) AS ro_ids, 
                    gjiGetDocParRoAdrByViolStage(pr.id) As ro_addresses,
                    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
                    ''::text AS mo_names,
                    ''::text AS place_names,
                    gjiGetDocMuIdByViolStage(pr.id) AS mu_id, 
                    mu.name as ctr_mu_name,
                    c.municipality_id as ctr_mu_id,
                    c.name AS contragent_name, 
                    exec.name AS type_exec_name, 
                    insp.id AS inspection_id, 
                    insp.type_base, 
                    doc.type_document AS type_doc
                FROM gji_document doc
                    JOIN gji_protocol pr ON pr.id = doc.id
                    LEFT JOIN gkh_contragent c ON c.id = pr.contragent_id
                    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                    LEFT JOIN gji_dict_executant exec ON exec.id = pr.executant_id
                    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id";
        }
        
        /// <summary>
        /// Вьюха постановлений
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewResolution(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE VIEW view_gji_resolution AS 
                 SELECT 
                    doc.id, 
                    doc.state_id, 
                    doc.id AS document_id, 
                    doc.document_date, 
                    doc.document_number, 
                    doc.document_num, 
                    payfine.sum_pays, 
                    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobject(doc.id) ELSE gjiGetDocParentRobjByViolStage(doc.id) end) AS ro_ids, 
                    gjiGetDocParRoAdrByViolStage(doc.id) as protocol_ro_addresses,
                    gjiGetDocParentMuByViolStage(doc.id) AS mu_names, 
                    ''::text AS mo_names,
                    ''::text AS place_names,
                    gjiGetDocParentMuIdByViolStage(doc.id) AS mu_id, 
                    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobjectAddress(doc.id) ELSE gjiGetDocParRoAdrByViolStage(doc.id) end) AS ro_address, 
                    inspector.fio AS official_name, 
                    res.official_id, 
                    res.penalty_amount, 
                    insp.id AS inspection_id, 
                    insp.type_base, 
                    exec.name AS type_exec_name, 
                    s.name AS sanction_name,
                    mu.name as ctr_mu_name,
                    c.municipality_id as ctr_mu_id,
                    c.name AS contragent_name, 
                    doc.type_document AS type_doc,
                    res.DELIVERY_DATE,
                    res.PAIDED,
                    res.BECAME_LEGAL,
                    res.TYPE_INITIATIVE_ORG
                FROM gji_document doc
                    JOIN gji_resolution res ON res.id = doc.id
                    LEFT JOIN ( 
                        SELECT 
                            gji_resolution_payfine.resolution_id, 
                            sum(gji_resolution_payfine.amount) AS sum_pays
                        FROM gji_resolution_payfine
                        GROUP BY gji_resolution_payfine.resolution_id
                        ) payfine ON payfine.resolution_id = doc.id
                    LEFT JOIN gkh_dict_inspector inspector ON inspector.id = res.official_id
                    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
                    LEFT JOIN gkh_contragent c ON c.id = res.contragent_id
                    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                    LEFT JOIN gji_dict_executant exec ON exec.id = res.executant_id
                    LEFT JOIN gji_dict_sanction s ON s.id = res.sanction_id";
        }

        private string CreateViewTatDisposal(DbmsKind dbmsKind)
        {
	        return $@"
				CREATE OR REPLACE VIEW view_gji_tat_disposal AS
                SELECT doc.id,
                   doc.state_id,
                   doc.id AS document_id,
                   doc.document_num,
                   doc.document_number,
                   doc.document_date,
                   insp.id AS inspection_id,
                   insp.type_base,
                   mu.name AS ctr_mu_name,
                   c.municipality_id AS ctr_mu_id,
                   c.name AS contragent,
                   disp.date_start,
                   disp.date_end,
                   kind_check.name AS kind_check_name,
                   STRING_AGG(DISTINCT tsurvey.name::text, ', '::text) AS tsurveys_name,
                       CASE
                           WHEN parent_link.* IS NULL THEN COUNT(DISTINCT ro.id)
                           ELSE COUNT(DISTINCT viol_ro.id)
                       END AS ro_count,
                   COUNT(act_check.id) > 0 AS act_check_exist,
                   STRING_AGG(DISTINCT inspector.fio::text, ', '::text) AS inspector_names,
                       CASE
                           WHEN parent_link.* IS NULL THEN STRING_AGG(DISTINCT ro.id::text, '/'::text)
                           ELSE STRING_AGG(DISTINCT viol_ro.id::text, '/'::text)
                       END AS ro_ids,
                       CASE
                           WHEN parent_link.* IS NULL THEN STRING_AGG(DISTINCT ro.address::text, ', '::text)
                           ELSE STRING_AGG(DISTINCT viol_ro.address::text, ', '::text)
                       END AS ro_addresses,
                   STRING_AGG(DISTINCT romu.name::text, ', '::text) AS mu_names,
                   min(romu.id) AS mu_id,
                   doc.type_document AS type_doc,
                   disp.type_disposal,
                   disp.type_agrprosecutor,
                   insp.control_type,
                   ml.id AS license,
                   COUNT(survey.id) > 0 AS has_act_survey,
                   tat_disp.registration_number_erp,
                   gdct.name AS control_type_name
                FROM gji_document doc
                JOIN gji_disposal disp ON disp.id = doc.id
                JOIN gji_tat_disposal tat_disp ON tat_disp.id = doc.id
                JOIN gji_inspection insp ON doc.inspection_id = insp.id
                LEFT JOIN 
                (
                    gji_disposal_typesurvey disp_tsurvey
                    JOIN gji_dict_typesurvey tsurvey ON tsurvey.id = disp_tsurvey.typesurvey_id
                ) ON disp_tsurvey.disposal_id = doc.id
                LEFT JOIN gji_dict_control_types gdct ON gdct.id = tat_disp.control_type_id
                LEFT JOIN 
                (
                    gji_inspection_lic_app ila
                    JOIN gkh_manorg_lic_request mlr ON ila.man_org_lic_id = mlr.id
                ) ON doc.inspection_id = ila.id
                LEFT JOIN gkh_contragent c ON 
                      CASE
                         WHEN insp.type_base = {(int)TypeBase.LicenseApplicants} THEN mlr.contragent_id
                         ELSE insp.contragent_id
                      END = c.id
                LEFT JOIN gkh_manorg_license ml ON ml.contragent_id = c.id
                     AND (ml.date_termination IS NULL OR ml.date_termination < CURRENT_DATE) 
                LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                LEFT JOIN gji_dict_kind_check kind_check ON kind_check.id = disp.kind_check_id
                LEFT JOIN 
                (
                    gji_document_children survey_link
                    JOIN gji_document survey ON survey.id = survey_link.children_id 
                         AND survey.type_document = {(int)TypeDocumentGji.ActSurvey}
                ) ON survey_link.parent_id = doc.id
                LEFT JOIN
                (
                    gji_document_children act_check_link
                    JOIN gji_document act_check ON act_check.id = act_check_link.children_id 
                    AND act_check.type_document = {(int)TypeDocumentGji.ActCheck}
                ) ON act_check_link.parent_id = doc.id
                LEFT JOIN 
                (
                    gji_document_inspector insp_link
                    JOIN gkh_dict_inspector inspector ON inspector.id = insp_link.inspector_id
                ) ON insp_link.document_id = doc.id
                LEFT JOIN 
                (
                    gji_document_children parent_link
                    JOIN gji_inspection_viol_stage viol_stage ON viol_stage.document_id = parent_link.parent_id
                    JOIN gji_inspection_violation viol ON viol.id = viol_stage.inspection_viol_id
                    JOIN gkh_reality_object viol_ro ON viol_ro.id = viol.reality_object_id
                ) ON parent_link.children_id = doc.id
                LEFT JOIN 
                (
                    gji_inspection_robject insp_ro
                    JOIN gkh_reality_object ro ON insp_ro.reality_object_id = ro.id
                    JOIN gkh_dict_municipality romu ON romu.id = ro.municipality_id
                ) ON insp_ro.inspection_id = insp.id
                WHERE disp.type_disposal <> {(int)TypeDisposalGji.NullInspection} AND doc.type_document <> {(int)TypeDocumentGji.TaskDisposal}
                GROUP BY doc.id, disp.id, tat_disp.id, insp.id, mu.id, c.id, ml.id, gdct.id, kind_check.id, parent_link.id;";
        }
        
        /// <summary>
        /// Представление "Реестр предостережний"
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewWarningDoc(DbmsKind dbmsKind)
        {
	        return $@"
				CREATE OR REPLACE VIEW public.view_gji_warning_doc AS
				SELECT
					gd.id,
					gd.state_id,
					gd.document_num,
					gd.document_number,
					gd.document_date,
					gi.id AS inspection_id,
					gi.type_base,
					gwd.base_warning,
					CASE
                        WHEN gta.id NOTNULL THEN gtac.name
                        ELSE gc.name
                    END AS contragent_name,
					gwi.inspection_basis,
				    CASE
						WHEN gta.id NOTNULL THEN
							CASE gta.type_object
								{MatchEnumNames<TypeDocObject>()}
								ELSE ''
							END
						ELSE
							CASE gi.person_inspection
								{MatchEnumNames<PersonInspection>()}
								ELSE ''
							END
					END AS person_inspection,
					CASE
						WHEN gta.id NOTNULL THEN gta.person_name::varchar(300)
						ELSE gi.physical_person
					END AS physical_person,
					CASE
						WHEN gta.id NOTNULL THEN gta.municipality_id::integer
						ELSE gjigetinsprobjectmuid(gd.inspection_id)
					END AS mu_id,
					gjigetwarningdoccntrobj(gd.id::bigint) AS ro_count,
					gjigetwarningdocrobj(gd.id::bigint) AS ro_ids,
					gdi.fio::text AS inspectors,
				    CASE
						WHEN gta.id NOTNULL THEN gtagdm.name
						ELSE gjigetinsprobjectmuname(gd.inspection_id)
					END AS municipality,
					gd.type_document AS type_doc
				FROM public.gji_document gd
					JOIN public.gji_warning_doc gwd ON gwd.id = gd.id
					JOIN public.gji_inspection gi ON gi.id = gd.inspection_id
					LEFT JOIN public.gji_warning_inspection gwi ON gwi.id = gi.id
					LEFT JOIN public.gkh_contragent gc ON gc.id = gi.contragent_id
					LEFT JOIN public.gkh_dict_inspector gdi ON gdi.id = gwd.executant_id
					LEFT JOIN (public.gji_task_actionisolated gta
						JOIN public.gji_document gtagd ON gtagd.id = gta.id
						JOIN public.gkh_dict_municipality gtagdm ON gtagdm.id = gta.municipality_id
                        JOIN gkh_contragent gtac ON gtac.id = gta.contragent_id
					) ON gtagd.inspection_id = gd.inspection_id
						AND gi.type_base = {(int)TypeBase.ActionIsolated};

				COMMENT ON VIEW public.view_gji_warning_doc IS 'Реестр предостережений';";
        }

        #endregion
        
        #region Функции

        /// <summary>
        /// если распоряжение главное то возвращает строку адресов жилых домов основания
        /// иначе строку адресов жилых домов родительского документа
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDisposalRobjectAddress(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetDispRealityObjAddress(bigint, bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                    cnt integer := 0;
                    cursorCon CURSOR IS
                    select 
                        count(doc_child.id) 
                    from gji_document_children 
                doc_child where doc_child.children_id = $1;
                begin
                   OPEN cursorCon; 
                LOOP
                   FETCH cursorCon INTO cnt; 
                   EXIT WHEN not FOUND; 
                END LOOP;
                   CLOSE cursorCon; 
                    
                   if(cnt>0)
                   then
                       return gjiGetDocParRoAdrByViolStage($1);
                    end if;
                    return gjiGetInspRobjectAddress($2); 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }
        
        
        /// <summary>
        /// Адреса жилых домов акта проверки
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetActcheckRobjectAddress(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjiGetActCheckRobjAddress(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   address text :=''; 
                   result text :=''; 
                   zp text:='; ';  
                   cursorCon CURSOR IS 
                   select ro.address 
                   from GJI_ACTCHECK_ROBJECT actcheck_ro
                   join gkh_reality_object ro on ro.id = actcheck_ro.reality_object_id
                   where actcheck_ro.actcheck_id = $1; 
                begin 
                   OPEN cursorCon;
                   loop
                   FETCH cursorCon INTO address;
                   EXIT WHEN not FOUND; 
                   if(result!='') 
                   then 
                       result:=result || zp; 
                   end if;
                   result:=result || address; 
                   end loop;
                   CLOSE cursorCon; 
                   return result; 
                end; 
                $BODY$
                LANGUAGE plpgsql VOLATILE
                COST 100;";
        }
        
        /// <summary>
        /// Адреса жилых домов по нарушениям
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocParRoAdrByViolStage(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION public.gjigetdocparroadrbyviolstage(
                	bigint)
                    RETURNS text
                AS 
                $BODY$
                declare 
                   address text :=''; 
                   result text := ''; 
                   zp text:='; '; 
                   cursorCon CURSOR IS 
                   SELECT DISTINCT ro.address 
                   FROM
                   (
                       SELECT ro.address
                   	   FROM gji_document_children doc_child
                   	   INNER JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_child.parent_id
                   	   INNER JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
                   	   INNER JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
                   	   WHERE doc_child.children_id = $1
                   	   UNION 
                	   SELECT ro.address
                	   FROM gji_document doc
                	   JOIN gji_document mvd_doc ON doc.inspection_id = mvd_doc.inspection_id
                	   JOIN gji_prot_mvd_robject mvd_ro ON mvd_ro.prot_mvd_id = mvd_doc.id
                	   JOIN gkh_reality_object ro ON ro.id = mvd_ro.reality_object_id
                	   WHERE doc.id = $1
                	   UNION
                	   SELECT ro.address
                	   FROM gji_document doc
                	   JOIN gji_document gji_protocol_doc ON doc.inspection_id = gji_protocol_doc.inspection_id
                	   JOIN gji_tatarstan_protocol_gji_reality_object gji_protocol_ro ON gji_protocol_ro.tatarstan_protocol_gji_id = gji_protocol_doc.id
                	   JOIN gkh_reality_object ro ON ro.id = gji_protocol_ro.reality_object_id
                	   WHERE doc.id = $1
                   ) ro;
                begin 
                   OPEN cursorCon;
                   loop 
                    FETCH cursorCon INTO address; 
                    EXIT WHEN not FOUND; 
                    if(result!='') 
                    then 
                        result:=result || zp; 
                    end if; 
                    result:=result || address; 
                    end loop; 
                    CLOSE cursorCon; 
                    return result; 
                    end;
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        #endregion Функции
        #endregion Create
        #region Delete
        #region Функции
        
        /// <summary>
        /// gjiGetActCheckRobjAdress(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetActcheckRobjectAddress(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropFunctionOracleQuery("gjiGetActCheckRobjAddress");
            }

            return @"DROP FUNCTION if exists gjiGetActCheckRobjAddress(bigint)";
        }

        /// <summary>
        /// Сопоставление значений Enum с их именами для отображения
        /// </summary>
        /// <typeparam name="TEnum">Тип перечисления</typeparam>
        /// <returns>SQL-строка</returns>
        private string MatchEnumNames<TEnum>()
            where TEnum : Enum
            =>
            string.Join("", Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(x => $"WHEN {Convert.ToInt32(x)} THEN '{x.GetDisplayName()}' "));

        #endregion Функции
        #endregion Delete
    }
}