namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class ViewERKNMMap : PersistentObjectMap<ViewERKNM>
    {
        
        public ViewERKNMMap() : 
                base("Представление \"Запрос в ЕРКНМ\"", "VIEW_GJI_CH_GIS_ERKNM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ObjectCreateDate, "Дата создания").Column("OBJECT_CREATE_DATE");
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Property(x => x.RequestDate, "Дата запроса").Column("REQ_DATE");
            Property(x => x.SendTime, "Время отправки").Column("SEND_TIME");
            Property(x => x.AnswerDate, "Дата ответа").Column("ANS_DATE");
            Property(x => x.TimeSpent, "Затраченное время").Column("TIME_SPENT");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.KindKND, "Вид надзора ").Column("KND_TYPE");
            Property(x => x.GisErpRequestType, "Тип запроса в ЕРП").Column("REQUEST_TYPE");
            Property(x => x.RequestState, "Статус запроса").Column("REQUEST_STATE");
            Property(x => x.ERPID, "ERPID").Column("ERPID");
            Property(x => x.ERKNMDocumentType, "ERKNMDocumentType").Column("DOC_TYPE");
            Property(x => x.Inspector, "Инспектор").Column("INSPECTOR");
            Property(x => x.Disposal, "Протокол").Column("DISPOSAL");
        }
    }
}
