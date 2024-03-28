/// <mapping-converter-backup>
/// using Bars.B4.DataAccess;
/// 
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Контрагент"
///     /// </summary>
///     public class PersonMap : BaseImportableEntityMap<Person>
///     {
///         public PersonMap()
///             : base("GKH_PERSON")
///         {
///             Map(x => x.TypeIdentityDocument, "IDENT_TYPE").Not.Nullable().CustomType<TypeIdentityDocument>();
/// 
///             Map(x => x.Email, "EMAIL").Length(200);
///             Map(x => x.Phone, "PHONE").Length(200);
///             Map(x => x.Inn, "INN").Length(20);
///             Map(x => x.Name, "NAME").Not.Nullable().Length(100);
///             Map(x => x.Surname, "SURNAME").Not.Nullable().Length(100);
///             Map(x => x.Patronymic, "PATRONYMIC").Length(100);
///             Map(x => x.FullName, "FULL_NAME").Length(500);
///             Map(x => x.AddressReg, "ADDRESS_REG").Length(2000);
///             Map(x => x.AddressLive, "ADDRESS_LIVE").Length(2000);
/// 			Map(x => x.AddressBirth, "ADDRESS_BIRTH").Length(2000);
/// 			Map(x => x.Birthdate, "BIRTHDATE");
/// 
///             Map(x => x.IdSerial, "IDENT_SERIAL").Length(10);
///             Map(x => x.IdNumber, "IDENT_NUMBER").Length(10);
///             Map(x => x.IdIssuedDate, "IDENT_ISSUEDDATE");
///             Map(x => x.IdIssuedBy, "IDENT_ISSUEDBY").Length(2000);
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Физ лицо"</summary>
    public class PersonMap : BaseImportableEntityMap<Person>
    {
        
        public PersonMap() : 
                base("Физ лицо", "GKH_PERSON")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeIdentityDocument, "Документ удостоверяющий личность").Column("IDENT_TYPE").NotNull();
            Property(x => x.Email, "Email").Column("EMAIL").Length(200);
            Property(x => x.Phone, "Телефон").Column("PHONE").Length(200);
            Property(x => x.Inn, "ИНН").Column("INN").Length(20);
            Property(x => x.Name, "Имя").Column("NAME").Length(100).NotNull();
            Property(x => x.Surname, "Фамилия").Column("SURNAME").Length(100).NotNull();
            Property(x => x.Patronymic, "Отчество").Column("PATRONYMIC").Length(100);
            Property(x => x.FullName, "ФИО - автогенерируемое поле после сохранения").Column("FULL_NAME").Length(500);
            Property(x => x.AddressReg, "Адрес регистрации").Column("ADDRESS_REG").Length(2000);
            Property(x => x.AddressLive, "Адрес места жительства").Column("ADDRESS_LIVE").Length(2000);
            Property(x => x.AddressBirth, "Адрес места рождения").Column("ADDRESS_BIRTH").Length(2000);
            Property(x => x.Birthdate, "Дата рождения").Column("BIRTHDATE");
            Property(x => x.IdSerial, "Серия документа удостоверяющего личность").Column("IDENT_SERIAL").Length(10);
            Property(x => x.IdNumber, "Номер документа удостоверяющего личность").Column("IDENT_NUMBER").Length(10);
            Property(x => x.IdIssuedDate, "Дата выдачи документа удостоверяющег оличность").Column("IDENT_ISSUEDDATE");
            Property(x => x.IdIssuedBy, "Кем выдан документ удостоверяющий личность").Column("IDENT_ISSUEDBY").Length(2000);
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
