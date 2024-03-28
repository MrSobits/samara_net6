namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    public class RisMeteringDeviceAccountMap : BaseRisEntityMap<RisMeteringDeviceAccount>
    {
        public RisMeteringDeviceAccountMap()
            : base("Bars.Gkh.Ris.Entities.HouseManagement.RisMeteringDeviceAccount", "RIS_METERING_DEVICE_ACCOUNT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Account, "Account").Column("ACCOUNT_ID").NotNull();
            this.Reference(x => x.MeteringDeviceData, "MeteringDeviceData").Column("METERING_DEVICE_ID").NotNull();
        }
    }
}
