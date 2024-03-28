namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.RegOp.Entities.Bills.RisAddressInfo
    /// </summary>
    public class RisAddressInfoMap : BaseRisEntityMap<RisAddressInfo>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.Gkh.Ris.Entities.Bills.RisAddressInfo
        /// </summary>
        public RisAddressInfoMap() :
            base("Bars.GisIntegration.RegOp.Entities.Bills.RisAddressInfo", "RIS_ADDRESS_INFO")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.LivingPersonsNumber, "LivingPersonsNumber").Column("LIVING_PERSON_NUMBER");
            this.Property(x => x.ResidentialSquare, "ResidentialSquare").Column("RESIDENTIAL_SQUARE");
            this.Property(x => x.HeatedArea, "HeatedArea").Column("HEATED_AREA");
            this.Property(x => x.TotalSquare, "TotalSquare").Column("TOTAL_SQUARE");
        }
    }
}
