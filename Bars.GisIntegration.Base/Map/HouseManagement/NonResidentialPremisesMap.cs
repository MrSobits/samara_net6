namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.RegOp.Entities.HouseManagement.NonResidentialPremises"
    /// </summary>
    public class NonResidentialPremisesMap : BaseRisEntityMap<NonResidentialPremises>
    {
        public NonResidentialPremisesMap() :
            base("Bars.GisIntegration.RegOp.Entities.HouseManagement.NonResidentialPremises", "RIS_NONRESIDENTIALPREMISES")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ApartmentHouse, "ApartmentHouse").Column("HOUSE_ID").Fetch();
            this.Property(x => x.PremisesNum, "PremisesNum").Column("PREMISESNUM").Length(50);
            this.Property(x => x.CadastralNumber, "CadastralNumber").Column("CADASTRALNUMBER").Length(50);
            this.Property(x => x.Floor, "Floor").Column("FLOOR").Length(50);
            this.Property(x => x.PrevStateRegNumberCadastralNumber, "PrevStateRegNumberCadastralNumber")
                .Column("PREVSTATEREGNUMBER_CADASTRALNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberInventoryNumber, "PrevStateRegNumberInventoryNumber")
                .Column("PREVSTATEREGNUMBER_INVENTORYNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberConditionalNumber, "PrevStateRegNumberConditionalNumber")
                .Column("PREVSTATEREGNUMBER_CONDITIONALNUMBER").Length(50);
            this.Property(x => x.TerminationDate, "TerminationDate").Column("TERMINATIONDATE");
            this.Property(x => x.PurposeCode, "PurposeCode").Column("PURPOSE_CODE").Length(50);
            this.Property(x => x.PurposeGuid, "PurposeGuid").Column("PURPOSE_GUID").Length(50);
            this.Property(x => x.PositionCode, "PositionCode").Column("POSITION_CODE").Length(50);
            this.Property(x => x.PositionGuid, "PositionGuid").Column("POSITION_GUID").Length(50);
            this.Property(x => x.GrossArea, "GrossArea").Column("GROSSAREA");
            this.Property(x => x.TotalArea, "TotalArea").Column("TOTALAREA");
            this.Property(x => x.IsCommonProperty, "IsCommonProperty").Column("ISCOMMONPROPERTY");
        }
    }
}