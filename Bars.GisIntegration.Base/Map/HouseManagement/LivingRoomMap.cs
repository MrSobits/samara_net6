namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using Bars.GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.RegOp.Entities.HouseManagement.LivingRoom"
    /// </summary>
    public class LivingRoomMap : BaseRisEntityMap<LivingRoom>
    {
        public LivingRoomMap() :
            base("Bars.GisIntegration.RegOp.Entities.HouseManagement.LivingRoom", "RIS_LIVINGROOM")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ResidentialPremises, "ResidentialPremises").Column("RES_PREMISES_ID").Fetch();
            this.Reference(x => x.House, "House").Column("HOUSE_ID").Fetch();
            this.Reference(x => x.Block, "Block").Column("BLOCK_ID").Fetch();
            this.Property(x => x.RoomNumber, "RoomNumber").Column("ROOMNUMBER").Length(50);
            this.Property(x => x.Square, "Square").Column("SQUARE");
            this.Property(x => x.TerminationDate, "TerminationDate").Column("TERMINATIONDATE");
            this.Property(x => x.CadastralNumber, "CadastralNumber").Column("CADASTRALNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberCadastralNumber, "PrevStateRegNumberCadastralNumber").Column("PREVSTATEREGNUMBER_CADASTRALNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberInventoryNumber, "PrevStateRegNumberInventoryNumber").Column("PREVSTATEREGNUMBER_INVENTORYNUMBER").Length(50);
            this.Property(x => x.PrevStateRegNumberConditionalNumber, "PrevStateRegNumberConditionalNumber").Column("PREVSTATEREGNUMBER_CONDITIONALNUMBER").Length(50);
            this.Property(x => x.Floor, "Floor").Column("FLOOR").Length(50);
        }
    }
}