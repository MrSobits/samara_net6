namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;

    public class RoomAreaVersionMap : VersionedEntity<Room>
    {
        public RoomAreaVersionMap() : base(VersionedParameters.RoomArea)
        {
            Map(x => x.Area, null, "Общая площадь квартиры");
        }
    }
}