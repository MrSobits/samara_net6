
namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Исполнитель документа ГЖИ"</summary>
    public class PreliminaryCheckMap : BaseEntityMap<PreliminaryCheck>
    {
        
        public PreliminaryCheckMap() : 
                base("Предварительная проверка", "GJI_PRELIMINARY_CHECK")
        {
        }
        
        protected override void Map()
        {          
            Property(x => x.Result, "Результат").Column("RESULT");
            Property(x => x.CheckDate, "Дата проверки").Column("CHECK_DATE");
            Property(x => x.PreliminaryCheckNumber, "Номер").Column("NUMBER");
            Property(x => x.PreliminaryCheckResult, "Результат").Column("RESULT_ENUM");
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID");
            Reference(x => x.AppealCits, "Обращение").Column("APPCIT_ID");
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.Inspector, "Проверяющий").Column("INSPECTOR_ID");

        }
    }
}
