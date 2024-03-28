namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedMap : GkhJoinedSubClassMap<TaskActionIsolated>
    {
        public TaskActionIsolatedMap()
            : base("GJI_TASK_ACTIONISOLATED")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID");
            this.Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID");
            this.Reference(x => x.PlanAction, "PlanAction").Column("PLAN_ACTION_ID");
            this.Reference(x => x.AppealCits, "AppealCits").Column("APPEAL_CITS_ID");
            this.Reference(x => x.IssuedTask, "IssuedTask").Column("ISSUED_TASK_ID");
            this.Reference(x => x.ResponsibleExecution, "ResponsibleExecution").Column("RESPONSIBLE_EXECUTION_ID");
            this.Reference(x => x.ControlType, "ControlType").Column("CONTROL_TYPE_ID");
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column("ZONAL_INSPECTION_ID");
            this.Property(x => x.Inn, "Inn").Column("INN");
            this.Property(x => x.PersonName, "PersonName").Column("PERSON_NAME");
            this.Property(x => x.KindAction, "KindKnm").Column("KIND_ACTION");
            this.Property(x => x.TypeBase, "TypeBase").Column("TYPE_BASE");
            this.Property(x => x.TypeObject, "TypeObject").Column("TYPE_OBJECT");
            this.Property(x => x.TypeJurPerson, "TypeJurPerson").Column("TYPE_JUR_PERSON");
            this.Property(x => x.DateStart, "DateStart").Column("DATE_START");
            this.Property(x => x.TimeStart, "TimeStart").Column("TIME_START");
            this.Property(x => x.BaseDocumentName, "BaseDocumentName").Column("BASE_DOC_NAME");
            this.Property(x => x.BaseDocumentNumber, "BaseDocumentNumber").Column("BASE_DOC_NUM");
            this.Property(x => x.BaseDocumentDate, "BaseDocumentDate").Column("BASE_DOC_DATE");
            this.Reference(x => x.BaseDocumentFile, "BaseDocumentFile").Column("BASE_DOC_FILE_ID");

        }
    }
}
