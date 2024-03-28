/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CalcAccountRealityObjectMap : BaseImportableEntityMap<CalcAccountRealityObject>
///     {
///         public CalcAccountRealityObjectMap() : base("REGOP_CALC_ACC_RO")
///         {
///             References(x => x.Account, "ACCOUNT_ID");
///             References(x => x.RealityObject, "RO_ID");
/// 
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом расчетного счета"</summary>
    public class CalcAccountRealityObjectMap : BaseEntityMap<CalcAccountRealityObject>
    {
        
        public CalcAccountRealityObjectMap() : 
                base("Жилой дом расчетного счета", "REGOP_CALC_ACC_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Расчетный счет").Column("ACCOUNT_ID");
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
        }
    }
}
