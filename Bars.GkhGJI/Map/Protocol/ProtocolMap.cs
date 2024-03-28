/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол"
///     /// </summary>
///     public class ProtocolMap : SubclassMap<Protocol>
///     {
///         public ProtocolMap()
///         {
///             Table("GJI_PROTOCOL");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(300);
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.DateToCourt, "DATE_TO_COURT");
///             Map(x => x.ToCourt, "TO_COURT");
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
/// 
///             Map(x => x.DateOfProceedings, "DATE_OF_PROCEEDINGS");
///             Map(x => x.HourOfProceedings, "HOUR_OF_PROCEEDINGS");
///             Map(x => x.MinuteOfProceedings, "MINUTE_OF_PROCEEDINGS");
/// 
///             Map(x => x.PersonFollowConversion, "PERSON_FOLLOW_CONVERSION");
/// 
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Протокол"</summary>
    public class ProtocolMap : JoinedSubClassMap<Protocol>
    {
        
        public ProtocolMap() : 
                base("Протокол", "GJI_PROTOCOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DateToCourt, "Дата передачи в суд").Column("DATE_TO_COURT");
            Property(x => x.BirthDay, "Дата передачи в суд").Column("BIRTH_DAY");
            Property(x => x.ToCourt, "Документ передан в суд").Column("TO_COURT");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Property(x => x.DateOfProceedings, "Дата рассмотрения дела").Column("DATE_OF_PROCEEDINGS");
            Property(x => x.HourOfProceedings, "Время рассмотрения дела(час)").Column("HOUR_OF_PROCEEDINGS");
            Property(x => x.MinuteOfProceedings, "Время рассмотрения дела(мин)").Column("MINUTE_OF_PROCEEDINGS");
            Property(x => x.PersonFollowConversion, "Лицо, выполнившее перепланировку/переустройство").Column("PERSON_FOLLOW_CONVERSION");
            Property(x => x.PhysicalPersonPosition, "Должность").Column("PP_POSITION");
            Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
            this.Property(x => x.UIN, "УИН ГИС ГМП").Column("UIN");
            this.Property(x => x.FormatDate, "FormatDate").Column("FORMAT_DATE");
            this.Property(x => x.FormatPlace, "FormatPlace").Column("FORMAT_PLACE").Length(500);
            this.Property(x => x.FormatHour, "FormatHour").Column("FORMAT_HOUR");
            this.Property(x => x.FormatMinute, "FormatMinute").Column("FORMAT_MINUTE");
            this.Property(x => x.NotifNumber, "NotifNumber").Column("NOTIF_NUM").Length(100);
            this.Property(x => x.ProceedingsPlace, "ProceedingsPlace").Column("PROCEEDINGS_PLACE").Length(100);
            this.Property(x => x.Remarks, "Remarks").Column("REMARKS").Length(100);
            this.Property(x => x.NotifDeliveredThroughOffice, "NotifDeliveredThroughOffice").Column("DELIV_THROUGH_OFFICE");
            this.Property(x => x.ProceedingCopyNum, "ProceedingCopyNum").Column("PROCEEDING_COPY_NUM");

            Property(x => x.PhysicalPersonDocumentNumber, "Номер документа").Column("PHYSICALPERSON_DOC_NUM").Length(500);
            Property(x => x.PhysicalPersonIsNotRF, "Гражданство").Column("PHYSICALPERSON_NOT_CITIZENSHIP").DefaultValue(false);
            Property(x => x.PhysicalPersonDocumentSerial, "Серия документа").Column("PHYSICALPERSON_DOC_SERIAL").Length(500);
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();

            Property(x => x.TypeAddress, "TypeAdress").Column("TYPE_ADDRESS");
            Property(x => x.PlaceOffense, "PlaceOffense").Column("PLACE_OFFENSE");
            this.Reference(x => x.FiasPlaceAddress, "Место выдачи документа").Column("FIAS_PLACE_ADDRESS").Fetch();
            Property(x => x.AddressPlace, "Адрес места совершения правонарушения").Column("ADDRESS_PLACE");
            Property(x => x.Familrefusal, "Отказ от ознакомления").Column("FAMILIARIZE_REFUSAL");
            this.Reference(x => x.JudSector, "Cудебный участок").Column("JUD_SECTOR").Fetch();
        }
    }
}
