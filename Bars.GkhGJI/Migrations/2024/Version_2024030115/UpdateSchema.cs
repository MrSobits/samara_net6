namespace Bars.GkhGji.Migrations._2024.Version_2024030115
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030115")]
    [MigrationDependsOn(typeof(Version_2024030114.UpdateSchema))]
    /// Является Version_2020093000 из ядра
    public class UpdateSchema : Migration
    {
        private const string GjiDocTable = "GJI_DOCUMENT";
        private const string ActCheckWitnessTable = "GJI_ACTCHECK_WITNESS";
        private const string ActCheckViolationTable = "GJI_ACTCHECK_VIOLAT";
        private const string ActCheckRealityObjectTable = "GJI_ACTCHECK_ROBJECT";
        private const string InspectionGjiRealityObjectTable = "GJI_INSPECTION_ROBJECT";
        private const string InspFoundCheckTable = "GJI_NSO_DISPOSAL_INSPFOUNDCHECK";
        private const string PrescriptionTable = "gji_prescription";
        private const string ViolationTable = "GJI_DICT_VIOLATION";
        private const string RealityObjectTable = "GKH_REALITY_OBJECT";
        private const string TatarstanDisposalTable = "gji_tat_disposal";

        private const string ErpGuidColumn = "ERP_GUID";

        /// <inheritdoc />
        public override void Up()
        {
            this.AddErpGuidColumns();
            this.UpdateErpGuids();

            this.Database.RemoveColumn(UpdateSchema.TatarstanDisposalTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.RealityObjectTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.ViolationTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.PrescriptionTable, UpdateSchema.ErpGuidColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.RemoveErpGuidColumns();
            this.Database.AddColumn(UpdateSchema.PrescriptionTable, new Column(UpdateSchema.ErpGuidColumn, DbType.String));
            this.Database.AddColumn(UpdateSchema.RealityObjectTable, new Column(UpdateSchema.ErpGuidColumn, DbType.String));
            this.Database.AddColumn(UpdateSchema.ViolationTable, new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            if (this.Database.TableExists(UpdateSchema.TatarstanDisposalTable))
            {
                this.Database.AddColumn(UpdateSchema.TatarstanDisposalTable,
                    new Column(UpdateSchema.ErpGuidColumn, DbType.String));
            }
        }

        private void AddErpGuidColumns()
        {
            this.Database.AddColumn(UpdateSchema.GjiDocTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.AddColumn(UpdateSchema.ActCheckWitnessTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.AddColumn(UpdateSchema.ActCheckRealityObjectTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.AddColumn(UpdateSchema.ActCheckViolationTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.AddColumn(UpdateSchema.InspectionGjiRealityObjectTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));

            this.Database.AddColumn(UpdateSchema.InspFoundCheckTable,
                new Column(UpdateSchema.ErpGuidColumn, DbType.String));
        }

        private void RemoveErpGuidColumns()
        {
            this.Database.RemoveColumn(UpdateSchema.GjiDocTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.ActCheckWitnessTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.ActCheckRealityObjectTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.ActCheckViolationTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.InspectionGjiRealityObjectTable, UpdateSchema.ErpGuidColumn);
            this.Database.RemoveColumn(UpdateSchema.InspFoundCheckTable, UpdateSchema.ErpGuidColumn);
        }

        private void UpdateErpGuids()
        {
            var updateGuidQuery = $@"
                    update public.gji_document doc set erp_guid = presc.erp_guid
                    from public.{UpdateSchema.PrescriptionTable} presc
                    where doc.id = presc.id;";

            this.Database.ExecuteNonQuery(updateGuidQuery);

            if (!this.Database.TableExists(UpdateSchema.TatarstanDisposalTable))
            {
                return;
            }

            var updateInspectionRoQuery = $@"--обновление ерп гуидов проверяемых домов
                    --список успешно выполненных задач
                drop table if exists tmp_complete_tasks;
                create temp table tmp_complete_tasks as
                  select disposal_id, max(end_time) ""end_time"" from public.gi_task
                            where task_state = 80
                            group by disposal_id;
                
                   drop table if exists tmp_table1;
                   create temp table tmp_table1 as
                     select insp_ro.id, ro.erp_guid
                     from public.{UpdateSchema.InspectionGjiRealityObjectTable} insp_ro
                        join public.{UpdateSchema.GjiDocTable} doc on doc.inspection_id = insp_ro.inspection_id
                        join public.{UpdateSchema.TatarstanDisposalTable} disposal on doc.id = disposal.id
                        join public.{UpdateSchema.RealityObjectTable} ro on insp_ro.reality_object_id = ro.id
                        join tmp_complete_tasks task on task.disposal_id = disposal.id
                            where disposal.registration_number_erp is not null
                            and disposal.erp_guid is not null
                            and insp_ro.object_create_date < task.end_time;


                    update {UpdateSchema.InspectionGjiRealityObjectTable} insp_ro set erp_guid = t1.erp_guid
                    from tmp_table1 t1
                    where t1.id = insp_ro.id;";

            this.Database.ExecuteNonQuery(updateInspectionRoQuery);

            var updateViolQuery = $@"--обновление ерп гуидов нарушений
                        drop table if exists tmp_table2;
                            create temp table tmp_table2 as
                            select act_viol.id, viol.erp_guid
                            from public.{UpdateSchema.ActCheckViolationTable} act_viol
                              --нарушение
                              join public.GJI_INSPECTION_VIOL_STAGE insp_viol_stage on act_viol.id = insp_viol_stage.id
                              join public.GJI_INSPECTION_VIOLATION insp_viol on insp_viol_stage.inspection_viol_id = insp_viol.id
                              join public.{UpdateSchema.ViolationTable} viol on insp_viol.violation_id = viol.id
                              --распоряжение
                              join public.GJI_ACTCHECK_ROBJECT act_ro on act_viol.actcheck_robject_id = act_ro.id
                              join public.GJI_DOCUMENT_CHILDREN doc_child on act_ro.actcheck_id = doc_child.children_id
                              join public.{UpdateSchema.TatarstanDisposalTable} disposal on doc_child.parent_id = disposal.id
                                join tmp_complete_tasks task on task.disposal_id = disposal.id
                            where disposal.registration_number_erp is not null
                                and disposal.erp_guid is not null
                                and insp_viol_stage.object_create_date < task.end_time;

                            update {UpdateSchema.ActCheckViolationTable} act_viol set erp_guid = t2.erp_guid
                            from tmp_table2 t2
                            where t2.id = act_viol.id;";

            this.Database.ExecuteNonQuery(updateViolQuery);

            var updateDisposalGuidQuery = $@"update public.gji_document doc set erp_guid = disp.erp_guid
                    from public.{UpdateSchema.TatarstanDisposalTable} disp
                    where doc.id = disp.id;";

            this.Database.ExecuteNonQuery(updateDisposalGuidQuery);
        }
    }
}