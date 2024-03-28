namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class GISERPMap : BaseEntityMap<GISERP>
    {
        
        public GISERPMap() : 
                base("Обмен данными с ГИС ЕРП", "GJI_CH_GIS_ERP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Property(x => x.RequestDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.CarryoutEvents, "Мероприятия по контролю").Column("CARRYOUT_EVENTS");
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.InspectionName, "Наименование проверки").Column("NAME");
            Property(x => x.checkId, "Ид проверки").Column("CHECK_ID");
            Property(x => x.ERPAddressType, "Тип адреса субъекта проверки").Column("ADDRESS_TYPE");
            Property(x => x.ERPInspectionType, "Тип проверки").Column("INSPECTION_TYPE");
            Property(x => x.ERPNoticeType, "Способ уведомления").Column("NOTICE_TYPE");
            Property(x => x.ERPObjectType, "Тип объекта проведения проверки").Column("OBJECT_TYPE");
            Property(x => x.ERPReasonType, "Основание проведения проверки").Column("REASON_TYPE");
            Property(x => x.ERPRiskType, "Код категории риска").Column("RISK_TYPE");
            Property(x => x.KindKND, "Вид надзора ").Column("KND_TYPE");
            Property(x => x.GisErpRequestType, "Тип запроса в ЕРП").Column("REQUEST_TYPE");
            Property(x => x.OKATO, "ОКАТО").Column("OKATO");
            Property(x => x.RequestState, "Статус запроса").Column("REQUEST_STATE");
            Property(x => x.SubjectAddress, "Адрес субъекта проверки").Column("SUBJ_ADDRESS");
            Property(x => x.Goals, "Цели и задачи проверки").Column("GOALS");
            Property(x => x.ACT_DATE_CREATE, "ACT_DATE_CREATE").Column("ACT_DATE_CREATE");
            Property(x => x.DURATION_HOURS, "DURATION_HOURS").Column("DURATION_HOURS");
            Property(x => x.HasViolations, "HAS_VIOLATIONS").Column("HAS_VIOLATIONS");
            Property(x => x.NeedToUpdate, "NEED_TO_UPDATE").Column("NEED_TO_UPDATE");
            Property(x => x.REPRESENTATIVE_FULL_NAME, "REPRESENTATIVE_FULL_NAME").Column("REPRESENTATIVE_FULL_NAME");
            Property(x => x.REPRESENTATIVE_POSITION, "REPRESENTATIVE_POSITION").Column("REPRESENTATIVE_POSITION");
            Property(x => x.START_DATE, "START_DATE").Column("START_DATE");
            Property(x => x.INSPECTION_GUID, "INSPECTION_GUID").Column("INSPECTION_GUID");
            Property(x => x.INSPECTOR_GUID, "INSPECTOR_GUID").Column("INSPECTOR_GUID");
            Property(x => x.OBJECT_GUID, "OBJECT_GUID").Column("OBJECT_GUID");
            Property(x => x.RESULT_GUID, "RESULT_GUID").Column("RESULT_GUID");
            Property(x => x.ERPID, "ERPID").Column("ERPID");
            //
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").Fetch();
            Reference(x => x.Disposal, "Протокол").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.ProsecutorOffice, "Отдел прокуратуры").Column("PROSECUTOR_ID").NotNull().Fetch();
        }
    }
}
