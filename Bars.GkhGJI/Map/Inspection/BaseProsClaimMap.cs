/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки ГЖИ по требованию прокуратуры"
///     /// </summary>
///     public class BaseProsClaimMap : SubclassMap<BaseProsClaim>
///     {
///         public BaseProsClaimMap()
///         {
///             Table("GJI_INSPECTION_PROSCLAIM");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.IssuedClaim, "ISSUED_CLAIM").Length(300);
///             Map(x => x.ProsClaimDateCheck, "PROSCLAIM_DATE_CHECK");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(250);
///             Map(x => x.DocumentDescription, "DOCUMENT_DESCRIPTION").Length(500);
///             Map(x => x.TypeBaseProsClaim, "TYPE_BASE_PROSCLAIM").Not.Nullable().CustomType<TypeBaseProsClaim>();
///             Map(x => x.TypeForm, "TYPE_FORM").Not.Nullable().CustomType<TypeFormInspection>();
///             Map(x => x.IsResultSent, "IS_RESULT_SENT");
/// 
///             References(x => x.File, "FILE_INFO_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание требование прокуратуры ГЖИ"</summary>
    public class BaseProsClaimMap : JoinedSubClassMap<BaseProsClaim>
    {
        
        public BaseProsClaimMap() : 
                base("Основание требование прокуратуры ГЖИ", "GJI_INSPECTION_PROSCLAIM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IssuedClaim, "ДЛ, вынесшее требование").Column("ISSUED_CLAIM").Length(300);
            Property(x => x.ProsClaimDateCheck, "Дата проверки").Column("PROSCLAIM_DATE_CHECK");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(250);
            Property(x => x.DocumentDescription, "Описание документа").Column("DOCUMENT_DESCRIPTION").Length(500);
            Property(x => x.TypeBaseProsClaim, "Тип основания проверки по требованию прокуратуры").Column("TYPE_BASE_PROSCLAIM").NotNull();
            Property(x => x.TypeForm, "Форма проверки ЮЛ").Column("TYPE_FORM").NotNull();
            Property(x => x.IsResultSent, "Результаты отправлены в прокуратуру").Column("IS_RESULT_SENT");
            Property(x => x.Inn, "ИНН физ./долж. лица").Column("INN");
            Reference(x => x.File, "Файл").Column("FILE_INFO_ID");
        }
    }
}
