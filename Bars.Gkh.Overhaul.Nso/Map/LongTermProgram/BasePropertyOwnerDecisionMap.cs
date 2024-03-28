/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class BasePropertyOwnerDecisionMap : BaseEntityMap<BasePropertyOwnerDecision>
///     {
///         public BasePropertyOwnerDecisionMap()
///             : base("OVRHL_PROP_OWN_DECISION_BASE")
///         {
///             Map(x => x.MonthlyPayment, "MONTHLY_PAYMENT", true, (object) 0);
///             Map(x => x.PropertyOwnerDecisionType, "DECISION_TYPE", true);
///             Map(x => x.MethodFormFund, "METHOD_FORM_FUND");
///             Map(x => x.MoOrganizationForm, "MO_ORG_FORM", true);
/// 
///             References(x => x.PropertyOwnerProtocol, "PROP_OWNER_PROTOCOL_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using System;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.BasePropertyOwnerDecision"</summary>
    public class BasePropertyOwnerDecisionMap : BaseEntityMap<BasePropertyOwnerDecision>
    {
        
        public BasePropertyOwnerDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.BasePropertyOwnerDecision", "OVRHL_PROP_OWN_DECISION_BASE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.PropertyOwnerProtocol, "PropertyOwnerProtocol").Column("PROP_OWNER_PROTOCOL_ID").Fetch();
            Property(x => x.MonthlyPayment, "MonthlyPayment").Column("MONTHLY_PAYMENT").DefaultValue(0m).NotNull();
            Property(x => x.PropertyOwnerDecisionType, "PropertyOwnerDecisionType").Column("DECISION_TYPE").NotNull();
            Property(x => x.MethodFormFund, "MethodFormFund").Column("METHOD_FORM_FUND");
            Property(x => x.MoOrganizationForm, "MoOrganizationForm").Column("MO_ORG_FORM").NotNull();
        }
    }
}
