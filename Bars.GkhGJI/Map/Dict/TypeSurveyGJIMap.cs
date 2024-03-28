/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Тип обследования"
///     /// </summary>
///     public class TypeSurveyGjiMap : BaseGkhEntityMap<TypeSurveyGji>
///     {
///         public TypeSurveyGjiMap()
///             : base("GJI_DICT_TYPESURVEY")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///             Map(x => x.NameGenitive, "NAME_GENITIVE").Length(300);
///             Map(x => x.NameDative, "NAME_DATIVE").Length(300);
///             Map(x => x.NameAccusative, "NAME_ACCUSATIVE").Length(300);
///             Map(x => x.NameAblative, "NAME_ABLATIVE").Length(300);
///             Map(x => x.NamePrepositional, "NAME_PREPOSITIONAL").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Тип обследования ГЖИ"</summary>
    public class TypeSurveyGjiMap : BaseEntityMap<TypeSurveyGji>
    {
        
        public TypeSurveyGjiMap() : 
                base("Тип обследования ГЖИ", "GJI_DICT_TYPESURVEY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.NameGenitive, "Наименование Родительный падеж").Column("NAME_GENITIVE").Length(300);
            Property(x => x.NameDative, "Наименование Дательный падеж").Column("NAME_DATIVE").Length(300);
            Property(x => x.NameAccusative, "Наименование Винительный падеж").Column("NAME_ACCUSATIVE").Length(300);
            Property(x => x.NameAblative, "Наименование Творительный падеж").Column("NAME_ABLATIVE").Length(300);
            Property(x => x.NamePrepositional, "Наименование Предложный падеж").Column("NAME_PREPOSITIONAL").Length(300);
        }
    }
}
