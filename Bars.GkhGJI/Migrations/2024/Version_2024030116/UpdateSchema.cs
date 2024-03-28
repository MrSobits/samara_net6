namespace Bars.GkhGji.Migrations._2024.Version_2024030116
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030116")]
    [MigrationDependsOn(typeof(Version_2024030115.UpdateSchema))]
    /// Является Version_2020110200 из ядра
    public class UpdateSchema : Migration
    {
        private const string InspectionViolStageTable = "GJI_INSPECTION_VIOL_STAGE";
        private const string ActViolTable = "GJI_ACTCHECK_VIOLAT";
        private const string ErpGuidColumn = "ERP_GUID";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(UpdateSchema.InspectionViolStageTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            var tranferActViolGuidsQuery = $@"
                    update public.GJI_INSPECTION_VIOL_STAGE stage set erp_guid = viol.erp_guid
                    from public.{UpdateSchema.ActViolTable} viol
                    where viol.id = stage.id;";

            this.Database.ExecuteNonQuery(tranferActViolGuidsQuery);
            this.Database.RemoveColumn(UpdateSchema.ActViolTable, UpdateSchema.ErpGuidColumn);

            if (!this.Database.TableExists(new SchemaQualifiedObjectName
            {
                Name = "gji_tat_disposal",
                Schema = "public"
            }))
            {
                return;
            }

            var transferPrescViolQuery = $@"
                --список успешно выполненных задач
                drop table if exists tmp_complete_tasks;
                create temp table tmp_complete_tasks as
                  select disposal_id, max(end_time) ""end_time"" from public.gi_task
                  where task_state = 80
                  group by disposal_id;
                
                            drop table if exists tmp_presc;
                            create temp table tmp_presc as
                              select stage.id ""stage_id"", presc.id ""presc_id"", doc.erp_guid from public.GJI_INSPECTION_VIOL_STAGE stage
                    join public.gji_prescription presc on stage.document_id = presc.id
                    join public.gji_document doc on presc.id = doc.id and doc.erp_guid is not null
                    join public.gji_document_children doc_ch on doc_ch.children_id = doc.id
                    join public.gji_document_children doc_ch2 on doc_ch2.children_id = doc_ch.parent_id
                    join public.gji_tat_disposal disposal on doc_ch2.parent_id = disposal.id
                    join tmp_complete_tasks task on task.disposal_id = disposal.id
                  where disposal.erp_id is not null and task.end_time > stage.object_create_date;
                
                        update public.GJI_INSPECTION_VIOL_STAGE stage set erp_guid = presc.erp_guid
                from tmp_presc presc
                where presc.stage_id = stage.id;
                
                update public.gji_document doc set erp_guid = null
                where exists(select 1 from tmp_presc t1 where t1.presc_id = doc.id);";

            this.Database.ExecuteNonQuery(transferPrescViolQuery);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.AddColumn(UpdateSchema.ActViolTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.RemoveColumn(UpdateSchema.InspectionViolStageTable, UpdateSchema.ErpGuidColumn);
        }
    }
}