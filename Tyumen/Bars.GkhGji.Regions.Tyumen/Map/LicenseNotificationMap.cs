namespace Bars.GkhGji.Regions.Tyumen.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tyumen.Entities;


    /// <summary>Маппинг для "Определение акта проверки ГЖИ"</summary>
    public class LicenseNotificationMap : BaseEntityMap<LicenseNotification>
    {
        
        public LicenseNotificationMap() : 
                base("Извещение об исключении из реестра лицензий", "GJI_LICENSE_NOTIFICATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Comment, "Комментарий").Column("COMMENT");
            Reference(x => x.LocalGovernment, "Орган местного самоуправления").Column("LOCAL_GOVERNMENT_ID").Fetch();
            Reference(x => x.Contragent, "Действующая УК").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.ManagingOrgRealityObject, "Основание управления домом").Column("MORG_RO_ID").Fetch();
            Property(x => x.MoDateStart, "Дата начала управления дома, действующей УК").Column("MO_DATE_START");
            Property(x => x.NoticeOMSSendDate, "Дата направления извещения в орган местного самоуправления").Column("NOTICE_OMS_SEND_DATE");
            Property(x => x.NoticeResivedDate, "Дата получения").Column("NOTICE_RECIVED_DATE");
            Property(x => x.OMSNoticeResult, "Результат").Column("OMS_NOTISE_RESULT").NotNull();
            Property(x => x.OMSNoticeResultDate, "Дата документа о результате решения органа местного самоуправления").Column("OMS_NOTISE_RESULT_DATE");
            Property(x => x.OMSNoticeResultNumber, "Номер документа о результате решения органа местного самоуправления").Column("OMS_NOTISE_RESULT_NUMBER");
            Property(x => x.RegistredNumber, "Номер регистрации").Column("REGISTRED_NUMBER");
            Property(x => x.LicenseNotificationNumber, "Номер извещения").Column("LICENSE_NOTIFICATION_NUMBER");
        }
    }
}
