namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Категория документации"</summary>
    public class InstructionGroupMap : BaseEntityMap<InstructionGroup>
    {
        public InstructionGroupMap() : base("Категория документации", "GKH_INSTR_GROUP")
        {
        }

        protected override void Map()
        {
            Property(x => x.DisplayName, "Отображаемое имя").Column("DISPLAY_NAME").Length(255).NotNull();
        }
    }
}
