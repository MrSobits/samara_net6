/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities;
/// 
///     public class RefManagingOrganizationMap : BaseEntityMap<RefManagingOrganization>
///     {
///         public RefManagingOrganizationMap()
///             : base("RFRM_MANAGING_ORG")
///         {
///             this.Map(x => x.Inn, "INN", true);
///             this.Map(x => x.RequestDate, "REQUEST_DATE", true);
///             this.Map(x => x.ProcessDate, "PROCESS_DATE", false);
///             this.Map(x => x.RequestStatus, "REQUEST_STATUS", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities;
    
    
    /// <summary>Маппинг для "Синхронизируемая управляющая организация"</summary>
    public class RefManagingOrganizationMap : BaseEntityMap<RefManagingOrganization>
    {
        
        public RefManagingOrganizationMap() : 
                base("Синхронизируемая управляющая организация", "RFRM_MANAGING_ORG")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Inn, "ИНН УО").Column("INN").Length(250).NotNull();
            Property(x => x.RequestDate, "Дата запроса").Column("REQUEST_DATE").NotNull();
            Property(x => x.ProcessDate, "Дата обработки заявки").Column("PROCESS_DATE");
            Property(x => x.RequestStatus, "Статус запроса").Column("REQUEST_STATUS").NotNull();
        }
    }
}
