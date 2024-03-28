/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using System;
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class CashPaymentCenterManOrgMap : BaseImportableEntityMap<CashPaymentCenterManOrg>
///     {
///         public CashPaymentCenterManOrgMap()
///             : base("REGOP_CASHPAYM_CENTER_MAN_ORG")
///         {
///             References(x => x.CashPaymentCenter, "CASHPAYM_CENTER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.ManOrg, "MAN_ORG_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.NumberContract, "NUMBER_CONTRACT");
///             Map(x => x.DateContract, "DATE_CONTRACT", false);
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

    /// <summary>Маппинг для "Обслуживаемая УК расчетно-кассового центра"</summary>
    public class CashPaymentCenterManOrgMap : BaseImportableEntityMap<CashPaymentCenterManOrg>
    {
        
        public CashPaymentCenterManOrgMap() : 
                base("Обслуживаемая УК расчетно-кассового центра", "REGOP_CASHPAYM_CENTER_MAN_ORG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CashPaymentCenter, "РКЦ").Column("CASHPAYM_CENTER_ID").NotNull().Fetch();
            Reference(x => x.ManOrg, "УК").Column("MAN_ORG_ID").NotNull().Fetch();
            Property(x => x.NumberContract, "Номер договора").Column("NUMBER_CONTRACT").Length(250);
            Property(x => x.DateContract, "Дата договора").Column("DATE_CONTRACT");
            Property(x => x.DateStart, "Дата начала действия договора").Column("DATE_START").DefaultValue(DateTime.Today).NotNull();
            Property(x => x.DateEnd, "Дата начала действия договора").Column("DATE_END");
        }
    }
}
