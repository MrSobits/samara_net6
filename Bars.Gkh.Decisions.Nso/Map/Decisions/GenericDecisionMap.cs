/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map.Decisions
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class GenericDecisionMap : BaseImportableEntityMap<GenericDecision>
///     {
///         public GenericDecisionMap() : base("GKH_GENERIC_DECISION")
///         {
///             Map(x => x.DecisionCode, "DECISION_CODE", true, 100);
///             Map(x => x.IsActual, "IS_ACTUAL", true);
///             Map(x => x.StartDate, "START_DATE", true);
///             Map(x => x.JsonObject, "JSON_OBJECT", false, 20000);
/// 
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Решение, принятое по протоколу"</summary>
    public class GenericDecisionMap : BaseImportableEntityMap<GenericDecision>
    {
        
        public GenericDecisionMap() : 
                base("Решение, принятое по протоколу", "GKH_GENERIC_DECISION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DecisionCode, "Код решения").Column("DECISION_CODE").Length(100).NotNull();
            Property(x => x.IsActual, "Является актуальным").Column("IS_ACTUAL").NotNull();
            Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            Property(x => x.StartDate, "Дата ввода в действие").Column("START_DATE").NotNull();
            Property(x => x.JsonObject, "Храним значение решения").Column("JSON_OBJECT").Length(20000);
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
