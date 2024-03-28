/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using Bars.Gkh.Overhaul.Entities;
///     using Bars.Gkh.Overhaul.Enum;
/// 
///     public class PaymentSizeCrMap : B4.DataAccess.BaseImportableEntityMap<PaymentSizeCr>
///     {
///         public PaymentSizeCrMap()
///             : base("OVRHL_DICT_PAYSIZE")
///         {
///             Map(x => x.TypeIndicator, "TYPE_INDICATOR").Not.Nullable().CustomType<TypeIndicator>();
///             Map(x => x.PaymentSize, "PAYMENT_SIZE");
///             Map(x => x.DateStartPeriod, "DATE_START_PERIOD").Not.Nullable();
///             Map(x => x.DateEndPeriod, "DATE_END_PERIOD");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Entities.PaymentSizeCr"</summary>
    public class PaymentSizeCrMap : BaseImportableEntityMap<PaymentSizeCr>
    {
        
        public PaymentSizeCrMap() : 
                base("Bars.Gkh.Overhaul.Entities.PaymentSizeCr", "OVRHL_DICT_PAYSIZE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeIndicator, "Тип показателя").Column("TYPE_INDICATOR").NotNull();
            Property(x => x.PaymentSize, "Размер взноса").Column("PAYMENT_SIZE");
            Property(x => x.DateStartPeriod, "Дата начала периода").Column("DATE_START_PERIOD").NotNull();
            Property(x => x.DateEndPeriod, "Дата окончания периода").Column("DATE_END_PERIOD");
        }
    }
}
