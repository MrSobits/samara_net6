/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewBusinessActivityMap : PersistentObjectMap<ViewBusinessActivity>
///     {
///         public ViewBusinessActivityMap()
///             : base("VIEW_GJI_BUISNES_NOTIF")
///         {
///             Map(x => x.ContragentName, "CONTR_NAME");
///             Map(x => x.TypeKindActivity, "TYPE_KIND_ACTIVITY").Not.Nullable().CustomType<TypeKindActivity>();
///             Map(x => x.IncomingNotificationNum, "INCOMING_NUM");
///             Map(x => x.DateRegistration, "DATE_REGISTRATION");
///             Map(x => x.DateNotification, "DATE_NOTIFICATION");
///             Map(x => x.RegNum, "REG_NUM");
///             Map(x => x.IsOriginal, "IS_ORIGINAL").Not.Nullable();
///             Map(x => x.FileInfoId, "FILE_ID");
///             Map(x => x.ContragentInn, "INN");
///             Map(x => x.ContragentOgrn, "OGRN");
///             Map(x => x.ContragentMailingAddress, "MAILING_ADDRESS");
///             Map(x => x.OrgFormName, "ORG_FORM_NAME");
///             Map(x => x.MunicipalityName, "MUNICIPALITY_NAME");
///             Map(x => x.ServiceCount, "SERV_COUNT");
///             Map(x => x.Registered, "REGISTERED");
///             Map(x => x.MunicipalityId, "MUNICIPALITY_ID");
///             Map(x => x.ContragentId, "CONTR_ID");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBusinessActivity"</summary>
    public class ViewBusinessActivityMap : PersistentObjectMap<ViewBusinessActivity>
    {
        
        public ViewBusinessActivityMap() : 
                base("Bars.GkhGji.Entities.ViewBusinessActivity", "VIEW_GJI_BUISNES_NOTIF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ContragentName, "Наименование юр. лица").Column("CONTR_NAME");
            Property(x => x.TypeKindActivity, "Вид деятельности").Column("TYPE_KIND_ACTIVITY").NotNull();
            Property(x => x.IncomingNotificationNum, "Входящий номер уведомления").Column("INCOMING_NUM");
            Property(x => x.DateRegistration, "Дата регистрации").Column("DATE_REGISTRATION");
            Property(x => x.DateNotification, "Дата уведомления").Column("DATE_NOTIFICATION");
            Property(x => x.RegNum, "Регистрационный номер").Column("REG_NUM");
            Property(x => x.IsOriginal, "Оригинал").Column("IS_ORIGINAL").NotNull();
            Property(x => x.FileInfoId, "id Файла").Column("FILE_ID");
            Property(x => x.ContragentInn, "ИНН").Column("INN");
            Property(x => x.ContragentOgrn, "ОГРН").Column("OGRN");
            Property(x => x.ContragentMailingAddress, "Почтовый адрес").Column("MAILING_ADDRESS");
            Property(x => x.OrgFormName, "Организационно-правовая форма").Column("ORG_FORM_NAME");
            Property(x => x.MunicipalityName, "Муниципальное образование(string)").Column("MUNICIPALITY_NAME");
            Property(x => x.ServiceCount, "Количество услуг").Column("SERV_COUNT");
            Property(x => x.Registered, "Зарегистрировано").Column("REGISTERED");
            Property(x => x.MunicipalityId, "Муниципальное образование(id)").Column("MUNICIPALITY_ID");
            Property(x => x.ContragentId, "Контрагент(id)").Column("CONTR_ID");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
