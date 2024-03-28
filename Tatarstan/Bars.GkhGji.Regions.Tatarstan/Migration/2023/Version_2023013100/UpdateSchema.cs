namespace Bars.GkhGji.Regions.Tatarstan.Migration._2023.Version_2023013100
{
	using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Enums;

    [Migration("2023013100")]
    [MigrationDependsOn(typeof(_2022.Version_2022120800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
	    /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery($@"
                DROP VIEW IF EXISTS view_gji_tat_disposal;

                CREATE VIEW view_gji_tat_disposal AS
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
                GROUP BY doc.id, disp.id, tat_disp.id, insp.id, mu.id, c.id, ml.id, gdct.id, kind_check.id, parent_link.id;");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ExecuteNonQuery($@"
                DROP VIEW IF EXISTS view_gji_tat_disposal;

                CREATE VIEW view_gji_tat_disposal AS
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
                    ml.id AS license
                FROM gji_document doc
                JOIN gji_disposal disp ON disp.id = doc.id
                LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
                LEFT JOIN gji_inspection_lic_app ila ON doc.inspection_id = ila.id
                LEFT JOIN gkh_manorg_lic_request mlr ON ila.man_org_lic_id = mlr.id
                LEFT JOIN gkh_contragent c ON
                   CASE
                       WHEN insp.type_base = {(int)TypeBase.LicenseApplicants} THEN mlr.contragent_id
                       ELSE insp.contragent_id
                   END = c.id
                LEFT JOIN gkh_manorg_license ml ON ml.contragent_id = c.id
                LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
                LEFT JOIN b4_state state ON doc.state_id = state.id
                LEFT JOIN gji_dict_kind_check kind_check ON kind_check.id = disp.kind_check_id
                WHERE disp.type_disposal <> {(int)TypeDisposalGji.NullInspection} AND doc.type_document <> {(int)TypeDocumentGji.TaskDisposal};");
        }
    }
}