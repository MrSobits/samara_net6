
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Нарушение в служебной записке по предписанию"</summary>
    public class PrescriptionOfficialReportViolationMap : BaseEntityMap<PrescriptionOfficialReportViolation>
    {
        
        public PrescriptionOfficialReportViolationMap() : 
                base("Приложения предписания ГЖИ", "GJI_PRESCR_OFF_REPORT_VIOLATION")
        {
        }
        
        protected override void Map()
        {          
            Reference(x => x.PrescriptionOfficialReport, "Служебная записка").Column("OFFICIAL_REPORT_ID").NotNull().Fetch();
            Reference(x => x.PrescriptionViol, "Нарушение этапа предписания").Column("VIOL_STAGE_ID").NotNull().Fetch();
        }
    }
}
