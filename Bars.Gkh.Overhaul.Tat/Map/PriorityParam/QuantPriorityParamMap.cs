/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class QuantPriorityParamMap : BaseEntityMap<QuantPriorityParam>
///     {
///         public QuantPriorityParamMap() : base("OVRHL_PRIOR_PARAM_QUANT")
///         {
///             Map(x => x.Code, "CODE").Length(100);
///             Map(x => x.MaxValue, "MAX_VALUE").Length(100);
///             Map(x => x.MinValue, "MIN_VALUE").Length(100);
///             Map(x => x.Point, "POINT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.QuantPriorityParam"</summary>
    public class QuantPriorityParamMap : BaseEntityMap<QuantPriorityParam>
    {
        
        public QuantPriorityParamMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.QuantPriorityParam", "OVRHL_PRIOR_PARAM_QUANT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(100);
            Property(x => x.MaxValue, "MaxValue").Column("MAX_VALUE").Length(100);
            Property(x => x.MinValue, "MinValue").Column("MIN_VALUE").Length(100);
            Property(x => x.Point, "Point").Column("POINT");
        }
    }
}
