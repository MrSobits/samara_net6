namespace Bars.Gkh.Map.Contragent
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Дополнительные роли контрагента"</summary>
    public class ContragentAdditionRoleMap : BaseEntityMap<ContragentAdditionRole>
    {
        public ContragentAdditionRoleMap() : 
                base("Дополнительные роли контрагента", "GKH_CONTRAGENT_ADDITION_ROLE")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            this.Reference(x => x.Role, "Роль").Column("ROLE_ID").NotNull().Fetch();
        }
    }
}
