namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.ResolProsDefinition"</summary>
    public class ResolProsDefinitionMap : BaseEntityMap<ResolProsDefinition>
    {
        
        public ResolProsDefinitionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.ResolProsDefinition", "GJI_RESOL_PROS_DEFINITION")
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
