namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using B4.Modules.Mapping.Mappers;
    using Controller;

    public class TomskProtocolMap : JoinedSubClassMap<TomskProtocol>
    {
        public TomskProtocolMap() : base("Bars.GkhGji.Regions.Tomsk.Entities.TomskProtocol", "GJI_TOMSK_PROTOCOL")
        {
        }

        protected override void Map()
        {
            Property(x => x.DateOfViolation, "DateOfViolation").Column("DATE_OF_VIOLATION");
            Property(x => x.HourOfViolation, "HourOfViolation").Column("HOUR_OF_VIOLATION");
            Property(x => x.MinuteOfViolation, "MinuteOfViolation").Column("MINUTE_OF_VIOLATION");
        }
    }
}