/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ManOrg
/// {
///     using Entities;
///     using Enums;
/// 
///     public class ManOrgContractRelationMap : BaseGkhEntityMap<ManOrgContractRelation>
///     {
///         public ManOrgContractRelationMap()
///             : base("GKH_MORG_CONTR_REL")
///         {
///             Map(x => x.TypeRelation, "TYPE_CONTRACT_RELATION").Not.Nullable().CustomType<TypeContractRelation>();
/// 
///             References(x => x.Parent, "PARENT_CONTRACT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Children, "CHILDREN_CONTRACT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Связь между двумя договорами"</summary>
    public class ManOrgContractRelationMap : BaseImportableEntityMap<ManOrgContractRelation>
    {
        
        public ManOrgContractRelationMap() : 
                base("Связь между двумя договорами", "GKH_MORG_CONTR_REL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeRelation, "Тип связи").Column("TYPE_CONTRACT_RELATION").NotNull();
            Reference(x => x.Parent, "Родительский договор").Column("PARENT_CONTRACT_ID").NotNull().Fetch();
            Reference(x => x.Children, "Дочерний договор").Column("CHILDREN_CONTRACT_ID").NotNull().Fetch();
        }
    }
}
