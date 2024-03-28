namespace Bars.Gkh.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public class WorkRealityObjectOutdoorMap : BaseEntityMap<WorkRealityObjectOutdoor>
    {
        /// <inheritdoc />
        public WorkRealityObjectOutdoorMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.Dict.WorkRealityObjectOutdoor", "CR_DICT_WORK_OUTDOOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Code, nameof(WorkRealityObjectOutdoor.Code)).Column("CODE").Length(255);
            this.Property(x => x.Name, nameof(WorkRealityObjectOutdoor.Name)).Column("NAME").Length(255).NotNull();
            this.Property(x => x.TypeWork, nameof(WorkRealityObjectOutdoor.TypeWork)).Column("TYPE_WORK").Length(255).NotNull();
            this.Reference(x => x.UnitMeasure, nameof(WorkRealityObjectOutdoor.UnitMeasure)).Column("UNIT_MEASURE_ID").Fetch();
            this.Property(x => x.IsActual, nameof(WorkRealityObjectOutdoor.IsActual)).Column("IS_ACTUAL").NotNull();
            this.Property(x => x.Description, nameof(WorkRealityObjectOutdoor.Description)).Column("DESCRIPTION").Length(255);
        }
    }
}