namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Изменения статьи закона постановления ГЖИ"</summary>
    public class ResolutionFizMap : BaseEntityMap<ResolutionFiz>
    {
        
        public ResolutionFizMap() : 
                base("Реквизиты физлица в постановлениях ГЖИ", "GJI_CH_RESOLUTION_FIZ")
        {
        }
        
        protected override void Map()
        {
            
            Property(x => x.DocumentNumber, "Номер документа").Column("DOC_NUM").Length(500);
            Property(x => x.IsRF, "Гражданство").Column("CITIZENSHIP");
            Property(x => x.DocumentSerial, "Серия документа").Column("DOC_SERIAL").Length(500);
            Property(x => x.PayerCode, "Код плательщика").Column("PAYER_CODE").Length(500);
            Reference(x => x.Resolution, "Постановление").Column("RESOLUTION_ID").NotNull().Fetch();
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();
        }
    }
}
