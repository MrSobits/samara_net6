namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVOwnershipPropertyMap : BaseEntityMap<SMEVOwnershipProperty>
    {
        
        public SMEVOwnershipPropertyMap() : 
                base("Запрос к ВС помещения", "GJI_SMEV_OW_PROPERTY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.AnswerFile, "Файл").Column("ANS_FILE_ID").Fetch();
            Reference(x => x.AttachmentFile, "Файл").Column("ATT_FILE_ID").Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Reference(x => x.RealityObject, "МКД").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.Room, "Room").Column("ROOM_ID").Fetch();
            Property(x => x.CadasterNumber, "CadasterNumber").Column("CADASTRAL");
            Property(x => x.QueryType, "тип").Column("QUERY_TYPE");
            Property(x => x.PublicPropertyLevel, "").Column("PROP_LEVEL");
            Property(x => x.RegisterNumber, "").Column("REG_NUM");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
