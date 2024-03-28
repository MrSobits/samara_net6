/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Overhaul.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Расценки по работам"
///     /// </summary>
///     public class WorkPriceMap : BaseImportableEntityMap<WorkPrice>
///     {
///         public WorkPriceMap()
///             : base("OVRHL_DICT_WORK_PRICE")
///         {
///             Map(x => x.NormativeCost, "NORMATIVE_COST");
///             Map(x => x.SquareMeterCost, "SQUARE_METER_COST");
///             Map(x => x.Year, "YEAR");
/// 
///             References(x => x.Job, "JOB_ID").Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join(); ;
///             References(x => x.CapitalGroup, "CAPITAL_GROUP_ID").Fetch.Join(); ;
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Расценка по работе"</summary>
    public class WorkPriceMap : BaseImportableEntityMap<WorkPrice>
    {
        
        public WorkPriceMap() : 
                base("Расценка по работе", "OVRHL_DICT_WORK_PRICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NormativeCost, "Нормативная стоимость").Column("NORMATIVE_COST");
            Property(x => x.SquareMeterCost, "Стоимость по квадратному метру жилой площади").Column("SQUARE_METER_COST");
            Property(x => x.Year, "Год").Column("YEAR");
            Reference(x => x.Job, "Работа").Column("JOB_ID").Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
            Reference(x => x.CapitalGroup, "Группа капитальности").Column("CAPITAL_GROUP_ID").Fetch();
        }
    }
}
