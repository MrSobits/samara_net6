namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    public class MunicipalityFiasOktmoMap : BaseImportableEntityMap<MunicipalityFiasOktmo>
    {
        public MunicipalityFiasOktmoMap() : 
                base("Справочник ФИАС ОКТМО", "GKH_MUNICIPALITY_FIAS_OKTMO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.FiasGuid, "FiasGuid").Column("FIAS_GUID").Length(36);
            Property(x => x.Oktmo, "Oktmo").Column("OKTMO");
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").Fetch();
        }
    }
}
