/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Entities;
///     using FluentNHibernate.Mapping;
/// 
///     public class AdditionalServiceMap : SubclassMap<AdditionalService>
///     {
///         public AdditionalServiceMap()
///         {
///             Table("DI_ADDITION_SERVICE");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///         
/// 
///             Map(x => x.Document, "DOCUMENT").Length(300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(300);
/// 
///             Map(x => x.DocumentFrom, "DOCUMENT_FROM");
///             Map(x => x.DateStart, "DATE_START").Not.Nullable();
///             Map(x => x.DateEnd, "DATE_END").Not.Nullable();
///             Map(x => x.Total, "TOTAL").Not.Nullable();
/// 
///             References(x => x.Periodicity, "PERIODICITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.AdditionalService"</summary>
    public class AdditionalServiceMap : JoinedSubClassMap<AdditionalService>
    {
        
        public AdditionalServiceMap() : 
                base("Bars.GkhDi.Entities.AdditionalService", "DI_ADDITION_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Document, "Document").Column("DOCUMENT").Length(300);
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER").Length(300);
            Property(x => x.DocumentFrom, "DocumentFrom").Column("DOCUMENT_FROM");
            Property(x => x.DateStart, "DateStart").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END").NotNull();
            Property(x => x.Total, "Total").Column("TOTAL").NotNull();
            Reference(x => x.Periodicity, "Periodicity").Column("PERIODICITY_ID").Fetch();
        }
    }
}
