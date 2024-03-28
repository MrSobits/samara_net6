/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Dict
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.RegOperator.Entities.Dict;
/// 
///     public class PrivilegedCategoryMap : BaseImportableEntityMap<PrivilegedCategory>
///     {
///         public PrivilegedCategoryMap() : base("REGOP_PRIVILEGED_CATEGORY")
///         {
///             Map(x => x.Code, "CODE");
///             Map(x => x.Name, "NAME");
///             Map(x => x.Percent, "PERCENT");
///             Map(x => x.DateFrom, "DATE_FROM");
///             Map(x => x.DateTo, "DATE_TO"); 
///             Map(x => x.LimitArea, "LIMIT_AREA");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;
    
    
    /// <summary>Маппинг для "Справочник "Группы льготных категорий граждан""</summary>
    public class PrivilegedCategoryMap : BaseImportableEntityMap<PrivilegedCategory>
    {
        
        public PrivilegedCategoryMap() : 
                base("Справочник \"Группы льготных категорий граждан\"", "REGOP_PRIVILEGED_CATEGORY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
            Property(x => x.Percent, "Процент льготы").Column("PERCENT");
            Property(x => x.DateFrom, "Действует с").Column("DATE_FROM");
            Property(x => x.DateTo, "Действует по").Column("DATE_TO");
            Property(x => x.LimitArea, "Предельное значение площади").Column("LIMIT_AREA");
        }
    }
}
