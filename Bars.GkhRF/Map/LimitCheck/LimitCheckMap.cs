/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using B4.DataAccess;
/// 
///     using Enums;
///     using Entities;
/// 
///     public class LimitCheckMap : BaseEntityMap<LimitCheck>
///     {
///         public LimitCheckMap() : base("RF_LIMIT_CHECK")
///         {
///             Map(x => x.TypeProgram, "TYPE_PROGRAM").Not.Nullable().CustomType<TypeProgramRequest>();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Проверка лимитов по заявке"</summary>
    public class LimitCheckMap : BaseEntityMap<LimitCheck>
    {
        
        public LimitCheckMap() : 
                base("Проверка лимитов по заявке", "RF_LIMIT_CHECK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeProgram, "Тип программы заявки перечисления средств").Column("TYPE_PROGRAM").NotNull();
        }
    }
}
