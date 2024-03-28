namespace Bars.Gkh.Modules.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>Маппинг для Базовый класс - "Сторона договора"</summary>
    public class BaseContractPartyMap : BaseImportableEntityMap<BaseContractPart>
    {
        
        public BaseContractPartyMap() : 
                base("Базовый класс - \"Сторона договора\"", "GKH_RSOCONTRACT_BASE_PARTY")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.TypeContractPart, "Вид договора (стороны договора)").Column("TYPE_CONTRACT").NotNull();
            this.Reference(x => x.PublicServiceOrgContract, "Договор поставщика ресурсов с домом - с другой стороны").Column("SERV_ORG_CONTRACT_ID").NotNull().Fetch();
        }
    }
}
