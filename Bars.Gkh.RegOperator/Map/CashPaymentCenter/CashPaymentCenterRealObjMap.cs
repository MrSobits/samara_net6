/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class CashPaymentCenterRealObjMap : BaseImportableEntityMap<CashPaymentCenterRealObj>
///     {
///         public CashPaymentCenterRealObjMap()
///             : base("REGOP_CASHPAYM_CENTER_REAL_OBJ")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END", false);
/// 
///             References(x => x.CashPaymentCenter, "CASHPAYM_CENTER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом расчетно-кассового центра"</summary>
    public class CashPaymentCenterRealObjMap : BaseImportableEntityMap<CashPaymentCenterRealObj>
    {
        
        public CashPaymentCenterRealObjMap() : 
                base("Жилой дом расчетно-кассового центра", "REGOP_CASHPAYM_CENTER_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REAL_OBJ_ID").NotNull().Fetch();
            Reference(x => x.CashPaymentCenter, "Агент доставки").Column("CASHPAYM_CENTER_ID").NotNull().Fetch();
            Property(x => x.DateStart, "Дата начала действия договора").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата начала действия договора").Column("DATE_END");
        }
    }
}
