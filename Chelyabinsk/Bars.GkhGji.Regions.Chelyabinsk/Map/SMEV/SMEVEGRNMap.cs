namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRNMap : BaseEntityMap<SMEVEGRN>
    {
        
        public SMEVEGRNMap() : 
                base("Предоставление данных из ФГИС ЕГРП ", "GJI_CH_SMEV_EGRN")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.RequestDate, "Дата запроса").Column("REQUEST_DATE").NotNull();
            Property(x => x.DeclarantId, "ID заявителя").Column("DECLARANT_ID").NotNull();
            Reference(x => x.RegionCode, "Код региона").Column("REG_CODE_ID").Fetch();
            Reference(x => x.EGRNApplicantType, "Тип заявителя").Column("EGRN_APPLICANT_TYPE_ID").Fetch();
            Reference(x => x.EGRNObjectType, "Тип объекта запроса").Column("EGRN_OBJECT_TYPE_ID").NotNull().Fetch();
            Property(x => x.IdDocumentRef, "Данные удостоверения личности").Column("DOCUMENT_REF");
            Property(x => x.PersonName, "Имя заявителя").Column("PERSON_NAME");
            Property(x => x.PersonSurname, "Фамилия заявителя").Column("PERSON_SURNAME");
            Property(x => x.RequestType, "Тип запроса").Column("REQUEST_TYPE");
            Property(x => x.RequestDataType, "Вид запроса данных по ОН").Column("REQUEST_DATA_TYPE");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.PersonPatronymic, "Отчество").Column("PERSON_PATRONYMIC");
            Property(x => x.DocumentSerial, "Серия документа").Column("DOCUMENT_SERIAL");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.CadastralNUmber, "Номер документа").Column("CADASTRAL_NUMBER");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.QualityPhone, "Телефон для службы опроса качества").Column("QUALITY_PHONE");
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").Fetch();
            Reference(x => x.Room, "Помещение").Column("ROOM_ID").Fetch();
        }
    }
}
