namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class VisitSheetMap: GkhJoinedSubClassMap<VisitSheet>
    {
        /// <inheritdoc />
        public VisitSheetMap()
            : base("GJI_VISIT_SHEET")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ExecutingInspector, "ExecutingInspector").Column("EXECUTING_INSPECTOR_ID");
            this.Property(x => x.HasCopy, "HasCopy").Column("HAS_COPY");
            this.Property(x => x.VisitDateStart, "VisitDateStart").Column("VISIT_DATE_START");
            this.Property(x => x.VisitDateEnd, "VisitDateEnd").Column("VISIT_DATE_END");
            this.Property(x => x.VisitTimeStart, "VisitTimeStart").Column("VISIT_TIME_START");
            this.Property(x => x.VisitTimeEnd, "VisitTimeEnd").Column("VISIT_TIME_END");
        }
    }
}
