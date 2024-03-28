/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class QualityPriorityParamMap : BaseEntityMap<QualityPriorityParam>
///     {
///         public QualityPriorityParamMap() : base("OVRHL_PRIOR_PARAM_QUALITY")
///         {
///             Map(x => x.Code, "CODE").Length(100);
///             Map(x => x.Value, "TYPE_PRESENCE");
///             Map(x => x.Point, "POINT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.QualityPriorityParam"</summary>
    public class QualityPriorityParamMap : BaseEntityMap<QualityPriorityParam>
    {
        
        public QualityPriorityParamMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.QualityPriorityParam", "OVRHL_PRIOR_PARAM_QUALITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(100);
            Property(x => x.Value, "Value").Column("TYPE_PRESENCE");
            Property(x => x.Point, "Point").Column("POINT");
        }
    }
}
