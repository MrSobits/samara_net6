namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Роль для категории"</summary>
    public class InstructionGroupRoleMap : BaseEntityMap<InstructionGroupRole>
    {
        public InstructionGroupRoleMap() : base("Роль для категории", "GKH_INSTR_GROUP_ROLE")
        {
        }

        protected override void Map()
        {
            Reference(x => x.InstructionGroup, "Категория документации").Column("GROUP_ID").NotNull().Fetch();
            Reference(x => x.Role, "Роль").Column("ROLE_ID").NotNull().Fetch();
        }
    }
}
