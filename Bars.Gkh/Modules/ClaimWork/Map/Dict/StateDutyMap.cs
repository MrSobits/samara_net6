/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map.Dict
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using Bars.B4.DataAccess.UserTypes;
///     using Bars.Gkh.Formulas;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class StateDutyMap : BaseEntityMap<StateDuty>
///     {
///         public StateDutyMap()
///             : base("CLW_DICT_STATE_DUTY")
///         {
///             Map(x => x.CourtType, "COURT_TYPE", true);
///             Map(x => x.Formula, "FORMULA", false, 1000);
///             Property(x => x.FormulaParameters,
///                 m =>
///                 {
///                     m.Column("FORMULA_PARAMS");
///                     m.Type<BinaryJsonType<List<FormulaParameterMeta>>>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using System.Collections.Generic;
    
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Госпошлина"</summary>
    public class StateDutyMap : BaseEntityMap<StateDuty>
    {
        
        public StateDutyMap() : 
                base("Госпошлина", "CLW_DICT_STATE_DUTY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CourtType, "Тип суда").Column("COURT_TYPE").NotNull();
            Property(x => x.Formula, "Формула расчтеа пошлины").Column("FORMULA").Length(1000);
            Property(x => x.FormulaParameters, "Параметры формулы").Column("FORMULA_PARAMS");
        }
    }

    public class StateDutyNHibernateMapping : ClassMapping<StateDuty>
    {
        public StateDutyNHibernateMapping()
        {
            Property(x => x.FormulaParameters,
                m =>
                {
                    m.Type<ImprovedBinaryJsonType<List<FormulaParameterMeta>>>();
                });
        }
    }
}
