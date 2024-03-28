namespace Bars.GisIntegration.Base.Map.HouseManagement
{
    using GisIntegration.Base.Map;
    using Entities.HouseManagement;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.RegOp.Entities.HouseManagement.Block"
    /// </summary>
    public class BlockMap : BaseRisEntityMap<Block>
    {
        public BlockMap() :
            base("Bars.GisIntegration.RegOp.Entities.HouseManagement.Block", "RIS_BLOCK")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.House, "House").Column("HOUSE_ID").Fetch();
            this.Property(x => x.CadastralNumber, "CadastralNumber").Column("CADASTRALNUMBER");
            this.Property(x => x.BlockNum, "BlockNum").Column("BLOCKNUM");
            this.Property(x => x.PremisesCharacteristicCode, "PremisesCharacteristicCode").Column("PREMISESCHARACTERISTICCODE");
            this.Property(x => x.PremisesCharacteristicGuid, "PremisesCharacteristicGuid").Column("PREMISESCHARACTERISTICGUID");
            this.Property(x => x.TotalArea, "TotalArea").Column("TOTALAREA");
            this.Property(x => x.GrossArea, "GrossArea").Column("GROSSAREA");
            this.Property(x => x.TerminationDate, "TerminationDate").Column("TERMINATIONDATE");
        }
    }
}