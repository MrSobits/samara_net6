namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVSocialHireMap : BaseEntityMap<SMEVSocialHire>
    {
        
        public SMEVSocialHireMap() : 
                base("Запрос к ВС СГИО", "GJI_CH_SMEV_SOCIAL_HIRE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").Fetch();
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Reference(x => x.Room, "Room").Column("ROOM_ID").Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.ContractNumber, "Номер договора").Column("CON_NUM");
            Property(x => x.ContractType, "Тип договора").Column("CON_TYPE");
            Property(x => x.Name, "Наименование объекта").Column("NAME");
            Property(x => x.Purpose, "Назначение объекта").Column("PURPOSE");
            Property(x => x.TotalArea, "Общая площадь").Column("TOTAL_AREA");
            Property(x => x.LiveArea, "Жилая площадь").Column("LIVE_AREA");
            Property(x => x.LastName, "Фамилия").Column("LAST_NAME");
            Property(x => x.FirstName, "Имя").Column("FIRST_NAME");
            Property(x => x.MiddleName, "Отчество").Column("MID_NAME");
            Property(x => x.Birthday, "Дата рождения").Column("BIRTHDAY");
            Property(x => x.Birthplace, "Место рождения").Column("BIRTHPLACE");
            Property(x => x.Citizenship, "Гражданство").Column("CITIZENSHIP");
            Property(x => x.DocumentType, "Вид документа, удостоверяющего личность").Column("DOC_TYPE");
            Property(x => x.DocumentNumber, "Номер документа, удостоверяющего личность").Column("DOC_NUM");
            Property(x => x.DocumentSeries, "Серия документа, удостоверяющего личность").Column("DOC_SERIES");
            Property(x => x.DocumentDate, "Дата выдачи документа, удостоверяющего личность").Column("DOC_DATE");
            Property(x => x.IsContractOwner, "Владелец договора").Column("IS_OWNER");
            Property(x => x.AnswerDistrict, "Район").Column("ANSWER_DISTRICT");
            Property(x => x.AnswerCity, "Город").Column("ANSWER_CITY");
            Property(x => x.AnswerStreet, "Улица").Column("ANSWER_STREET");
            Property(x => x.AnswerHouse, "Дом").Column("ANSWER_HOUSE");
            Property(x => x.AnswerFlat, "Квартира").Column("ANSWER_FLAT");
            Property(x => x.AnswerRegion, "Регион").Column("ANSWER_REGION");
        }
    }
}
