/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities;
/// 
///     public class ZonalInspectionMap : BaseGkhEntityMap<ZonalInspection>
///     {
///         public ZonalInspectionMap()
///             : base("GKH_DICT_ZONAINSP")
///         {
///             Map(x => x.Name, "NAME").Length(300).Not.Nullable();
///             Map(x => x.ZoneName, "ZONE_NAME").Length(300);
///             Map(x => x.BlankName, "BLANK_NAME").Length(300);
///             Map(x => x.ShortName, "SHORT_NAME").Length(300);
///             Map(x => x.Address, "ADDRESS").Length(300);
///             Map(x => x.NameSecond, "NAME_SECOND").Length(300);
///             Map(x => x.ZoneNameSecond, "ZONE_NAME_SECOND").Length(300);
///             Map(x => x.BlankNameSecond, "BLANK_NAME_SECOND").Length(300);
///             Map(x => x.ShortNameSecond, "SHORT_NAME_SECOND").Length(300);
///             Map(x => x.AddressSecond, "ADDRESS_SECOND").Length(300);
///             Map(x => x.Phone, "PHONE").Length(50);
///             Map(x => x.Email, "EMAIL").Length(50);
///             Map(x => x.Okato, "OKATO").Length(30);
/// 
///             Map(x => x.NameGenetive, "NAME_GENETIVE").Length(300);
///             Map(x => x.NameDative, "NAME_DATIVE").Length(300);
///             Map(x => x.NameAccusative, "NAME_ACCUSATIVE").Length(300);
///             Map(x => x.NameAblative, "NAME_ABLATIVE").Length(300);
///             Map(x => x.NamePrepositional, "NAME_PREPOSITIONAL").Length(300);
/// 
///             Map(x => x.ShortNameGenetive, "SHORT_NAME_GENETIVE").Length(300);
///             Map(x => x.ShortNameDative, "SHORT_NAME_DATIVE").Length(300);
///             Map(x => x.ShortNameAccusative, "SHORT_NAME_ACCUSATIVE").Length(300);
///             Map(x => x.ShortNameAblative, "SHORT_NAME_ABLATIVE").Length(300);
///             Map(x => x.ShortNamePrepositional, "SHORT_NAME_PREPOSITIONAL").Length(300);
///             Map(x => x.IndexOfGji, "INDEX_OF_GJI").Length(100);
///             Map(x => x.AppealCode, "APPEAL_CODE");
///             Map(x => x.Oktmo, "OKTMO").Length(30);
///             Map(x => x.Locality, "LOCALITY");
///             Map(x => x.DepartmentCode, "DEPARTMENT_CODE");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Зональная жилищная инспекция"</summary>
    public class ZonalInspectionMap : BaseImportableEntityMap<ZonalInspection>
    {
        
        public ZonalInspectionMap() : 
                base("Зональная жилищная инспекция", "GKH_DICT_ZONAINSP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.ZoneName, "Зональное наименование").Column("ZONE_NAME").Length(300);
            Property(x => x.BlankName, "Наименование для бланка").Column("BLANK_NAME").Length(300);
            Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(300);
            Property(x => x.Address, "Адрес").Column("ADDRESS").Length(300);
            Property(x => x.NameSecond, "Наименование (2 гос.язык)").Column("NAME_SECOND").Length(300);
            Property(x => x.ZoneNameSecond, "Зональное наименование (2 гос.язык)").Column("ZONE_NAME_SECOND").Length(300);
            Property(x => x.BlankNameSecond, "Наименование для бланка (2 гос.язык)").Column("BLANK_NAME_SECOND").Length(300);
            Property(x => x.ShortNameSecond, "Краткое наименование (2 гос.язык)").Column("SHORT_NAME_SECOND").Length(300);
            Property(x => x.AddressSecond, "Адрес (2 гос.язык)").Column("ADDRESS_SECOND").Length(300);
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(50);
            Property(x => x.Email, "E-mail").Column("EMAIL").Length(50);
            Property(x => x.Okato, "ОКАТО - Общероссийский классификатор объектов административно-территориального де" +
                    "ления. Пример: ОКАТО Татарстана 92000000000.").Column("OKATO").Length(30);
            Property(x => x.NameGenetive, "Наименование Родительный падеж").Column("NAME_GENETIVE").Length(300);
            Property(x => x.NameDative, "Наименование Дательный падеж").Column("NAME_DATIVE").Length(300);
            Property(x => x.NameAccusative, "Наименование Винительный падеж").Column("NAME_ACCUSATIVE").Length(300);
            Property(x => x.NameAblative, "Наименование Творительный падеж").Column("NAME_ABLATIVE").Length(300);
            Property(x => x.NamePrepositional, "Наименование Предложный падеж").Column("NAME_PREPOSITIONAL").Length(300);
            Property(x => x.ShortNameGenetive, "Краткое наименование Родительный падеж").Column("SHORT_NAME_GENETIVE").Length(300);
            Property(x => x.ShortNameDative, "Краткое наименование Дательный падеж").Column("SHORT_NAME_DATIVE").Length(300);
            Property(x => x.ShortNameAccusative, "Краткое наименование Винительный падеж").Column("SHORT_NAME_ACCUSATIVE").Length(300);
            Property(x => x.ShortNameAblative, "Краткое наименование Творительный падеж").Column("SHORT_NAME_ABLATIVE").Length(300);
            Property(x => x.ShortNamePrepositional, "Краткое наименование Предложный падеж").Column("SHORT_NAME_PREPOSITIONAL").Length(300);
            Property(x => x.IndexOfGji, "Индекс отдела гжи").Column("INDEX_OF_GJI").Length(100);
            Property(x => x.AppealCode, "Код нумерации обращения").Column("APPEAL_CODE");
            Property(x => x.Oktmo, "ОКТМО").Column("OKTMO").Length(30);
            Property(x => x.Locality, "Населенный пункт").Column("LOCALITY");
            Property(x => x.IsNotGZHI, "Не является ГЖИ").Column("IS_NOT_GZHI").NotNull();
            Property(x => x.DepartmentCode, "Код отдела - используется при формировании документа ГЖИ").Column("DEPARTMENT_CODE");
        }
    }
}
