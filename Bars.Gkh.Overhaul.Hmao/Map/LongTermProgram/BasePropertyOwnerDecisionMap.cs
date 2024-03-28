/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class BasePropertyOwnerDecisionMap : BaseImportableEntityMap<BasePropertyOwnerDecision>
///     {
///         public BasePropertyOwnerDecisionMap()
///             : base("OVRHL_PROP_OWN_DECISION_BASE")
///         {
///             Map(x => x.MonthlyPayment, "MONTHLY_PAYMENT", true, 0);
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

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Базовая сущность решения собственников помещений МКД"</summary>
    public class BasePropertyOwnerDecisionMap : BaseImportableEntityMap<BasePropertyOwnerDecision>
    {
        
        public BasePropertyOwnerDecisionMap() : 
                base("Базовая сущность решения собственников помещений МКД", "OVRHL_PROP_OWN_DECISION_BASE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PropertyOwnerProtocol, "Протокол собственников помещений МКД").Column("PROP_OWNER_PROTOCOL_ID").Fetch();
            Property(x => x.MonthlyPayment, "Ежемесячный взнос на КР (руб./кв.м)").Column("MONTHLY_PAYMENT").NotNull();
            Reference(x => x.RealityObject, "Объект долгосрочной программы").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Property(x => x.PropertyOwnerDecisionType, "Наименование решения").Column("DECISION_TYPE").NotNull();
            Property(x => x.MethodFormFund, "Способ формирования фонда").Column("METHOD_FORM_FUND");
            Property(x => x.MoOrganizationForm, "Организационно-правовая форма УО").Column("MO_ORG_FORM").NotNull();
        }
    }
}
