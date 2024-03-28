/// <mapping-converter-backup>
/// using Bars.GkhGji.Regions.Stavropol.Entities;
/// using Bars.GkhGji.Regions.Stavropol.Enums;
/// 
/// namespace Bars.GkhGji.Regions.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Определения постановления прокуратуры ГЖИ"
///     /// </summary>
///     public class ResolProsDefinitionMap : BaseGkhEntityMap<ResolProsDefinition>
///     {
/// 		public ResolProsDefinitionMap()
///             : base("GJI_RESOLPROS_DEFINITION")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ExecutionDate, "EXECUTION_DATE");
///             Map(x => x.DocumentNumber, "DOC_NUMBER");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.TypeDefinition, "TYPE_DEFINITION").Not.Nullable().CustomType<TypeDefinitionResolPros>();
///             Map(x => x.TimeDefinition, "DEF_TIME");
///             Map(x => x.DateOfProceedings, "DATE_PROC");
/// 
///             References(x => x.ResolPros, "RESOLPROS_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedDefinition, "ISSUED_DEFINITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Stavropol.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Stavropol.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Stavropol.Entities.ResolProsDefinition"</summary>
    public class ResolProsDefinitionMap : BaseEntityMap<ResolProsDefinition>
    {
        
        public ResolProsDefinitionMap() : 
                base("Bars.GkhGji.Regions.Stavropol.Entities.ResolProsDefinition", "GJI_RESOLPROS_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "ExecutionDate").Column("EXECUTION_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOC_NUMBER");
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(2000);
            Property(x => x.TypeDefinition, "TypeDefinition").Column("TYPE_DEFINITION").NotNull();
            Property(x => x.TimeDefinition, "TimeDefinition").Column("DEF_TIME");
            Property(x => x.DateOfProceedings, "DateOfProceedings").Column("DATE_PROC");
            Reference(x => x.ResolPros, "ResolPros").Column("RESOLPROS_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "IssuedDefinition").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
