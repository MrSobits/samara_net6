namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class CourtPracticeFileMap : BaseEntityMap<CourtPracticeFile>
    {
        
        public CourtPracticeFileMap() : 
                base("Приложение", "GJI_VR_COURT_PRACTICE_FILE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentName, "DocumentName").Column("DOC_NAME");
            Property(x => x.Description, "Discription").Column("DESCRIPTION");
            Property(x => x.DocDate, "DocDate").Column("DOC_DATE");

            Reference(x => x.CourtPractice, "CourtPractice").Column("CP_ID").NotNull();
            Reference(x => x.FileInfo, "Inspector").Column("FILE_ID").NotNull().Fetch();           
        }
    }
}
