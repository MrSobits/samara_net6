namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVRedevelopmentMap : BaseEntityMap<SMEVRedevelopment>
    {
        
        public SMEVRedevelopmentMap() : 
                base("Запрос к ВС ", "GJI_CH_SMEV_REDEVELOPMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
            Reference(x => x.AnswerFile, "Файл").Column("ANS_FILE_ID").Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Reference(x => x.RealityObject, "RO").Column("RO_ID").Fetch();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.ActDate, "Дата акта").Column("ACT_DATE");
            Property(x => x.ActNum, "Номер акта").Column("ACT_NUM");
            Property(x => x.Cadastral, "Кадастровый").Column("CADASTRAL");
            Property(x => x.ObjectName, "Имя объекта").Column("OBJECT_NAME");
            Property(x => x.GovermentName, "Имя органа").Column("GOV_NAME");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");

        }
    }
}
