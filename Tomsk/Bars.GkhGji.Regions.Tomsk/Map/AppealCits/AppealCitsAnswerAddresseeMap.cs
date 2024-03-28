/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Адресат ответа на обращение"
///     /// </summary>
///     public class AppealCitsAnswerAddresseeMap : BaseEntityMap<AppealCitsAnswerAddressee>
///     {
///         public AppealCitsAnswerAddresseeMap()
///             : base("GJI_APPCIT_ANS_ADDR")
///         {
///             References(x => x.Addressee, "REVENUE_SOURCE_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Answer, "ANSWER_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.AppealCitsAnswerAddressee"</summary>
    public class AppealCitsAnswerAddresseeMap : BaseEntityMap<AppealCitsAnswerAddressee>
    {
        
        public AppealCitsAnswerAddresseeMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.AppealCitsAnswerAddressee", "GJI_APPCIT_ANS_ADDR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Addressee, "Addressee").Column("REVENUE_SOURCE_ID").NotNull().Fetch();
            Reference(x => x.Answer, "Answer").Column("ANSWER_ID").NotNull().Fetch();
        }
    }
}
