namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVPropertyTypeMap : BaseEntityMap<SMEVPropertyType>
    {
        
        public SMEVPropertyTypeMap() : 
                base("Запрос к ВС ЕГРИП", "GJI_CH_SMEV_PROPERTY_TYPE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "МКД").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.Room, "Room").Column("ROOM_ID").Fetch();
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.PublicPropertyLevel, "PublicPropertyLevel").Column("PROP_LVL").NotNull();
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.CadastralNumber, "Кадастровый номер").Column("CADASTRAL_NUMBER");
            Property(x => x.Attr1, "Attr1").Column("ATTR1");
            Property(x => x.Attr2, "Attr2").Column("ATTR2");
            Property(x => x.Attr3, "Attr3").Column("ATTR3");
            Property(x => x.Attr4, "Attr4").Column("ATTR4");
            Property(x => x.Attr5, "Attr5").Column("ATTR5");
        }
    }
}
