/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealityObjectSupplierAccountMap : BaseImportableEntityMap<RealityObjectSupplierAccount>
///     {
///         public RealityObjectSupplierAccountMap() : base("REGOP_RO_SUPP_ACC")
///         {
///             Map(x => x.CloseDate, "CLOSE_DATE");
///             Map(x => x.OpenDate, "OPEN_DATE");
///             Map(x => x.AccountNumber, "ACC_NUM");
/// 
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Счет расчета с поставщиками"</summary>
    public class RealityObjectSupplierAccountMap : BaseImportableEntityMap<RealityObjectSupplierAccount>
    {
        
        public RealityObjectSupplierAccountMap() : 
                base("Счет расчета с поставщиками", "REGOP_RO_SUPP_ACC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AccountNumber, "Номер счета").Column("ACC_NUM").Length(250);
            Property(x => x.OpenDate, "Дата открытия").Column("OPEN_DATE");
            Property(x => x.CloseDate, "Дата закрытия").Column("CLOSE_DATE");
            Reference(x => x.RealityObject, "Дом - владелец счета").Column("RO_ID").NotNull().Fetch();
        }
    }
}
