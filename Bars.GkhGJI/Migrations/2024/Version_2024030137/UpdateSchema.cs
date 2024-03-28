namespace Bars.GkhGji.Migrations._2024.Version_2024030137
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;
    using System.Data;

    [Migration("2024030137")]
    [MigrationDependsOn(typeof(Version_2024030136.UpdateSchema))]
    /// Является Version_2023013100 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
    //        this.Database.ExecuteNonQuery($@"
				//DROP VIEW IF EXISTS view_gji_actcheck;

				//CREATE VIEW view_gji_actcheck AS
				//SELECT doc.id,
				//    doc.state_id,
				//    doc.id AS document_id,
				//    doc.document_date,
				//    doc.document_number,
				//    doc.document_num,
				//    STRING_AGG(inspector.fio, ', '::text) AS inspector_names,
				//    COUNT(DISTINCT ro.id) AS count_ro,
				//    COALESCE(NULLIF(MIN(aro.have_violation), {(int)YesNoNotSet.NotSet}), {(int)YesNo.No}) AS has_violation,
				//    COUNT(DISTINCT child_doc.id) AS count_exec_doc,
				//    STRING_AGG(DISTINCT ro.id::text, '/'::text) AS ro_ids,
				//    STRING_AGG(DISTINCT ro.address, ', '::text) AS ro_addresses,
				//    STRING_AGG(DISTINCT mun.name, ', '::text) AS mu_names,
				//    MIN(mun.id) AS mu_id,
				//    insp.id AS inspection_id,
				//    insp.type_base,
				//    mu.name AS ctr_mu_name,
				//    c.municipality_id AS ctr_mu_id,
				//    c.name AS contragent_name,
				//    doc.type_document AS type_doc
				//   FROM gji_document doc
				//   JOIN gji_inspection insp ON insp.id = doc.inspection_id
				//   LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
				//   LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
				//   LEFT JOIN
				//   (
				//    gji_document_inspector insp_link
				//      	 JOIN gkh_dict_inspector inspector ON inspector.id = insp_link.inspector_id 
				//   ) ON insp_link.document_id = doc.id
				//   LEFT JOIN 
				//   (
				//		gji_actcheck_robject aro
				//		JOIN gkh_reality_object ro ON aro.reality_object_id = ro.id
				//		JOIN gkh_dict_municipality mun ON mun.id = ro.municipality_id
				//   ) ON aro.actcheck_id = doc.id
				//   LEFT JOIN
				//   (
				//		gji_document_children child
				//		JOIN gji_document child_doc ON child_doc.id = child.children_id
				//    	     AND child_doc.type_document != {(int)TypeDocumentGji.ActRemoval}
				//   ) ON child.parent_id = doc.id
				//   WHERE doc.type_document = {(int)TypeDocumentGji.ActCheck}
				//   GROUP BY doc.id, insp.id, mu.id, c.id");
        }

        /// <inheritdoc />
        public override void Down()
        {
    //        this.Database.ExecuteNonQuery($@"
				//DROP VIEW IF EXISTS view_gji_actcheck;

				//CREATE VIEW view_gji_actcheck
 			//	AS
 			//	SELECT doc.id,
 			//	   doc.state_id,
 			//	   doc.id AS document_id,
 			//	   doc.document_date,
 			//	   doc.document_number,
 			//	   doc.document_num,
 			//	   gjigetdocumentinspectors(doc.id::bigint) AS inspector_names,
 			//	   gjigetactcheckcntrobj(doc.id::bigint) AS count_ro,
 			//	   gjigetactcheckhasviolation(doc.id::bigint) AS has_violation,
 			//	   gjigetdoccountchilddoc(doc.id::bigint) AS count_exec_doc,
 			//	   gjigetactcheckrobj(doc.id::bigint) AS ro_ids,
 			//	   gjigetactcheckrobjaddress(doc.id::bigint) AS ro_addresses,
 			//	   gjigetactcheckrobjmun(doc.id::bigint) AS mu_names,
 			//	   ''::text AS mo_names,
 			//	   ''::text AS place_names,
 			//	   gjigetactcheckrobjectmuid(doc.id::bigint) AS mu_id,
 			//	   insp.id AS inspection_id,
 			//	   insp.type_base,
 			//	   mu.name AS ctr_mu_name,
 			//	   c.municipality_id AS ctr_mu_id,
 			//	   c.name AS contragent_name,
 			//	   doc.type_document AS type_doc
 			//	FROM gji_document doc
 			//	LEFT JOIN gji_inspection insp ON insp.id = doc.inspection_id
 			//	LEFT JOIN gkh_contragent c ON c.id = insp.contragent_id
 			//	LEFT JOIN gkh_dict_municipality mu ON mu.id = c.municipality_id
 			//	WHERE doc.type_document = {(int)TypeDocumentGji.ActCheck};");
        }
    }
}