namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Enums;

    [Migration("2022042601")]
    [MigrationDependsOn(typeof(Version_2022042500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
	        var parentDocumentTypes = new[]
	        {
				(int)TypeDocumentGji.TaskActionIsolated,
				(int)TypeDocumentGji.PreventiveAction
	        };
	        var childDocumentTypes = new[]
	        {
		        (int)TypeDocumentGji.ActActionIsolated,
		        (int)TypeDocumentGji.VisitSheet,
		        (int)TypeDocumentGji.PreventiveActionTask,
		        (int)TypeDocumentGji.MotivatedPresentation
	        };
	        
	        this.Database.ExecuteNonQuery($@"
			WITH document_to_update AS 
			(
				SELECT gd1.id, gd2.document_number, gd2.document_num 
				FROM   gji_document gd1
				JOIN   gji_document gd2 
					   ON  gd1.inspection_id = gd2.inspection_id 
				       AND gd2.document_number ~ '\w{{2}}-\d+'
					   AND gd2.type_document IN ({string.Join(",", parentDocumentTypes)})
				WHERE  gd1.document_number <> gd2.document_number
					   AND gd1.type_document IN ({string.Join(",", childDocumentTypes)})
					   AND COALESCE(gd1.document_number, '') <> ''
			)
			UPDATE gji_document gd
			SET    document_number = dtu.document_number,
				   document_num = dtu.document_num
			FROM   document_to_update dtu
			WHERE  gd.id = dtu.id");
        }
    }
}
