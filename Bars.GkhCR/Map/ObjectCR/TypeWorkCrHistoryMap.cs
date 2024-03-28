/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhCr.Entities;
///     using Bars.GkhCr.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Вид работы КР"
///     /// </summary>
///     public class TypeWorkCrHistoryMap : BaseImportableEntityMap<TypeWorkCrHistory>
///     {
///         public TypeWorkCrHistoryMap()
///             : base("CR_OBJ_TYPE_WORK_HIST")
///         {
///             Map(x => x.Volume, "VOLUME");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.YearRepair, "YEAR_REPAIR");
///             Map(x => x.NewYearRepair, "NEW_YEAR_REPAIR");
///             Map(x => x.UserName, "USER_NAME").Length(300);
///             Map(x => x.TypeAction, "TYPE_ACTION").Not.Nullable().CustomType<TypeWorkCrHistoryAction>();
///             Map(x => x.TypeReason, "TYPE_REASON").Not.Nullable().CustomType<TypeWorkCrReason>();
/// 
///             References(x => x.TypeWorkCr, "TYPE_WORK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FinanceSource, "FIN_SOURCE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "История изменения вида работы объекта КР"</summary>
    public class TypeWorkCrHistoryMap : BaseImportableEntityMap<TypeWorkCrHistory>
    {
        
        public TypeWorkCrHistoryMap() : 
                base("История изменения вида работы объекта КР", "CR_OBJ_TYPE_WORK_HIST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Volume, "Объем выполнения").Column("VOLUME");
            Property(x => x.Sum, "Сумма расходов").Column("SUM");
            Property(x => x.YearRepair, "Год ремонта").Column("YEAR_REPAIR");
            Property(x => x.NewYearRepair, "Новый год ремонта").Column("NEW_YEAR_REPAIR");
            Property(x => x.UserName, "Имя пользователя").Column("USER_NAME").Length(300);
            Property(x => x.TypeAction, "Тип действия для истории вида работ объекта КР").Column("TYPE_ACTION").NotNull();
            Property(x => x.TypeReason, "Причина").Column("TYPE_REASON").NotNull();
            Property(x => x.StructElement, "Конструктивный элемент").Column("STRUCT_EL").Length(500);
            Reference(x => x.TypeWorkCr, "Вид работы объекта КР").Column("TYPE_WORK_ID").NotNull().Fetch();
            Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
        }
    }
}
