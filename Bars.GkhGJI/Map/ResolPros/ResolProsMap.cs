/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Постановление прокуратуры"
///     /// </summary>
///     public class ResolProsMap : SubclassMap<ResolPros>
///     {
///         public ResolProsMap()
///         {
///             Table("GJI_RESOLPROS");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(300);
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.DateSupply, "DATE_SUPPLY");
/// 
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Постановление прокуратуры"</summary>
    public class ResolProsMap : JoinedSubClassMap<ResolPros>
    {
        
        public ResolProsMap() : 
                base("Постановление прокуратуры", "GJI_RESOLPROS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.UIN, "УИН").Column("UIN");
            Property(x => x.IssuedByPosition, "ISSUED_POSITION").Column("ISSUED_POSITION");
            Property(x => x.IssuedByRank, "ISSUED_RANK").Column("ISSUED_RANK");
            Property(x => x.IssuedByFio, "ISSUED_FIO").Column("ISSUED_FIO");
            Property(x => x.PhysicalPersonPosition, "Должность").Column("PP_POSITION");
            Property(x => x.DateSupply, "Дата поступления в ГЖИ").Column("DATE_SUPPLY");
            Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            Reference(x => x.ProsecutorOffice, "Орган прокуратуры").Column("PROS_ID").Fetch();
            Reference(x => x.Municipality, "Муниципальное образование (Орган прокуратуры, вынесший постановление)").Column("MUNICIPALITY_ID").Fetch();
            Property(x => x.FormatHour, "Часы постановление прокуратуры").Column("FORMAT_HOUR");
            Property(x => x.FormatMinute, "Минуты постановление прокуратуры").Column("FORMAT_MINUTE");
            Property(x => x.DateResolPros, "Дата рассмотрения постановления прокуратуры").Column("DATE_RESOL_PROS");

            Property(x => x.Surname, "Фамилия").Column("SURNAME");
            Property(x => x.FirstName, "Имя").Column("FIRSTNAME");
            Property(x => x.Patronymic, "Отчество").Column("PATRONYMIC");
            Property(x => x.PhysicalPersonDocumentNumber, "Номер документа").Column("PHYSICALPERSON_DOC_NUM").Length(500);
            Property(x => x.PhysicalPersonIsNotRF, "Гражданство").Column("PHYSICALPERSON_NOT_CITIZENSHIP").DefaultValue(false);
            Property(x => x.PhysicalPersonDocumentSerial, "Серия документа").Column("PHYSICALPERSON_DOC_SERIAL").Length(500);
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();
            Property(x => x.PersonRegistrationAddress, "Нарушение").Column("PERSON_REG_ADDRESS");
            Property(x => x.PersonFactAddress, "Нарушение").Column("PERSON_FACT_ADDRESS");
            Property(x => x.TypePresence, "TypePresence").Column("TYPE_PRESENCE");
            Property(x => x.Representative, "Representative").Column("REPRESENTATIVE").Length(500);
            Property(x => x.ReasonTypeRequisites, "ReasonTypeRequisites").Column("REASON_TYPE_REQ").Length(1000);
            Property(x => x.PersonBirthDate, "TypePresence").Column("PERSON_BIRTH_DATE");
            Property(x => x.PersonBirthPlace, "TypePresence").Column("PERSON_BIRTH_PLACE");
        }
    }
}
