/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class CashPaymentCenterPersAccMap : BaseImportableEntityMap<CashPaymentCenterPersAcc>
///     {
///         public CashPaymentCenterPersAccMap()
///             : base("REGOP_CASHPAY_PERS_ACC")
///         {
///             Map(x => x.DateStart, "DATE_START", true);
///             Map(x => x.DateEnd, "DATE_END", false);
/// 
///             References(x => x.CashPaymentCenter, "CASHPAYM_CENTER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PersonalAccount, "PERS_ACC_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Лицевой счет расчетно-кассового центра"</summary>
    public class CashPaymentCenterPersAccMap : BaseImportableEntityMap<CashPaymentCenterPersAcc>
    {
        
        public CashPaymentCenterPersAccMap() : 
                base("Лицевой счет расчетно-кассового центра", "REGOP_CASHPAY_PERS_ACC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PersonalAccount, "Лицевой счет").Column("PERS_ACC_ID").NotNull().Fetch();
            Reference(x => x.CashPaymentCenter, "Агент доставки").Column("CASHPAYM_CENTER_ID").NotNull().Fetch();
            Property(x => x.DateStart, "Дата начала действия договора").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата окончания действия договора").Column("DATE_END");
        }
    }
}
