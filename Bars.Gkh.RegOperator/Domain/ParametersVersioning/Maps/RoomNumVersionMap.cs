namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;

    public class RoomNumVersionMap : VersionedEntity<Room>
    {
        public RoomNumVersionMap() : base(VersionedParameters.RoomNum)
        {
            Map(x => x.RoomNum, null, "Номер помещения");
        }
    }
}