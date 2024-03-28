/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Квалификационный отбор"
///     /// </summary>
///     public class QualificationMap : BaseGkhEntityMap<Qualification>
///     {
///         public QualificationMap() : base("CR_QUALIFICATION")
///         {
///             Map(x => x.Sum, "SUM");
/// 
///             References(x => x.ObjectCr, "OBJECT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Builder, "BUILDER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Квалификационный отбор"</summary>
    public class QualificationMap : BaseImportableEntityMap<Qualification>
    {
        
        public QualificationMap() : 
                base("Квалификационный отбор", "CR_QUALIFICATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Sum, "Сумма").Column("SUM");
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Builder, "Подрядчик").Column("BUILDER_ID").NotNull().Fetch();
        }
    }
}
