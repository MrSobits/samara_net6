/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "оплаты штрафов постановления ГЖИ"
///     /// </summary>
///     public class ResolutionPayFineMap : BaseGkhEntityMap<ResolutionPayFine>
///     {
///         public ResolutionPayFineMap()
///             : base("GJI_RESOLUTION_PAYFINE")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(50);
///             Map(x => x.TypeDocumentPaid, "KIND_PAY").Not.Nullable().CustomType<TypeDocumentPaidGji>();
///             Map(x => x.Amount, "AMOUNT");
///             Map(x => x.GisUip, "GIS_UIP").Length(50);
/// 
///             References(x => x.Resolution, "RESOLUTION_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Оплата штрафов в постановлении ГЖИ"</summary>
    public class ResolutionPayFineMap : BaseEntityMap<ResolutionPayFine>
    {
        
        public ResolutionPayFineMap() : 
                base("Оплата штрафов в постановлении ГЖИ", "GJI_RESOLUTION_PAYFINE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "номер документа").Column("DOCUMENT_NUM").Length(50);
            Property(x => x.TypeDocumentPaid, "Тип документа оплаты штрафа").Column("KIND_PAY").NotNull();
            Property(x => x.Amount, "сумма штрафа").Column("AMOUNT");
            Property(x => x.GisUip, "Код из Гис программы").Column("GIS_UIP").Length(50);
            Reference(x => x.Resolution, "Постановление").Column("RESOLUTION_ID").NotNull().Fetch();
        }
    }
}
