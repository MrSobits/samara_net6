namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022031800
{
	using System;
	using System.Text;

	using Bars.B4.Modules.Ecm7.Framework;
	using Bars.B4.Utils;
	using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;

    [Migration("2022031800")]
    [MigrationDependsOn(typeof(Version_2022031500.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2022031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiDecisionTable =
            new SchemaQualifiedObjectName { Name = "GJI_DECISION", Schema = "PUBLIC"};

        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.gjiDecisionTable, 
                "GJI_TAT_DISPOSAL", "GJI_DECISION_TAT_DISPOSAL");
            this.Database.ExecuteNonQuery(this.GetSqlUpdateToDocType(TypeDocumentGji.Decision));
        }

        public override void Down()
        {
	        this.Database.RemoveTable(this.gjiDecisionTable);
	        this.Database.ExecuteNonQuery(this.GetSqlUpdateToDocType(TypeDocumentGji.Disposal));
        }

        private string GetSqlUpdateToDocType(TypeDocumentGji newTypeDocumentGji)
        {
	        var tmpTable = $@"tmp_table_{DateTime.Now.Ticks}";
	        var newEntityName = string.Empty;
	        var oldEntityName = string.Empty;

	        TypeStage inspTypeStage = default;
	        TypeStage inspPrescriptionTypeStage = default;

	        switch (newTypeDocumentGji)
	        {
		        case TypeDocumentGji.Decision:
			        newEntityName = "Decision";
			        oldEntityName = "TatDisposal";
			        inspTypeStage = TypeStage.Decision;
			        inspPrescriptionTypeStage = TypeStage.DecisionPrescription;
			        break;
		        case TypeDocumentGji.Disposal:
			        newEntityName = "TatDisposal";
			        oldEntityName = "Decision";
			        inspTypeStage = TypeStage.Disposal;
			        inspPrescriptionTypeStage = TypeStage.DisposalPrescription;
			        break;
	        }

	        var replaceTemplateUpdateQuery = new StringBuilder();
	        new[]
	        {
		        "Documentary",
		        "Exit",
		        "InspectionVisit"
	        }.ForEach(x =>
	        {
		        replaceTemplateUpdateQuery.Append($@"
					UPDATE gkh_template_replacement SET code = '{newEntityName + x}'
					WHERE code = '{oldEntityName + x}';");
	        });

            return $@"
                DROP TABLE IF EXISTS {tmpTable};
				CREATE TEMP TABLE {tmpTable} AS
				SELECT
					gd.id,
					gd.stage_id,
					CASE
						WHEN parent.type_document ISNULL
						THEN {(int)inspTypeStage} ELSE {(int)inspPrescriptionTypeStage}
					END AS insp_type_stage
				FROM
					gji_tat_disposal gtd
				JOIN gji_dict_control_types gdct ON
					gdct.id = gtd.control_type_id
				JOIN gji_document gd ON
					gd.id = gtd.id
				JOIN gji_inspection gi ON
					gi.id = gd.inspection_id
					AND (gi.check_date > '2021-12-31' 
							AND gdct.name = 'Государственный жилищный надзор'
						OR gi.check_date >= '2022-03-01' 
							AND gdct.name = 'Лицензионный контроль в отношении юридических лиц или индивидуальных предпринимателей, осуществляющих деятельность по управлению многоквартирными домами на основании лицензии')
				LEFT JOIN gji_document_children gdc ON
					gdc.children_id = gd.id
				LEFT JOIN gji_document parent ON
					parent.id = gdc.parent_id
					AND parent.type_document = {(int)TypeDocumentGji.Prescription};

				UPDATE gji_document gd SET type_document = {(int)newTypeDocumentGji}
				FROM {tmpTable} tt WHERE tt.id = gd.id;

				UPDATE gji_inspection_stage gis SET type_stage = tt.insp_type_stage
				FROM {tmpTable} tt WHERE tt.stage_id = gis.id;

				{(newTypeDocumentGji == TypeDocumentGji.Decision
					? $@"
				INSERT INTO {this.gjiDecisionTable.Name}
				SELECT tt.id FROM {tmpTable} tt;" : null)}

				{replaceTemplateUpdateQuery}";
        }
    }
}