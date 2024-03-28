namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class CourtPracticePrescriptionMap : BaseEntityMap<CourtPracticePrescription>
    {
        
        public CourtPracticePrescriptionMap() : 
                base("Приложение", "GJI_CH_COURT_PRACTICE_GJIDOC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CourtPractice, "CourtPractice").Column("CP_ID").NotNull().Fetch();
            Reference(x => x.DocumentGji, "DocumentGji").Column("DOCUMENT_ID").NotNull().Fetch();           
        }
    }
}
