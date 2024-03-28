namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class VisitSheetInfoMap: BaseEntityMap<VisitSheetInfo>
    {
        /// <inheritdoc />
        public VisitSheetInfoMap()
            : base(nameof(VisitSheetInfo), "GJI_VISIT_SHEET_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.VisitSheet, "VisitSheet").Column("VISIT_SHEET_ID");
            this.Property(x => x.Info, "Info").Column("INFO");
            this.Property(x => x.Comment, "Comment").Column("COMMENT");
        }
    }
}
