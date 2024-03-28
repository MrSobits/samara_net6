namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.RegOp.Entities.HouseManagement.ResidentialPremises"
    /// </summary>
    public class ResidentialPremisesMap : BaseRisEntityMap<ResidentialPremises>
    {
        public ResidentialPremisesMap() :
            base("Bars.GisIntegration.RegOp.Entities.HouseManagement.ResidentialPremises", "RIS_RESIDENTIALPREMISES")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ApartmentHouse, "ApartmentHouse").Column("HOUSE_ID").Fetch();
            this.Property(x => x.PremisesNum, "PremisesNum").Column("PREMISESNUM").Length(50);
            this.Property(x => x.CadastralNumber, "CadastralNumber").Column("CADASTRALNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberCadastralNumber, "PrevStateRegNumberCadastralNumber")
                .Column("PREVSTATEREGNUMBER_CADASTRALNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberInventoryNumber, "PrevStateRegNumberInventoryNumber")
                .Column("PREVSTATEREGNUMBER_INVENTORYNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberConditionalNumber, "PrevStateRegNumberConditionalNumber")
                .Column("PREVSTATEREGNUMBER_CONDITIONALNUMBER").Length(50);
            this.Property(x => x.TerminationDate, "TerminationDate").Column("TERMINATIONDATE");
            this.Property(x => x.EntranceNum, "EntranceNum").Column("ENTRANCENUM");
            this.Property(x => x.PremisesCharacteristicCode, "PremisesCharacteristicCode").Column("PREMISESCHARACTERISTIC_CODE").Length(50);
            this.Property(x => x.PremisesCharacteristicGuid, "PremisesCharacteristicGuid").Column("PREMISESCHARACTERISTIC_GUID").Length(50);
            this.Property(x => x.RoomsNumCode, "RoomsNumCode").Column("ROOMSNUM_CODE").Length(50);
            this.Property(x => x.RoomsNumGuid, "RoomsNumGuid").Column("ROOMSNUM_GUID").Length(50);
            this.Property(x => x.ResidentialHouseTypeCode, "ResidentialHouseTypeCode").Column("RESIDENTIALHOUSETYPE_CODE").Length(50);
            this.Property(x => x.ResidentialHouseTypeGuid, "ResidentialHouseTypeGuid").Column("RESIDENTIALHOUSETYPE_GUID").Length(50);
            this.Property(x => x.GrossArea, "GrossArea").Column("GROSSAREA");
            this.Property(x => x.TotalArea, "TotalArea").Column("TOTALAREA");
            this.Property(x => x.Floor, "Floor").Column("FLOOR");
        }
    }
}