/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class MultiPriorityParamMap : BaseEntityMap<MultiPriorityParam>
///     {
///         public MultiPriorityParamMap() : base("OVRHL_PRIOR_PARAM_MULTI")
///         {
///             Map(x => x.Code, "CODE").Length(100);
///             Map(x => x.Value, "VALUE").Length(300);
///             Map(x => x.Point, "POINT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.MultiPriorityParam"</summary>
    public class MultiPriorityParamMap : BaseEntityMap<MultiPriorityParam>
    {
        
        public MultiPriorityParamMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.MultiPriorityParam", "OVRHL_PRIOR_PARAM_MULTI")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(100);
            Property(x => x.Value, "Value").Column("VALUE").Length(300);
            Property(x => x.Point, "Point").Column("POINT");
        }
    }
}
