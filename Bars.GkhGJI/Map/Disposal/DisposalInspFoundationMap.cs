namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для сущности "Правовое основание проведения проверки приказа ГЖИ"</summary>
	public class DisposalInspFoundationMap : BaseEntityMap<DisposalInspFoundation>
    {
        public DisposalInspFoundationMap() : 
                base("Правовое основание проведения проверки приказа ГЖИ", "GJI_NSO_DISPOSAL_INSPFOUND")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            this.Reference(x => x.InspFoundation, "Правовое основание проведения проверки").Column("INSPFOUND_ID").NotNull().Fetch();
        }
    }
}
