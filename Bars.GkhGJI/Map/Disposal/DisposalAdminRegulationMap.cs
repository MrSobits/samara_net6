namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для сущности "Административный регламент приказа ГЖИ"</summary>
	public class DisposalAdminRegulationMap : BaseEntityMap<DisposalAdminRegulation>
    {
        public DisposalAdminRegulationMap() : 
                base("Административный регламент приказа ГЖИ", "GJI_NSO_DISPOSAL_ADMREG")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            this.Reference(x => x.AdminRegulation, "Административный регламент").Column("ADMREG_ID").NotNull().Fetch();
        }
    }
}