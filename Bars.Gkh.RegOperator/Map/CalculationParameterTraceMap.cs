/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities.PersonalAccount;
/// 
///     public class CalculationParameterTraceMap : BaseImportableEntityMap<CalculationParameterTrace>
///     {
///         public CalculationParameterTraceMap() : base("REGOP_CALC_PARAM_TRACE")
///         {
///             //Id(x => x.Id, m =>
///             //{
///             //    m.Column("ID");
///             //    m.Generator(Generators.Native);
///             //});
///             Map(x => x.CalculationGuid, "CALC_GUID", true);
///             Map(x => x.CalculationType, "CALC_TYPE");
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END");
///             Property(x => x.ParameterValues, m =>
///             {
///                 m.Column("PARAM_VALUES");
///                 m.NotNullable(false);
///                 m.Type<ImprovedJsonSerializedType<Dictionary<string, object>>>();
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using System.Collections.Generic;
    
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Трассировка параметров расчетов ЛС"</summary>
    public class CalculationParameterTraceMap : BaseEntityMap<CalculationParameterTrace>
    {
        
        public CalculationParameterTraceMap() : 
                base("Трассировка параметров расчетов ЛС", "REGOP_CALC_PARAM_TRACE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CalculationType, "Тип расчета").Column("CALC_TYPE");
            Property(x => x.ParameterValues, "Словарь значений параметров на шаг расчета").Column("PARAM_VALUES");
            Property(x => x.DateStart, "Дата начала действия параметров").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата окончания действия параметров").Column("DATE_END");
            Property(x => x.CalculationGuid, "Гуид связи с неподтвержденным начислением и боевым").Column("CALC_GUID").Length(250).NotNull();
            Reference(x => x.ChargePeriod, "Период начисления").Column("PERIOD_ID").NotNull().Fetch();
        }
    }

    public class CalculationParameterTraceNHibernateMapping : ClassMapping<CalculationParameterTrace>
    {
        public CalculationParameterTraceNHibernateMapping()
        {
            Property(x => x.ParameterValues, m =>
            {
                m.Type<ImprovedJsonSerializedType<Dictionary<string, object>>>();
            });
        }
    }
}
