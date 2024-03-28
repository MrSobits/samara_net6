/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class MultiPriorityParamMap : BaseEntityMap<MultiPriorityParam>
///     {
///         public MultiPriorityParamMap()
///             : base("OVRHL_PRIOR_PARAM_MULTI")
///         {
///             Map(x => x.Code, "CODE", false, 100);
///             Map(x => x.Value, "VALUE", false, 300);
///             Map(x => x.Point, "POINT");
/// 
///             Property(x => x.StoredValues, x =>
///             {
///                 x.Column("STORED_VALUES");
///                 x.Type<ImprovedJsonSerializedType<List<StoredMultiValue>>>();
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.MultiPriorityParam"</summary>
    public class MultiPriorityParamMap : BaseEntityMap<MultiPriorityParam>
    {
        
        public MultiPriorityParamMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.MultiPriorityParam", "OVRHL_PRIOR_PARAM_MULTI")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").Length(100);
            Property(x => x.Value, "Value").Column("VALUE").Length(300);
            Property(x => x.Point, "Point").Column("POINT");
            Property(x => x.StoredValues, "StoredValues").Column("STORED_VALUES");
        }
    }

    public class MultiPriorityParamNHibernateMapping : ClassMapping<MultiPriorityParam>
    {
        public MultiPriorityParamNHibernateMapping()
        {
            Property(
                x => x.StoredValues,
                x =>
                    {
                        x.Type<ImprovedJsonSerializedType<List<StoredMultiValue>>>();
                    });
        }
    }
}
