namespace Bars.GkhGji.Migrations._2024.Version_2024030117
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030117")]
    [MigrationDependsOn(typeof(Version_2024030116.UpdateSchema))]
    /// Является Version_2020121400 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
//            const string query = @"
//CREATE OR REPLACE VIEW view_gji_resolution AS 
// SELECT 
//    doc.id, 
//    doc.state_id, 
//    doc.id AS document_id, 
//    doc.document_date, 
//    doc.document_number, 
//    doc.document_num, 
//    payfine.sum_pays, 
//    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobject(doc.id) ELSE gjiGetDocParentRobjByViolStage(doc.id) end) AS ro_ids, 
//    gjiGetDocParentMuByViolStage(doc.id) AS mu_names, 
//    ''::text AS mo_names,
//    ''::text AS place_names,
//    gjiGetDocParentMuIdByViolStage(doc.id) AS mu_id, 
//    (CASE WHEN insp.type_base = 60 THEN gjiGetResolProsRobjectAddress(doc.id) ELSE gjiGetDocParRoAdrByViolStage(doc.id) end) AS ro_address, 
//    inspector.fio AS official_name, 
//    res.official_id, 
//    res.penalty_amount, 
//    insp.id AS inspection_id, 
//    insp.type_base, 
//    exec.name AS type_exec_name, 
//    s.name AS sanction_name,
//    mu.name as ctr_mu_name,
//    c.municipality_id as ctr_mu_id,
//    c.name AS contragent_name, 
//    doc.type_document AS type_doc,
//    res.DELIVERY_DATE,
//    res.PAIDED,
//    res.BECAME_LEGAL,
//    doc.GIS_UIN,
//    res.TYPE_INITIATIVE_ORG
//FROM gji_document doc
//    JOIN gji_resolution res ON res.id = doc.id
//    LEFT JOIN ( 
//        SELECT 
//            gji_resolution_payfine.resolution_id, 
//            sum(gji_resolution_payfine.amount) AS sum_pays
//        FROM gji_resolution_payfine
//        GROUP BY gji_resolution_payfine.resolution_id
//        ) payfine ON payfine.resolution_id = doc.id
//    LEFT JOIN gkh_dict_inspector inspector ON inspector.id = res.official_id
//    LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
//    LEFT JOIN gkh_contragent c ON c.id = res.contragent_id
//    LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
//    LEFT JOIN gji_dict_executant exec ON exec.id = res.executant_id
//    LEFT JOIN gji_dict_sanction s ON s.id = res.sanction_id";
//            this.Database.ExecuteNonQuery(query);
        }
    }
}