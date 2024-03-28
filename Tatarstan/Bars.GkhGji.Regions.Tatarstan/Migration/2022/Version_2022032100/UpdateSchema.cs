namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022032100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022032100")]
    [MigrationDependsOn(typeof(Version_2022031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string ViolationDocTableName = "GJI_WARNING_DOC_VIOLATIONS";
        
        public override void Up()
        {
	        // Обновляем неверные ключи reality_object_id в таблице gji_warning_doc_violations
	        // Если остались ключи ссылающиеся на не существующие gkh_reality_object то обнуляем
            this.Database.ExecuteNonQuery(
                @"WITH q1 AS(
						SELECT doc_viol.id AS doc_viol_id, 
							   insp_ro.reality_object_id AS gji_insp_ro
						FROM gji_warning_doc_violations AS doc_viol
						INNER JOIN gji_warning_doc AS war_doc
							ON doc_viol.warning_doc_id = war_doc.id
						INNER JOIN gji_document AS doc
							ON doc.id = war_doc.id
						INNER JOIN gji_inspection_robject as insp_ro
							ON doc.inspection_id = insp_ro.inspection_id AND doc_viol.reality_object_id = insp_ro.id
						LEFT JOIN gkh_reality_object AS ro
							ON doc_viol.reality_object_id = ro.id
								WHERE ro.id is NULL)
					UPDATE gji_warning_doc_violations
					SET reality_object_id = q1.gji_insp_ro
					FROM q1
					WHERE id = q1.doc_viol_id;

					WITH q2 as(
					  SELECT id as doc_viol_id, reality_object_id FROM gji_warning_doc_violations as doc_viol
					  WHERE NOT EXISTS (
					    SELECT id FROM gkh_reality_object as ro 
					      WHERE doc_viol.reality_object_id = ro.id)
					  AND reality_object_id IS NOT NULL
					)
					UPDATE gji_warning_doc_violations as doc_viol
					SET reality_object_id = null
					FROM q2
					WHERE q2.doc_viol_id = doc_viol.id");

            this.Database.AddIndex("IND_GJI_WARNING_DOC_VIOLATIONS_RO", 
                false, 
                ViolationDocTableName,
                "REALITY_OBJECT_ID");
            
            this.Database.AddForeignKey("FK_GJI_WARNING_DOC_VIOLATIONS_RO", 
                ViolationDocTableName, 
                "REALITY_OBJECT_ID", 
                "GKH_REALITY_OBJECT", 
                "ID");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint(ViolationDocTableName, "FK_GJI_WARNING_DOC_VIOLATIONS_RO");
            this.Database.RemoveIndex(ViolationDocTableName,"IND_GJI_WARNING_DOC_VIOLATIONS_RO");
        }
    }
}