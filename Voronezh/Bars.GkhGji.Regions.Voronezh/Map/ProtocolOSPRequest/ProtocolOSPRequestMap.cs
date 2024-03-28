namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities;


    /// <summary>Маппинг для "Протокол заявки ОСП"</summary>
    public class ProtocolOSPRequestMap : BaseEntityMap<ProtocolOSPRequest>
    {
        
        public ProtocolOSPRequestMap() : 
                base("Протокол заявки ОСП", "GJI_PROTOCOL_OSP_REQUEST")
        {
        }

        protected override void Map()
        {
            Property(x => x.FIO, "ФИО").Column("FIO").NotNull();
            Property(x => x.Address, "Адрес").Column("ADDRESS");
            Property(x => x.Room, "Адрес").Column("ROOM");
            Property(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY");
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID");
            Property(x => x.RoFiasGuid, "ФИАС ГУИД Дома").Column("RO_FIAS_GUID").NotNull();
            Property(x => x.UserEsiaGuid, "ФИАС ГУИД Пользователя ЕСИА").Column("USER_ESIA_GUID").NotNull();
            Property(x => x.Date, "Дата заявки").Column("DATE").NotNull();
            Property(x => x.GjiId, "ИД заявки на сайте ГЖИ").Column("GJI_ID").NotNull();
            Property(x => x.Approved, "Одобрено").Column("APPROVED").NotNull();
            Property(x => x.Email, "Электронная почта").Column("EMAIL").NotNull();
            Property(x => x.DateFrom, "Дата c").Column("DATE_FROM");
            Property(x => x.DateTo, "Дата по").Column("DATE_TO");
            Property(x => x.CadastralNumber, "Кадастровый номер").Column("CADASTRAL_NUMBER");
            Property(x => x.AttorneyDate, "Дата доверенность").Column("ATTORNEY_DATE");
            Property(x => x.AttorneyNumber, "Дата доверенность").Column("ATTORNEY_NUMBER");
            Property(x => x.AttorneyFio, "ФИО Доверителя").Column("ATTORNEY_FIO");
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID");
            Reference(x => x.AttorneyFile, "Файл").Column("ATTFILE_ID");
            Reference(x => x.ProtocolFile, "Файл").Column("PROTFILE_ID");
            Property(x => x.ResolutionContent, "Причина отказа").Column("RESOLUTION_CONTENT");
            Reference(x => x.OwnerProtocolType, "Повестка").Column("PROTOCOL_TYPE");
            Property(x => x.PhoneNumber, "Номер телефона").Column("PHONE");
            Property(x => x.RequestNumber, "Номер заявки").Column("REQ_NUMBER");
            Property(x => x.ApplicantType, "Тип заявителя").Column("APPLICANT_TYPE");
            Reference(x => x.Inspector, "Inspector").Column("INSPECTOR_ID");
            Property(x => x.DocNumber, "Серия и номер документа собственности").Column("DOC_NUMBER");
            Property(x => x.DocDate, "Дата документа собственности").Column("DOC_DATE");
            Property(x => x.ProtocolDate, "Дата требуемого протокола").Column("PROTOCOL_DATE");
            Property(x => x.ProtocolNum, "Номер требуемого протокола").Column("PROTOCOL_NUMBER");
            Property(x => x.Note, "Примечание").Column("NOTE");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
