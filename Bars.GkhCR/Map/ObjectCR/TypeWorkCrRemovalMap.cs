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
///     public class TypeWorkCrRemovalMap : BaseImportableEntityMap<TypeWorkCrRemoval>
///     {
///         public TypeWorkCrRemovalMap()
///             : base("CR_OBJ_TYPE_WORK_REMOVAL")
///         {
///             Map(x => x.NumDoc, "NUM_DOC").Length(100);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.DateDoc, "DATE_DOC");
///             Map(x => x.YearRepair, "YEAR_REPAIR");
///             Map(x => x.NewYearRepair, "NEW_YEAR_REPAIR");
///             Map(x => x.TypeReason, "TYPE_REASON").Not.Nullable().CustomType<TypeWorkCrReason>();
/// 
///             References(x => x.TypeWorkCr, "TYPE_WORK_ID").Not.Nullable().Fetch.Join();
///             References(x => x.FileDoc, "FILE_DOC_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Причина изменения вида работы объекта КР"</summary>
    public class TypeWorkCrRemovalMap : BaseImportableEntityMap<TypeWorkCrRemoval>
    {
        
        public TypeWorkCrRemovalMap() : 
                base("Причина изменения вида работы объекта КР", "CR_OBJ_TYPE_WORK_REMOVAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NumDoc, "Номер документа").Column("NUM_DOC").Length(100);
            Property(x => x.Description, "Документ Основание").Column("DESCRIPTION").Length(2000);
            Property(x => x.StructElement, "Конструктивный элемент").Column("STRUCT_EL").Length(500);
            Property(x => x.DateDoc, "Дата документа От").Column("DATE_DOC");
            Property(x => x.YearRepair, "Год выполнения по долгосрочной программе").Column("YEAR_REPAIR");
            Property(x => x.NewYearRepair, "Новый год выполнения").Column("NEW_YEAR_REPAIR");
            Property(x => x.TypeReason, "Причина удаления").Column("TYPE_REASON").NotNull();
            Reference(x => x.TypeWorkCr, "Вид работы объекта КР").Column("TYPE_WORK_ID").NotNull().Fetch();
            Reference(x => x.FileDoc, "Документ (основание)").Column("FILE_DOC_ID").Fetch();
        }
    }
}
