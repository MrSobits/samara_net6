namespace Bars.Gkh.Map.Contragent
{
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Муниципальные образования контрагенты"</summary>
    public class ContragentMunicipalityMap : BaseImportableEntityMap<ContragentMunicipality>
    {
        
        public ContragentMunicipalityMap() : 
                base("Муниципальные образования контрагенты", "GKH_CONTRAGENT_MUNICIPALITY")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
