namespace Bars.GkhCr.Map.ControlDate
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;

    public class ControlDateMunicipalityLimitDateMap : BaseEntityMap<ControlDateMunicipalityLimitDate>
    {
        /// <inheritdoc />
        public ControlDateMunicipalityLimitDateMap() 
            : base(typeof(ControlDateMunicipalityLimitDate).FullName, "CR_CTRL_DATE_MUNICIPALITY_LIMIT_DATE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ControlDate, nameof(ControlDateMunicipalityLimitDate.ControlDate))
                .Column("CONTROL_DATE_ID").NotNull().Fetch();
            this.Reference(x => x.Municipality, nameof(ControlDateMunicipalityLimitDate.Municipality))
                .Column("MUNICIPALITY_ID").NotNull().Fetch();
            this.Property(x => x.LimitDate, nameof(ControlDateMunicipalityLimitDate.LimitDate))
                .Column("LIMIT_DATE").NotNull();
        }
    }
}
