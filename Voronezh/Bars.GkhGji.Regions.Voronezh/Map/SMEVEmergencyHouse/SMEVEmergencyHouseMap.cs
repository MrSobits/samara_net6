namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVEmergencyHouse;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEmergencyHouseMap : BaseEntityMap<SMEVEmergencyHouse>
    {
        
        public SMEVEmergencyHouseMap() : 
                base("Запрос к ВС ", "GJI_CH_SMEV_EMERGENCY_HOUSE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.AnswerFile, "Файл").Column("ANS_FILE_ID").Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Reference(x => x.RealityObject, "RO").Column("RO_ID").Fetch();
            Reference(x => x.Room, "ROOM").Column("ROOM_ID").Fetch();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.EmergencyTypeSGIO, "Тип запроса").Column("TYPE_REQUEST");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
