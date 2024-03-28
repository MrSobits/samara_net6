/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Обслуживающая организация"
///     /// </summary>
///     public class PaymentAgentMap : BaseImportableEntityMap<PaymentAgent>
///     {
///         public PaymentAgentMap()
///             : base("GKH_PAYMENT_AGENT")
///         {
///             Map(x => x.Code, "CODE", true, 50);
///             Map(x => x.PenaltyContractId, "PENALTY_CONTRACT_ID", false, 10);
///             Map(x => x.SumContractId, "SUM_CONTRACT_ID", false, 10);
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Платежный агент"</summary>
    public class PaymentAgentMap : BaseImportableEntityMap<PaymentAgent>
    {
        
        public PaymentAgentMap() : 
                base("Платежный агент", "GKH_PAYMENT_AGENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.Code, "Код (Идентификатор) платежного агента").Column("CODE").Length(50).NotNull();
            Property(x => x.SumContractId, "Id договора загрузки суммы (используется для импорта)").Column("SUM_CONTRACT_ID").Length(10);
            Property(x => x.PenaltyContractId, "Id договора загрузки пени (используется для импорта)").Column("PENALTY_CONTRACT_ID").Length(10);
        }
    }
}
