/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.Gkh.Map;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Услуги оказываемые юридическим лицом"
///     /// </summary>
///     public class ServiceJuridicalGjiMap : BaseGkhEntityMap<ServiceJuridicalGji>
///     {
///         public ServiceJuridicalGjiMap()
///             : base("GJI_DICT_SERV_JURID")
///         {
///             References(x => x.BusinessActivityNotif, "BUISNES_NOTIF_ID").Not.Nullable().Fetch.Join();
///             References(x => x.KindWorkNotif, "KIND_WORK_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Услуги оказываемые юридическим лицом"</summary>
    public class ServiceJuridicalGjiMap : BaseEntityMap<ServiceJuridicalGji>
    {
        
        public ServiceJuridicalGjiMap() : 
                base("Услуги оказываемые юридическим лицом", "GJI_DICT_SERV_JURID")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.BusinessActivityNotif, "Уведомление о начале предпринимательской деятельности").Column("BUISNES_NOTIF_ID").NotNull().Fetch();
            Reference(x => x.KindWorkNotif, "Вид работ уведомлений").Column("KIND_WORK_ID").Fetch();
        }
    }
}
