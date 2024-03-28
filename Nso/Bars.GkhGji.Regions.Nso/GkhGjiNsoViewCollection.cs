namespace Bars.GkhGji.Regions.Nso
{
	using Bars.Gkh;
	using Bars.B4.Modules.Ecm7.Framework;

    public class GkhGjiNsoViewCollection : BaseGkhViewCollection
    {
        public override int Number => 1;

        #region Create
        #region Функции

        /// <summary>
        /// возвращает строку наименвоаний муниципальных образований жилых домов родительского документа
        /// gjiGetDocParentPlaceByViolStg
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateFunctionGetDocumentParentRobjectPlaceNameByViolStage(DbmsKind dbmsKind)
        {
            return @"
                CREATE OR REPLACE FUNCTION gjigetdocparentplacebyviolstg(bigint)
                  RETURNS text AS
                $BODY$ 
                declare 
                   mu_name text :=''; 
                   zp text := ', ';
                   result text := ''; 
                   cursorCon CURSOR IS 
                   SELECT DISTINCT munt.place_name
                            FROM 
                            (
	                    select 
		                distinct doc_child.children_id, fa.place_name
	                    from gji_document_children doc_child
		                JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
		                JOIN gji_inspection_viol_stage viol_stage on viol_stage.document_id = doc_par.Id
		                JOIN gji_inspection_violation viol on viol.id = viol_stage.inspection_viol_id
		                JOIN gkh_reality_object ro on ro.id = viol.reality_object_id
		                join b4_fias_address fa ON fa.id = ro.fias_address_id
	                    where doc_par.TYPE_DOCUMENT != 80
                            UNION ALL
                            select 
	                       distinct doc_child.children_id, fa.place_name
                            from gji_document_children doc_child
                                    JOIN GJI_DOCUMENT doc_par on doc_par.Id = doc_child.parent_id
	                            JOIN GJI_RESOLPROS_ROBJECT GRO on GRO.RESOLPROS_ID= doc_par.Id
	                            JOIN gkh_reality_object ro on ro.id = GRO.REALITY_OBJECT_ID
	                            join b4_fias_address fa ON fa.id = ro.fias_address_id
                            where doc_par.TYPE_DOCUMENT = 80
	                ) munt
                   where munt.children_id = $1;
                begin 
                   OPEN cursorCon;
                   loop 
                   FETCH cursorCon INTO mu_name; 
                   EXIT WHEN not FOUND;
                   if(result <> '')
                   then
                      result:=result || zp; 
                   end if;
                   result := result || mu_name;
                   end loop; 
                   CLOSE cursorCon; 
                   return result; 
                end; 
                $BODY$
                  LANGUAGE plpgsql VOLATILE
                  COST 100;";
        }

        #endregion Функции
        #region Вьюхи

        /// <summary>
        /// Вьюха обращений граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewAppealCits(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_appeal_cits;
CREATE VIEW view_gji_appeal_cits AS 
SELECT 
    gac.id, 
    gac.DOCUMENT_NUMBER, 
    gac.gji_number, 
    gac.date_from, 
    gac.check_time, 
    gac.questions_count, 
    countro.count_ro, 
    mun.municipality, 
    mo_settlement,
    place_name,
    c.name AS 
    contragent_name, 
    gac.state_id,
    gac.executant_id, 
    gac.tester_id, 
    gac.SURETY_RESOLVE_ID,
    gac.EXECUTE_DATE,
    gac.SURETY_DATE,
    gjiGetAppealRobject(gac.id) AS ro_ids,
    gac.ZONAINSP_ID,
    gjiGetRobjectAdrAppeal(gac.id) AS ro_adr,
    gac.correspondent,
    gac.correspondent_address,
    mun.municipality_id,
    gjiGetRevenueSourceNames(gac.id) as source_names,
    gjiGetRevenueSourceDates(gac.id::bigint) AS source_dates,
    gjiGetRevenueSourceNumbers(gac.id::bigint) AS source_numbers,
    '' as executant_names,
    '' as subjects
FROM gji_appeal_citizens gac
    LEFT JOIN ( 
        SELECT 
            count(garo.reality_object_id) AS count_ro, 
            gac1.id AS gac_id
        FROM gji_appeal_citizens gac1
            JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
            JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
            GROUP BY gac1.id
        ) countro ON countro.gac_id = gac.id
    LEFT JOIN ( 
        SELECT 
            gaac.id AS gac_id, 
            ( 
                SELECT 
                    gdm.name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS municipality,
            ( 
                SELECT 
                    gdm.id AS municipality_id
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS municipality_id,
            ( 
                SELECT 
                    gdm.name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN gkh_dict_municipality gdm ON gdm.id = gro.stl_municipality_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS mo_settlement,
            ( 
                SELECT 
                    fa.place_name AS municipality
                FROM gji_appeal_citizens gac1
                    JOIN gji_appcit_ro garo ON garo.appcit_id = gac1.id
                    JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
                    JOIN b4_fias_address fa ON fa.id = gro.fias_address_id
                    WHERE gac1.id = gaac.id
                LIMIT 1
            ) AS place_name
        FROM gji_appeal_citizens gaac
        ) mun ON mun.gac_id = gac.id
    LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
    LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id";
		}

		/// <summary>
		/// Вьюха оснований плановой проверки юр лиц
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsJurpers(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_ins_jurpers;
CREATE VIEW view_gji_ins_jurpers AS 
 SELECT 
    insp.id, 
    insp.id AS inspection_id, 
    plan.id AS plan_id, 
    plan.name AS plan_name, 
    c.name AS contragent_name, 
    t.type_fact, 
    t.count_days, 
    t.date_start, 
    insp.inspection_number, 
    state.id as state_id,
    gjiGetInspDisposalNumber(insp.id) AS disp_number, 
    gjiGetInspectionInsp(insp.id) AS inspectors, 
    gjiGetInspRobject(insp.id) AS ro_ids, 
    gjiGetInspRobjectAddress(insp.id) AS ro_address, 
    gjiGetInspRobjectMuName(insp.id) AS mu_names, 
	gjiGetInspRobjectMoName(insp.id) AS mo_names,
	gjiGetInspRobjectPlaceName(insp.id) AS place_names,
    (select count(insp_ro.reality_object_id) 
            from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_jurperson t ON t.id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN gji_dict_planjurperson plan ON t.plan_id = plan.id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
		}

		/// <summary>
		/// вьюха оснований проверки по требованию прокуратуры
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsProsclaim(DbmsKind dbmsKind)
		{
			return @"
                    DROP VIEW IF EXISTS view_gji_ins_prosclaim;
                    CREATE VIEW view_gji_ins_prosclaim AS 
                     SELECT 
                        insp.id, 
                        c.name AS contragent_name, 
                        t.prosclaim_date_check, 
                        t.document_number, 
                        insp.person_inspection,
                        insp.inspection_number, 
                        insp.type_jur_person,
                        state.id as state_id,
                        gjiGetInspectionInsp(insp.id) AS inspectors, 
                        gjiGetInspRobjectMuName(insp.id) AS mu_names,
						gjiGetInspRobjectMoName(insp.id) AS mo_names,
						gjiGetInspRobjectPlaceName(insp.id) AS place_names,
                        (select count(insp_ro.reality_object_id) 
                             from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
                        ( select distinct mu.id 
                             from gji_inspection_robject gji_ro
                             join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
                             left join gkh_dict_municipality mu on mu.id = ro.municipality_id
                             where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
                    FROM gji_inspection insp
                        JOIN gji_inspection_prosclaim t ON t.id = insp.id
                        LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
                        LEFT JOIN b4_state state ON insp.state_id = state.id";
		}

		/// <summary>
		/// Вьюха оснований проверки без основания
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsBasedef(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_ins_basedef;
CREATE VIEW view_gji_ins_basedef AS 
SELECT 
    insp.id, 
    insp.inspection_num, 
    insp.physical_person, 
    c.name AS contragent_name, 
    state.id as state_id,
    gjiGetInspRobjectMuName(insp.id) AS mu_names,
	gjiGetInspRobjectMoName(insp.id) AS mo_names,
	gjiGetInspRobjectPlaceName(insp.id) AS place_names,
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_basedef t ON t.id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
		}

		/// <summary>
		/// вьюха оснований проверки по поручению руководства
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsDisphead(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_ins_disphead;
CREATE VIEW view_gji_ins_disphead AS 
SELECT 
    t.id, 
    t.disphead_date, 
    c.name AS contragent_name, 
    t.head_id, 
    head.fio AS head_fio,
    t.document_number, 
    insp.inspection_number, 
    insp.person_inspection, 
    insp.type_jur_person,
    state.id as state_id,
    gjiGetInspDisposalTypeSurveys(insp.id) AS disp_types, 
    gjiGetInspectionInsp(insp.id) AS inspectors, 
    gjiGetInspRobjectMuName(insp.id) AS mu_names, 
	gjiGetInspRobjectMoName(insp.id) AS mo_names,
	gjiGetInspRobjectPlaceName(insp.id) AS place_names,
    (select count(insp_ro.reality_object_id) 
            from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_disphead t ON t.id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN gkh_dict_inspector head ON head.id = t.head_id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
		}

		/// <summary>
		/// Вьюха оснований инспекционной проверки
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsInschek(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_ins_inschek;
CREATE VIEW view_gji_ins_inschek AS 
 SELECT 
    insp.id, 
    plan.id AS plan_id, 
    plan.name AS plan_name, 
    t.inscheck_date, 
    c.name AS contragent_name, 
    insp.inspection_number, 
    t.type_fact, 
    state.id as state_id,
    gjiGetInspDisposalNumber(insp.id) AS disp_number, 
    gjiGetInspectionInsp(insp.id) AS inspectors, 
    gjiGetInspRobject(insp.id) AS ro_ids, 
    gjiGetInspRobjectAddress(insp.id) AS ro_address, 
    gjiGetInspRobjectMuName(insp.id) AS mu_names, 
	gjiGetInspRobjectMoName(insp.id) AS mo_names,
	gjiGetInspRobjectPlaceName(insp.id) AS place_names,
    (select count(insp_ro.reality_object_id) 
            from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count, 
    ( select distinct mu.id 
            from gji_inspection_robject gji_ro
            join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
            left join gkh_dict_municipality mu on mu.id = ro.municipality_id
            where gji_ro.inspection_id = insp.id limit 1 ) AS mu_id
FROM gji_inspection insp
    JOIN gji_inspection_inscheck t ON t.id = insp.id
    LEFT JOIN gji_dict_planinscheck plan ON t.plan_id = plan.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN b4_state state ON insp.state_id = state.id";
		}

		/// <summary>
		/// Вьюха оснований проверки по обращению граждан
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsStatement(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_ins_statement;
CREATE VIEW view_gji_ins_statement AS 
SELECT 
    insp.id, 
    ctr.name AS contragent_name, 
    insp.person_inspection, 
    insp.inspection_number, 
    insp.type_jur_person,
    gjigetinsprobjectmuname(insp.id) AS mu_names,
	gjiGetInspRobjectMoName(insp.id) AS mo_names,
	gjiGetInspRobjectPlaceName(insp.id) AS place_names,
    gjiGetInspRobjectAdr(insp.id) as ro_adr,
    state.id as state_id,
    (select count(insp_ro.reality_object_id) 
        from gji_inspection_robject insp_ro where insp_ro.inspection_id = insp.id) AS ro_count,
    ( select distinct mu.id 
        from gji_inspection_robject gji_ro
        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
        where gji_ro.inspection_id = insp.id limit 1) AS mu_id,
    (CASE WHEN DOCS.COUNT_DOC > 0 THEN true ELSE FALSE end) AS is_disposal
FROM gji_inspection insp
	JOIN gji_inspection_statement t ON t.id = insp.id
	LEFT JOIN gkh_contragent ctr ON ctr.id = insp.contragent_id
    LEFT JOIN b4_state state ON insp.state_id = state.id
	LEFT JOIn
	(
		select 
		doc.inspection_id, 
		COUNT(doc.Id)COUNT_DOC
               from gji_disposal disp
               inner join gji_document doc on doc.id = disp.id
               where disp.type_disposal = 10 
               GROUP BY doc.inspection_id
	
	) DOCS ON DOCS.inspection_id = insp.ID";
		}

		/// <summary>
		/// Вьюха оснований проверки по cоискателям лицензии
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewInsLicApplicants(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_ins_license_app;
CREATE VIEW view_gji_ins_license_app AS 
SELECT 
    insp.id, 
    ctr.name AS contragent_name, 
    insp.person_inspection, 
    insp.inspection_number, 
    insp.type_jur_person,
    gjigetinsprobjectmuname(insp.id) AS mu_names,
	gjiGetInspRobjectMoName(insp.id) AS mo_names,
	gjiGetInspRobjectPlaceName(insp.id) AS place_names,
    gjiGetInspRobjectAdr(insp.id) as ro_adr,
    state.id as state_id,
    ( select distinct mu.id 
        from gji_inspection_robject gji_ro
        join gkh_reality_object ro on ro.id = gji_ro.reality_object_id
        left join gkh_dict_municipality mu on mu.id = ro.municipality_id
        where gji_ro.inspection_id = insp.id limit 1) AS mu_id,
    (CASE WHEN DOCS.COUNT_DOC > 0 THEN true ELSE FALSE end) AS is_disposal,
    req.reg_number,
    ctr.id as ctr_id
FROM gji_inspection insp
	JOIN GJI_INSPECTION_LIC_APP t ON t.id = insp.id
    LEFT JOIN gkh_manorg_lic_request req ON req.id = t.man_org_lic_id
	LEFT JOIN gkh_contragent ctr ON ctr.id = req.contragent_id
    LEFT JOIN b4_state state ON insp.state_id = state.id
	LEFT JOIN
	(
		select 
		doc.inspection_id, 
		COUNT(doc.Id)COUNT_DOC
               from gji_disposal disp
               inner join gji_document doc on doc.id = disp.id
               where disp.type_disposal = 10 
               GROUP BY doc.inspection_id
	
	) DOCS ON DOCS.inspection_id = insp.ID";
		}

		/// <summary>
		/// Вьюха актов устранения нарушений
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewActRemoval(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_act_removal;
CREATE VIEW view_gji_act_removal AS 
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
    gjiGetDocMuByViolStage(doc.id) AS mu_names, 
    gjiGetDocMoByViolStage(doc.id) AS mo_names, 
    gjiGetDocPlaceByViolStage(doc.id) AS place_names, 
    gjiGetDocMuIdByViolStage(doc.id) AS mu_id, 
    insp.id AS inspection_id, 
    insp.type_base, 
    act.type_removal, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_actremoval act ON act.id = doc.id
    JOIN gji_document_children ch ON ch.children_id = doc.id
    JOIN gji_document docparent ON docparent.type_document <> 20 AND docparent.id = ch.parent_id
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
			return @"
DROP VIEW IF EXISTS view_gji_actcheck;
CREATE VIEW view_gji_actcheck AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    gjiGetDocumentInspectors(doc.id) AS inspector_names, 
    gjiGetActCheckCntRobj(doc.id) AS count_ro, 
    gjiGetActCheckHasViolation(doc.id) AS has_violation, 
    gjiGetDocCountChildDoc(doc.id) AS count_exec_doc, 
    gjiGetActCheckRobj(doc.id) AS ro_ids, 
    gjiGetActCheckRobjMun(doc.id) AS mu_names, 
    gjiGetActCheckRobjMo(doc.id) AS mo_names, 
    gjiGetActCheckRobjPlace(doc.id) AS place_names, 
    gjiGetActCheckRobjectMuId(doc.id) AS mu_id, 
    insp.id AS inspection_id,
    insp.type_base,
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name, 
    doc.type_document AS type_doc
FROM gji_document doc
	JOIN gji_actcheck act on doc.id = act.id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
    LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
WHERE doc.type_document = 20 and act.type_actcheck <> 30";
		}

		/// <summary>
		/// вьюха актов обследования
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewActsurvey(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_actsurvey;
CREATE VIEW view_gji_actsurvey AS 
SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    gjiGetDocumentInspectors(act.id) AS inspector_names, 
    gjiGetActSurvRobjAddress(act.id) AS ro_address, 
    gjiGetActSurvRobj(act.id) AS ro_ids, 
    gjiGetActSurveyRobjMun(act.id) AS mu_names, 
    gjiGetActSurveyRobjMo(act.id) AS mo_names, 
    gjiGetActSurveyRobjPlace(act.id) AS place_names, 
    gjiGetActSurveyRobjMunId(act.id) AS mu_id, 
    insp.id AS inspection_id, 
    insp.type_base,
    act.fact_surveyed, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_actsurvey act ON act.id = doc.id
    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id";
		}

		/// <summary>
		/// Вьюха распоряжений
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewDisposal(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_disposal;
CREATE VIEW view_gji_disposal AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_num, 
    doc.document_number, 
    doc.document_date, 
    insp.id AS inspection_id, 
    insp.type_base,
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent, 
    disp.date_start, 
    disp.date_end, 
    kind_check.name as kind_check_name,
    gjiGetDisposalTypeSurveys(doc.id) AS tsurveys_name, 
    gjiGetDispCntRobj(doc.id, doc.inspection_id) AS ro_count, 
    gjiGetDispActCheckExist(doc.id) AS act_check_exist, 
    gjiGetDocumentInspectors(doc.id) AS inspector_names, 
    gjiGetDispRealityObj(doc.id, doc.inspection_id) AS ro_ids, 
    gjiGetInspRobjectMuName(doc.inspection_id) AS mu_names, 
    gjiGetInspRobjectMoName(doc.inspection_id) AS mo_names, 
    gjiGetInspRobjectPlaceName(doc.inspection_id) AS place_names, 
    gjiGetInspRobjectMuId(doc.inspection_id) AS mu_id, 
    doc.type_document AS type_doc,
    disp.type_disposal,
    disp.type_agrprosecutor,
    insp.control_type
FROM gji_document doc
    JOIN gji_disposal disp ON disp.id = doc.id
    LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
    LEFT JOIN gkh_contragent c ON insp.contragent_id = c.id
    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
    LEFT JOIN b4_state state ON doc.state_id = state.id
    left join gji_dict_kind_check kind_check on kind_check.id = disp.kind_check_id
WHERE disp.type_disposal <> 30";
		}

		/// <summary>
		/// Вьюха предписаний
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewPrescription(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_prescription;
CREATE VIEW view_gji_prescription AS 
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
    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
    gjiGetDocMoByViolStage(pr.id) AS mo_names, 
    gjiGetDocPlaceByViolStage(pr.id) AS place_names, 
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
			max(iv.DATE_PLAN_REMOVAL) AS date_removal, 
			viol.document_id
		FROM gji_inspection_viol_stage viol
		JOIN gji_inspection_violation iv ON iv.id = viol.inspection_viol_id
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
DROP VIEW IF EXISTS view_gji_protocol;
CREATE VIEW view_gji_protocol AS 
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
    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
    gjiGetDocMoByViolStage(pr.id) AS mo_names, 
    gjiGetDocPlaceByViolStage(pr.id) AS place_names, 
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
		/// Вьюха протоколов 19.7
		/// </summary>
		/// <param name="dbmsKind"></param>
		/// <returns></returns>
		private string CreateViewProtocol197(DbmsKind dbmsKind)
		{
			return @"
DROP VIEW IF EXISTS view_gji_protocol197;
CREATE VIEW view_gji_protocol197 AS 
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
    gjiGetDocMuByViolStage(pr.id) AS mu_names, 
    gjiGetDocMoByViolStage(pr.id) AS mo_names, 
    gjiGetDocPlaceByViolStage(pr.id) AS place_names, 
    gjiGetDocMuIdByViolStage(pr.id) AS mu_id, 
    mu.name as ctr_mu_name,
    c.municipality_id as ctr_mu_id,
    c.name AS contragent_name, 
    exec.name AS type_exec_name, 
    insp.id AS inspection_id, 
    insp.type_base, 
    doc.type_document AS type_doc
FROM gji_document doc
    JOIN gji_protocol197 pr ON pr.id = doc.id
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
DROP VIEW IF EXISTS view_gji_resolution;
CREATE VIEW view_gji_resolution AS 
 SELECT 
    doc.id, 
    doc.state_id, 
    doc.id AS document_id, 
    doc.document_date, 
    doc.document_number, 
    doc.document_num, 
    payfine.sum_pays, 
    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobject(doc.id) ELSE gjiGetDocParentRobjByViolStage(doc.id) end) AS ro_ids, 
    gjiGetDocParentMuByViolStage(doc.id) AS mu_names, 
    gjiGetDocParentMoByViolStage(doc.id) AS mo_names, 
    gjiGetDocParentPlaceByViolStg(doc.id) AS place_names, 
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
    res.BECAME_LEGAL
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
        #endregion Вьюхи
        #endregion Create
        #region Delete
        #region Вьюхи
        private string DeleteViewAppealCits(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_appeal_cits");
		}

		private string DeleteViewInsJurpers(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_jurpers");
		}

		private string DeleteViewInsProsclaim(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_prosclaim");
		}

		private string DeleteViewInsBasedef(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_basedef");
		}

		private string DeleteViewInsDisphead(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_disphead");
		}

		private string DeleteViewInsInschek(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_inschek");
		}

		private string DeleteViewInsStatement(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_statement");
		}

		private string DeleteViewLicenseApp(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_ins_license_app");
		}

		private string DeleteViewActRemoval(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_act_removal");
		}

		private string DeleteViewActcheck(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_actcheck");
		}

		private string DeleteViewActsurvey(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_actsurvey");
		}

		private string DeleteViewDisposal(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_disposal");
		}

		private string DeleteViewPrescription(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_prescription");
		}

		private string DeleteViewProtocol(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_protocol");
		}

		private string DeleteViewProtocol197(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_protocol197");
		}

		private string DeleteViewResolution(DbmsKind dbmsKind)
		{
			return DropViewPostgreQuery("view_gji_resolution");
		}
        #endregion Вьюхи	
        #region Функции

        /// <summary>
        /// удаление gjiGetDocParentPlaceByViolStg(bigint)
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string DeleteFunctionGetDocumentParentRobjectPlaceNameByViolStage(DbmsKind dbmsKind)
        {
            if (dbmsKind == DbmsKind.Oracle)
            {
                return this.DropFunctionOracleQuery("gjiGetDocParentPlaceByViolStg");
            }

            return @"DROP FUNCTION if exists gjiGetDocParentPlaceByViolStg(bigint)";
        }

        #endregion Функции
        #endregion Delete
    }
}