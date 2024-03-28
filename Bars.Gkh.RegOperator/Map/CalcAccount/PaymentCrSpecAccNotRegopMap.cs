/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class PaymentCrSpecAccNotRegopMap : BaseImportableEntityMap<PaymentCrSpecAccNotRegop>
///     {
///         public PaymentCrSpecAccNotRegopMap()
///             : base("PAYM_CR_SPECACC_NOTREGOP")
///         {
///             Map(x => x.InputDate, "INPUT_DATE");
///             Map(x => x.AmountIncome, "AMOUNT_INCOME");
///             Map(x => x.EndYearBalance, "ENDYEAR_BALANCE");
/// 
///             References(x => x.RealityObject, "REALOBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Period, "PERIOD_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.File, "FILE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Взнос на капремонт дома, у которого в протоколе решений действующее решение о способе формирования фонда на спец.счете и владелец НЕ регоператор"</summary>
    public class PaymentCrSpecAccNotRegopMap : BaseImportableEntityMap<PaymentCrSpecAccNotRegop>
    {
        
        public PaymentCrSpecAccNotRegopMap() : 
                base("Взнос на капремонт дома, у которого в протоколе решений действующее решение о спо" +
                        "собе формирования фонда на спец.счете и владелец НЕ регоператор", "PAYM_CR_SPECACC_NOTREGOP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALOBJ_ID").NotNull().Fetch();
            Reference(x => x.Period, "Период начислений").Column("PERIOD_ID").NotNull().Fetch();
            Property(x => x.InputDate, "Дата ввода").Column("INPUT_DATE");
            Property(x => x.AmountIncome, "Сумма поступления").Column("AMOUNT_INCOME");
            Property(x => x.EndYearBalance, "Остаток на конец года").Column("ENDYEAR_BALANCE");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
