/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class StateDutyPetitionMap : BaseEntityMap<StateDutyPetition>
///     {
///         public StateDutyPetitionMap()
///             : base("CLW_STATE_DUTY_PETITION")
///         {
///             References(x => x.StateDuty, "STATE_DUTY_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PetitionToCourtType, "PETITION_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Привязка типа заявления к госпошлине"</summary>
    public class StateDutyPetitionMap : BaseEntityMap<StateDutyPetition>
    {
        
        public StateDutyPetitionMap() : 
                base("Привязка типа заявления к госпошлине", "CLW_STATE_DUTY_PETITION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.StateDuty, "Госпошлина").Column("STATE_DUTY_ID").NotNull().Fetch();
            Reference(x => x.PetitionToCourtType, "Тип заявления").Column("PETITION_TYPE_ID").NotNull().Fetch();
        }
    }
}
