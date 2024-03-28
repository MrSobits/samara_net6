namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Связи Расчета категории риска с коэффициентом Vпр"</summary>
    public class VprPrescriptionMap : BaseEntityMap<VprPrescription>
    {
        
        public VprPrescriptionMap() : 
                base("Коэффициент Vпр", "GJI_CH_ROM_VPR_PRESCRIPTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.StateName, "Статус").Column("STATE_NAME");
            Reference(x => x.Prescription, "Предписание").Column("PRESCRIPTION_ID").NotNull().Fetch();
            Reference(x => x.ROMCategory, "Расчет").Column("ROM_CATEGORY_ID").NotNull().Fetch();

        }
    }
}
