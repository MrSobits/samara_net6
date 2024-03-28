/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Уведомление о начале предпринимательской деятельности"
///     /// </summary>
///     public class BusinessActivityMap : BaseGkhEntityMap<BusinessActivity>
///     {
///         public BusinessActivityMap() : base("GJI_BUISNES_NOTIF")
///         {
///             Map(x => x.TypeKindActivity, "TYPE_KIND_ACTIVITY").Not.Nullable().CustomType<TypeKindActivity>();
///             Map(x => x.IncomingNotificationNum, "INCOMING_NUM").Length(300);
///             Map(x => x.DateBegin, "DATE_BEGIN");
///             Map(x => x.DateRegistration, "DATE_REGISTRATION");
///             Map(x => x.DateNotification, "DATE_NOTIFICATION");
///             Map(x => x.IsNotBuisnes, "IS_NOT_BUISNES").Not.Nullable();
///             Map(x => x.AcceptedOrganization, "ACCEPTED_ORGANIZATION").Length(300);
///             Map(x => x.RegNum, "REG_NUM").Length(50);
///             Map(x => x.IsOriginal, "IS_ORIGINAL").Not.Nullable();
///             Map(x => x.RegNumber, "REG_NUMBER");
///             Map(x => x.Registered, "REGISTERED");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///             References(x => x.Contragent, "CONTRAGENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.File, "FILE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Уведомление о начале предпринимательской деятельности"</summary>
    public class BusinessActivityMap : BaseEntityMap<BusinessActivity>
    {
        
        public BusinessActivityMap() : 
                base("Уведомление о начале предпринимательской деятельности", "GJI_BUISNES_NOTIF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeKindActivity, "Вид деятельности").Column("TYPE_KIND_ACTIVITY").NotNull();
            Property(x => x.IncomingNotificationNum, "Входящий номер уведомления").Column("INCOMING_NUM").Length(300);
            Property(x => x.DateBegin, "Дата начала деятельности").Column("DATE_BEGIN");
            Property(x => x.DateRegistration, "Дата регистрации").Column("DATE_REGISTRATION");
            Property(x => x.DateNotification, "Дата уведомления").Column("DATE_NOTIFICATION");
            Property(x => x.IsNotBuisnes, "Не осуществляет предпринимательскую деятельность").Column("IS_NOT_BUISNES").NotNull();
            Property(x => x.AcceptedOrganization, "Орган принявший уведомление").Column("ACCEPTED_ORGANIZATION").Length(300);
            Property(x => x.RegNum, "Регистрационный номер(string)").Column("REG_NUM").Length(50);
            Property(x => x.IsOriginal, "Оригинал").Column("IS_ORIGINAL").NotNull();
            Property(x => x.RegNumber, "Регистрационный номер(int)").Column("REG_NUMBER");
            Property(x => x.Registered, "Зарегистрировано").Column("REGISTERED");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
