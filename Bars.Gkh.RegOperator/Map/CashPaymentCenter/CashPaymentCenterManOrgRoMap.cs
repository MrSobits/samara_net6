/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using System;
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CashPaymentCenterManOrgRoMap : BaseImportableEntityMap<CashPaymentCenterManOrgRo>
///     {
///         public CashPaymentCenterManOrgRoMap()
///             : base("REGOP_CASHPAYM_CENTER_MAN_ORG_RO")
///         {
///             References(x => x.CashPaymentCenterManOrg, "CASHPAYM_CENTER_MAN_ORG_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.DateStart, "DATE_START", true, DateTime.Today);
///             Map(x => x.DateEnd, "DATE_END", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Дом обслуживаемой УК расчетно-кассового центра"</summary>
    public class CashPaymentCenterManOrgRoMap : BaseImportableEntityMap<CashPaymentCenterManOrgRo>
    {
        
        public CashPaymentCenterManOrgRoMap() : 
                base("Дом обслуживаемой УК расчетно-кассового центра", "REGOP_CASHPAYM_CENTER_MAN_ORG_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CashPaymentCenterManOrg, "Обслуживаемая УК расчетно-кассового центра").Column("CASHPAYM_CENTER_MAN_ORG_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Дом").Column("REAL_OBJ_ID").NotNull().Fetch();
            Property(x => x.DateStart, "Дата начала действия договора").Column("DATE_START").DefaultValue(DateTime.Today).NotNull();
            Property(x => x.DateEnd, "Дата начала действия договора").Column("DATE_END");
        }
    }
}
