namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание распоряжения руководителя ГЖИ"</summary>
    public class BaseDispHeadMap : JoinedSubClassMap<BaseDispHead>
    {
        
        public BaseDispHeadMap() : 
                base("Основание распоряжения руководителя ГЖИ", "GJI_INSPECTION_DISPHEAD")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.DispHeadDate, "Дата распоряжения").Column("DISPHEAD_DATE");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(50);
            this.Property(x => x.TypeBaseDispHead, "Тип основания проверки поручения руководства").Column("TYPE_BASE_DISPHEAD").NotNull();
            this.Property(x => x.TypeForm, "Форма проверки").Column("TYPE_FORM").NotNull();
            this.Property(x => x.Inn, "ИНН физ./долж. лица").Column("INN");
            this.Reference(x => x.Head, "Руководитель").Column("HEAD_ID");
            this.Reference(x => x.File, "Файл").Column("FILE_INFO_ID");
        }
    }
}
