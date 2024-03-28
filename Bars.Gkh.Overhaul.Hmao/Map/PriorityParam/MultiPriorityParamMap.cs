/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities;
/// 
///     public class MultiPriorityParamMap : BaseImportableEntityMap<MultiPriorityParam>
///     {
///         public MultiPriorityParamMap() : base("OVRHL_PRIOR_PARAM_MULTI")
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Множественные параметры"</summary>
    public class MultiPriorityParamMap : BaseImportableEntityMap<MultiPriorityParam>
    {
        
        public MultiPriorityParamMap() : 
                base("Множественные параметры", "OVRHL_PRIOR_PARAM_MULTI")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(100);
            Property(x => x.Value, "Значение").Column("VALUE").Length(300);
            Property(x => x.Point, "Балл").Column("POINT");
            Property(x => x.StoredValues, "Записи справочника").Column("STORED_VALUES");
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
