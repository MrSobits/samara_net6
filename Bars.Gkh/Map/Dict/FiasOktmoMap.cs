/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities.Dicts;
///     using Bars.B4.DataAccess;
/// 
///     public class FiasOktmoMap : BaseImportableEntityMap<FiasOktmo>
///     {
///         public FiasOktmoMap()
///             : base("GKH_DICT_FIASOKTMO")
///         {
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///             Map(x => x.FiasGuid, "FIAS_GUID").Length(36);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Entities.Dicts.FiasOktmo"</summary>
    public class FiasOktmoMap : BaseImportableEntityMap<FiasOktmo>
    {
        
        public FiasOktmoMap() : 
                base("Bars.Gkh.Entities.Dicts.FiasOktmo", "GKH_DICT_FIASOKTMO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.FiasGuid, "FiasGuid").Column("FIAS_GUID").Length(36);
            Reference(x => x.Municipality, "Municipality").Column("MUNICIPALITY_ID").Fetch();
        }
    }
}
