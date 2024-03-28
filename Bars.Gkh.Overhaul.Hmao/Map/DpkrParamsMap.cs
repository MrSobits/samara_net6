/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class DpkrParamsMap : BaseImportableEntityMap<DpkrParams>
///     {
///         public DpkrParamsMap()
///             : base("OVRHL_DPKR_PARAMS")
///         {
///             Map(x => x.Params, "PARAMS", true, 2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>Маппинг для "Параметры ДПКР"</summary>
    public class DpkrParamsMap : BaseImportableEntityMap<DpkrParams>
    {
        
        public DpkrParamsMap() : 
                base("Параметры ДПКР", "OVRHL_DPKR_PARAMS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Params, "Параметры (в виде JSON)").Column("PARAMS").Length(2000).NotNull();
        }
    }
}
