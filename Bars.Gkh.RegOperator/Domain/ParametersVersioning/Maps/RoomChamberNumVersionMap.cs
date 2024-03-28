namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг номер комнаты
    /// </summary>
    public class RoomChamberNumVersionMap : VersionedEntity<Room>
    {
        /// <summary>
        /// Маппинг номер комнаты
        /// </summary>
        public RoomChamberNumVersionMap() : base(VersionedParameters.RoomChamberNum)
        {
            this.Map(x => x.ChamberNum, null, "Номер комнаты");
        }
    }
}