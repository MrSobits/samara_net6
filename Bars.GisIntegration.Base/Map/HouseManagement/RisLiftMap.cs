namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.Gkh.Ris.Entities.HouseManagement.RisHouse"
    /// </summary>
    public class RisLiftMap : BaseRisEntityMap<RisLift>
    {
        public RisLiftMap() :
            base("Bars.Gkh.Ris.Entities.HouseManagement.RisLift", "RIS_LIFT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ApartmentHouse, "ApartmentHouse").Column("APARTAMENT_HOUSE");
            this.Property(x => x.EntranceNum, "EntranceNum").Column("ENTRANCE_NUM");
            this.Property(x => x.FactoryNum, "FactoryNum").Column("FACTORY_NUM");
            this.Property(x => x.OperatingLimit, "OperatingLimit").Column("OPERATING_LIMIT");
            this.Property(x => x.TerminationDate, "TerminationDate").Column("TERMINATION_DATE");
            this.Property(x => x.OgfDataCode, "OgfDataCode").Column("OGF_DATA_CODE");
            this.Property(x => x.OgfDataValue, "OgfDataValue").Column("OGF_DATA_VALUE");
            this.Property(x => x.TypeCode, "TypeCode").Column("TYPE_CODE");
            this.Property(x => x.TypeGuid, "TypeGuid").Column("TYPE_GUID");
        }
    }
}