namespace Bars.GkhCr.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict;

    public class WorksElementOutdoorMap : BaseEntityMap<WorksElementOutdoor>
    {
        public WorksElementOutdoorMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.Dict.WorksElementOutdoor", "CR_WORK_ELEMENT_OUTDOOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ElementOutdoor, nameof(WorksElementOutdoor.ElementOutdoor)).Column("ELEMENT_OUTDOOR_ID").NotNull().Fetch();
            this.Reference(x => x.Work, nameof(WorksElementOutdoor.Work)).Column("WORK_OUTDOOR_ID").NotNull().Fetch();
        }
    }
}