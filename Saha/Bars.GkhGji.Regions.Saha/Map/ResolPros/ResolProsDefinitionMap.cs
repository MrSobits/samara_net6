/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// using Bars.GkhGji.Regions.Saha.Entities;
/// using Bars.GkhGji.Regions.Saha.Enums;
/// 
/// namespace Bars.GkhGji.Regions.Saha.Map
/// {
///     /// <summary>
///     /// Маппинг для сущности "Определения протокола ГЖИ"
///     /// </summary>
///     public class ResolProsDefinitionMap : BaseEntityMap<ResolProsDefinition>
///     {
///         public ResolProsDefinitionMap()
///             : base("GJI_RESOL_PROS_DEFINITION")
///         {
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.ExecutionDate, "EXECUTION_DATE");
///             Map(x => x.DocumentNumber, "DOC_NUMBER");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM").Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.TypeDefinition, "TYPE_DEFINITION").Not.Nullable().CustomType<TypeDefinitionResolPros>();
/// 
///             References(x => x.ResolPros, "RESOL_PROS_ID").Not.Nullable().Fetch.Join();
///             References(x => x.IssuedDefinition, "ISSUED_DEFINITION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Saha.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Saha.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Saha.Entities.ResolProsDefinition"</summary>
    public class ResolProsDefinitionMap : BaseEntityMap<ResolProsDefinition>
    {
        
        public ResolProsDefinitionMap() : 
                base("Bars.GkhGji.Regions.Saha.Entities.ResolProsDefinition", "GJI_RESOL_PROS_DEFINITION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            Property(x => x.ExecutionDate, "ExecutionDate").Column("EXECUTION_DATE");
            Property(x => x.DocumentNumber, "DocumentNumber").Column("DOC_NUMBER");
            Property(x => x.DocumentNum, "DocumentNum").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(2000);
            Property(x => x.TypeDefinition, "TypeDefinition").Column("TYPE_DEFINITION").NotNull();
            Reference(x => x.ResolPros, "ResolPros").Column("RESOL_PROS_ID").NotNull().Fetch();
            Reference(x => x.IssuedDefinition, "IssuedDefinition").Column("ISSUED_DEFINITION_ID").Fetch();
        }
    }
}
