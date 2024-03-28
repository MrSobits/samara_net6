namespace Bars.GkhGji.Migrations._2024.Version_2024030118
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030118")]
    [MigrationDependsOn(typeof(Version_2024030117.UpdateSchema))]
    /// Является Version_2021020300 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
//            const string query = @"
//CREATE OR REPLACE VIEW view_gji_disposal AS 
// SELECT 
//    doc.id, 
//    doc.state_id, 
//    doc.id AS document_id, 
//    doc.document_num, 
//    doc.document_number, 
//    doc.document_date, 
//    insp.id AS inspection_id, 
//    insp.type_base,
//    mu.name as ctr_mu_name,
//    c.municipality_id as ctr_mu_id,
//    c.name AS contragent,
//    disp.date_start, 
//    disp.date_end, 
//    kind_check.name as kind_check_name,
//    gjiGetDisposalTypeSurveys(doc.id) AS tsurveys_name, 
//    gjiGetDispCntRobj(doc.id, doc.inspection_id) AS ro_count, 
//    gjiGetDispActCheckExist(doc.id) AS act_check_exist, 
//    gjiGetDocumentInspectors(doc.id) AS inspector_names, 
//    gjiGetDispRealityObj(doc.id, doc.inspection_id) AS ro_ids, 
//    gjiGetInspRobjectMuName(doc.inspection_id) AS mu_names,
//    ''::text AS mo_names,
//    ''::text AS place_names, 
//    gjiGetInspRobjectMuId(doc.inspection_id) AS mu_id, 
//    doc.type_document AS type_doc,
//    disp.type_disposal,
//    disp.type_agrprosecutor,
//    insp.control_type,
//    ml.id AS license
//FROM gji_document doc
//    JOIN gji_disposal disp ON disp.id = doc.id
//    LEFT JOIN gji_inspection insp ON doc.inspection_id = insp.id
//    LEFT JOIN gji_inspection_lic_app ila ON doc.inspection_id = ila.id
//    LEFT JOIN gkh_manorg_lic_request mlr ON ila.man_org_lic_id = mlr.id
//    LEFT JOIN gkh_contragent c ON case when insp.type_base = 130 then mlr.contragent_id else insp.contragent_id end = c.id
//    LEFT JOIN gkh_manorg_license ml ON ml.contragent_id = c.id
//    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
//    LEFT JOIN b4_state state ON doc.state_id = state.id
//    left join gji_dict_kind_check kind_check on kind_check.id = disp.kind_check_id
//WHERE disp.type_disposal <> 30 AND doc.type_document <> 13";
//            this.Database.ExecuteNonQuery(query);
        }
    }
}