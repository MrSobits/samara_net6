namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Предостережение - Нарушение"</summary>
    public class AppCitPrFondObjectCrMap : BaseEntityMap<AppCitPrFondObjectCr>
    {
        
        public AppCitPrFondObjectCrMap() : 
                base("Обращениям граждан - Предписание ФКР - Виды работ", "GJI_APPCIT_PR_FOND_OBJECT_CR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AppealCitsPrescriptionFond, "Предписание ФКР").Column("PR_FOND_ID").NotNull().Fetch();
            Reference(x => x.ObjectCr, "CR OBJECT").Column("CR_OBJECT_ID").NotNull().Fetch();
        }
    }
}
