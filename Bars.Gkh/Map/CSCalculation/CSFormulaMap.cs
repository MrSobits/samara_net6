namespace Bars.Gkh.Map
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Формулы расчета платы ЖКУ"</summary>
    public class CSFormulaMap : BaseEntityMap<CSFormula>
    {
        
        public CSFormulaMap() : 
                base("Формула", "GKH_CS_FORMULA")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            Property(x => x.Formula, "Формула расчтеа пошлины").Column("FORMULA").Length(1000);
            Property(x => x.FormulaParameters, "Параметры формулы").Column("FORMULA_PARAMS");
        }
    }

    public class StateDutyNHibernateMapping : ClassMapping<CSFormula>
    {
        public StateDutyNHibernateMapping()
        {
            Property(x => x.FormulaParameters,
                m =>
                {
                    m.Type<BinaryJsonType<List<FormulaParameterMeta>>>();
                });
        }
    }
}
