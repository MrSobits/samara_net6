namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Жилой дом договора управляющей организации"</summary>
    public class ManOrgContractRealityObjectMap : BaseImportableEntityMap<ManOrgContractRealityObject>
    {
        
        public ManOrgContractRealityObjectMap() : 
                base("Жилой дом договора управляющей организации", "GKH_MORG_CONTRACT_REALOBJ")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").NotNull().Fetch();
            this.Reference(x => x.ManOrgContract, "Договор управления").Column("MAN_ORG_CONTRACT_ID").NotNull().Fetch();
        }
    }
}
