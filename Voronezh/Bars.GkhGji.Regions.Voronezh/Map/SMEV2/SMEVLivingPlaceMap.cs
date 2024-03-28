namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVLivingPlaceMap : BaseEntityMap<SMEVLivingPlace>
    {
        
        public SMEVLivingPlaceMap() : 
                base("Запрос к ВС мест жительства", "GJI_CH_SMEV_LIVING_PLACE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.CitizenLastname, "Фамилия").Column("CITIZEN_LASTNAME");
            Property(x => x.CitizenFirstname, "Имя").Column("CITIZEN_FIRSTNAME");
            Property(x => x.CitizenGivenname, "Отчество").Column("CITIZEN_GIVENNAME");
            Property(x => x.CitizenBirthday, "Дата рождения").Column("CITIZEN_BIRTHDAY");
            Property(x => x.CitizenSnils, "СНИЛС").Column("CITIZEN_SNILS");
            Property(x => x.DocType, "Тип документа").Column("DOC_TYPE");
            Property(x => x.DocSerie, "Серия документа").Column("DOC_SERIE");
            Property(x => x.DocNumber, "Номер документа").Column("DOC_NUMBER");
            Property(x => x.DocIssueDate, "Дата выдачи документа").Column("DOC_ISSUEDATE");
            Property(x => x.RegionCode, "Регион запроса").Column("REGION_CODE");
            Property(x => x.LPlaceRegion, "Регион регистрации").Column("LPLACE_REGION");
            Property(x => x.LPlaceDistrict, "Район").Column("LPLACE_DISTRICT");
            Property(x => x.LPlaceCity, "Населенный пункт").Column("LPLACE_CITY");
            Property(x => x.LPlaceStreet, "Улица").Column("LPLACE_STREET");
            Property(x => x.LPlaceHouse, "Дом").Column("LPLACE_HOUSE");
            Property(x => x.LPlaceBuilding, "Корпус").Column("LPLACE_BUILDING");
            Property(x => x.LPlaceFlat, "Квартира").Column("LPLACE_FLAT");
            Property(x => x.RegStatus, "Действительность регистрации").Column("REG_STATUS");
            Property(x => x.DocCountry, "Страна").Column("DOC_COUNTRY");
            Property(x => x.Answer, "Ответ").Column("ANSWER");
            Property(x => x.TaskId, "TaskId").Column("TASK_ID");
        }
    }
}
