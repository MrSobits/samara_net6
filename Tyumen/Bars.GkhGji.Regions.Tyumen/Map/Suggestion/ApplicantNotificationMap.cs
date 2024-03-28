/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tyumen.Map.Suggestion
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tyumen.Entities.Suggestion;
/// 
///     /// <summary>
///     /// Маппинг уведомления для заявителя
///     /// </summary>
///     public class ApplicantNotificationMap : BaseEntityMap<ApplicantNotification>
///     {
///         public ApplicantNotificationMap() : base("GKH_APPLICANT_NOTIFY")
///         {
///             Map(x => x.Code, "CODE").Not.Nullable();
///             Map(x => x.EmailSubject, "EMAIL_SUBJECT").Not.Nullable();
///             Map(x => x.EmailTemplate, "EMAIL_TEMPLATE").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tyumen.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tyumen.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tyumen.Entities.Suggestion.ApplicantNotification"</summary>
    public class ApplicantNotificationMap : BaseEntityMap<ApplicantNotification>
    {
        
        public ApplicantNotificationMap() : 
                base("Bars.GkhGji.Regions.Tyumen.Entities.Suggestion.ApplicantNotification", "GKH_APPLICANT_NOTIFY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Code").Column("CODE").NotNull();
            Property(x => x.EmailSubject, "EmailSubject").Column("EMAIL_SUBJECT").NotNull();
            Property(x => x.EmailTemplate, "EmailTemplate").Column("EMAIL_TEMPLATE").NotNull();
            Reference(x => x.State, "State").Column("STATE_ID").NotNull();
        }
    }
}
