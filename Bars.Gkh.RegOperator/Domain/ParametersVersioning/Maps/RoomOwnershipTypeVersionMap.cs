namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;

    public class RoomOwnershipTypeVersionMap : VersionedEntity<Room>
    {
        public RoomOwnershipTypeVersionMap() : base(VersionedParameters.RoomOwnershipType)
        {
            Map(x => x.OwnershipType, null, "Тип собственности");
        }
    }
}