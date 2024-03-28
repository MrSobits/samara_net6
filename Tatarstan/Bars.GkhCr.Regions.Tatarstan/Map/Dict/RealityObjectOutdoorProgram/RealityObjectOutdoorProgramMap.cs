namespace Bars.GkhCr.Regions.Tatarstan.Map.Dict.RealityObjectOutdoorProgram
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    public class RealityObjectOutdoorProgramMap : BaseEntityMap<RealityObjectOutdoorProgram>
    {
        /// <inheritdoc />
        public RealityObjectOutdoorProgramMap()
            : base("Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram.RealityObjectOutdoorProgram", "CR_DICT_PROGRAM_OUTDOOR")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, nameof(RealityObjectOutdoorProgram.Name)).Column("NAME").NotNull().Length(255);
            this.Property(x => x.Code, nameof(RealityObjectOutdoorProgram.Code)).Column("CODE").Length(255);
            this.Property(x => x.TypeVisibilityProgram, nameof(RealityObjectOutdoorProgram.TypeVisibilityProgram)).Column("TYPE_VISIBILITY").NotNull();
            this.Property(x => x.TypeProgramState, nameof(RealityObjectOutdoorProgram.TypeProgramState)).Column("TYPE_PROGRAM_STATE").NotNull();
            this.Property(x => x.TypeProgram, nameof(RealityObjectOutdoorProgram.TypeProgram)).Column("TYPE_PROGRAM").NotNull();
            this.Property(x => x.DocumentNumber, nameof(RealityObjectOutdoorProgram.DocumentNumber)).Column("DOC_NUMBER").NotNull().Length(255);
            this.Property(x => x.DocumentDate, nameof(RealityObjectOutdoorProgram.DocumentDate)).Column("DOC_DATE");
            this.Property(x => x.DocumentDepartment, nameof(RealityObjectOutdoorProgram.DocumentDepartment)).Column("DOCUMENT_DEPARTMENT").Length(255);
            this.Property(x => x.IsNotAddOutdoor, nameof(RealityObjectOutdoorProgram.IsNotAddOutdoor)).Column("IS_NOT_ADD_OUTDOOR");
            this.Property(x => x.Description, nameof(RealityObjectOutdoorProgram.Description)).Column("DESCRIPTION").Length(255);

            this.Reference(x => x.GovernmentCustomer, nameof(RealityObjectOutdoorProgram.GovernmentCustomer)).Column("GOV_CUSTOMER_ID").Fetch();
            this.Reference(x => x.Period, nameof(RealityObjectOutdoorProgram.Period)).Column("PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.NormativeDoc, nameof(RealityObjectOutdoorProgram.NormativeDoc)).Column("NORMATIVE_DOC_ID").NotNull().Fetch();
            this.Reference(x => x.File, nameof(RealityObjectOutdoorProgram.File)).Column("FILE_ID").Fetch();
        }
    }
}
