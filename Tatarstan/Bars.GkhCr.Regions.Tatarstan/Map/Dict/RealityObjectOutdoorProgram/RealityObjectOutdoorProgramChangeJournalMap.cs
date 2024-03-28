namespace Bars.GkhCr.Regions.Tatarstan.Map.Dict.RealityObjectOutdoorProgram
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    public class RealityObjectOutdoorProgramChangeJournalMap : BaseEntityMap<RealityObjectOutdoorProgramChangeJournal>
    {
        /// <inheritdoc />
        public RealityObjectOutdoorProgramChangeJournalMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram.RealityObjectOutdoorProgramChangeJournal", "CR_DICT_PROG_OUTDOOR_CHANGE_JOUR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.RealityObjectOutdoorProgram, nameof(RealityObjectOutdoorProgramChangeJournal.RealityObjectOutdoorProgram)).Column("PROGRAM_ID").NotNull().Fetch();
            this.Property(x => x.ChangeDate, nameof(RealityObjectOutdoorProgramChangeJournal.ChangeDate)).Column("CHANGE_DATE").NotNull();
            this.Property(x => x.MuCount, nameof(RealityObjectOutdoorProgramChangeJournal.MuCount)).Column("MU_COUNT");
            this.Property(x => x.UserName, nameof(RealityObjectOutdoorProgramChangeJournal.UserName)).Column("USER_NAME").Length(255);
            this.Property(x => x.Description, nameof(RealityObjectOutdoorProgramChangeJournal.Description)).Column("DESCRIPTION").Length(255);
        }
    }
}
