namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVChangePremisesStateMap : BaseEntityMap<SMEVChangePremisesState>
    {
        
        public SMEVChangePremisesStateMap() : 
                base("Запрос к ВС СГИО о переводe статуса помещения", "GJI_CH_SMEV_CHANGE_PREM_STATE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Reference(x => x.AnswerFile, "Файл").Column("ANS_FILE_ID").Fetch();
            Property(x => x.ChangePremisesType, "").Column("CHANGE_PREMISES_TYPE");
            Reference(x => x.RealityObject, "").Column("RO_ID").Fetch();
            Reference(x => x.Room, "").Column("ROOM_ID").Fetch();
            Property(x => x.CadastralNumber, "Кадастровый").Column("CADASTRAL_NUMBER");
            Property(x => x.DeclarantType, "Тип заявителя").Column("DECLARANT_TYPE");
            Property(x => x.DeclarantName, "Имя заявителя").Column("DECLARANT_NAME");
            Property(x => x.DeclarantAddress, "Адрес заявителя").Column("DECLARANT_ADDRESS");
            Property(x => x.Department, "Орган местного самоуправления, осуществляющий перевод помещения").Column("DEPARTMENT");
            Property(x => x.Area, "Площадь помещения").Column("AREA");
            Property(x => x.City, "Город").Column("CITY");
            Property(x => x.Street, "Улица").Column("STREET");
            Property(x => x.House, "Дом").Column("HOUSE");
            Property(x => x.Block, "Корпус").Column("BLOCK");
            Property(x => x.Apartment, "Квартира").Column("APARTMENT");
            Property(x => x.RoomType, "Тип помещения").Column("ROOM_TYPE");
            Property(x => x.Appointment, "Цель использования помещения").Column("APPOINTMENT");
            Property(x => x.ActNumber, "Номер акта").Column("ACT_NUMBER");
            Property(x => x.ActName, "Акт").Column("ACT_NAME");
            Property(x => x.ActDate, "Дата акта").Column("ACT_DATE");
            Property(x => x.OldPremisesType, "Старый тип помещения").Column("OLD_PREM_TYPE");
            Property(x => x.NewPremisesType, "Новый тип помещения").Column("NEW_PREM_TYPE");
            Property(x => x.ConditionTransfer, "Условия перевода").Column("CONDITION_TRANSFER");
            Property(x => x.ResponsibleName, "Наименование ответственного лица").Column("RESPONSIBLE_NAME");
            Property(x => x.ResponsiblePost, "Должность ответственного лица").Column("RESPONSIBLE_POST");
            Property(x => x.ResponsibleDate, " Дата уведомления").Column("RESPONSIBLE_DATE");
        }
    }
}
