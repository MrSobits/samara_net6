namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для сущности "НПА проверки приказа ГЖИ"</summary>
	public class DisposalInspFoundationCheckMap : BaseEntityMap<DisposalInspFoundationCheck>
    {
        public DisposalInspFoundationCheckMap() : 
                base("НПА проверки", "GJI_NSO_DISPOSAL_INSPFOUNDCHECK")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Disposal, "Распоряжение ГЖИ").Column("DISPOSAL_ID").NotNull().Fetch();
            this.Reference(x => x.InspFoundationCheck, "Правовое основание проведения проверки").Column("INSPFOUND_ID").NotNull().Fetch();
            this.Property(x => x.ErpGuid, nameof(ActCheckViolation.ErpGuid)).Column("ERP_GUID");
        }
    }
}
