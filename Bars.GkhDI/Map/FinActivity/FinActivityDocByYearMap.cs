/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Документы фин деятельности (сметы доходов и Заключение рев коммиссии) в разрезе по годам"
///     /// </summary>
///     public class FinActivityDocByYearMap : BaseGkhEntityMap<FinActivityDocByYear>
///     {
///         public FinActivityDocByYearMap(): base("DI_DISINFO_FINACT_DOCYEAR")
///         {
///             Map(x => x.TypeDocByYearDi, "TYPE_DOC_BY_YEAR").Not.Nullable().CustomType<TypeDocByYearDi>();
///             Map(x => x.Year, "YEAR").Not.Nullable();
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             
///             References(x => x.ManagingOrganization, "MANAGING_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityDocByYear"</summary>
    public class FinActivityDocByYearMap : BaseImportableEntityMap<FinActivityDocByYear>
    {
        
        public FinActivityDocByYearMap() : 
                base("Bars.GkhDi.Entities.FinActivityDocByYear", "DI_DISINFO_FINACT_DOCYEAR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeDocByYearDi, "TypeDocByYearDi").Column("TYPE_DOC_BY_YEAR").NotNull();
            Property(x => x.Year, "Year").Column("YEAR").NotNull();
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Reference(x => x.ManagingOrganization, "ManagingOrganization").Column("MANAGING_ORG_ID").NotNull().Fetch();
            Reference(x => x.File, "File").Column("FILE_ID").Fetch();
        }
    }
}
