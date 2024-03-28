/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class DpkrParamsMap : BaseEntityMap<DpkrParams>
///     {
///         public DpkrParamsMap()
///             : base("OVRHL_DPKR_PARAMS")
///         {
///             Map(x => x.Params, "PARAMS", true, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.DpkrParams"</summary>
    public class DpkrParamsMap : BaseEntityMap<DpkrParams>
    {
        
        public DpkrParamsMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.DpkrParams", "OVRHL_DPKR_PARAMS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Params, "Params").Column("PARAMS").Length(2000).NotNull();
        }
    }
}
