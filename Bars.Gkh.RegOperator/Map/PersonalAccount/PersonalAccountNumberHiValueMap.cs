/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PersonalAccountNumberHiValueMap : BaseImportableEntityMap<PersonalAccountNumberHiValue>
///     {
///         public PersonalAccountNumberHiValueMap() : base("REGOP_PERS_ACC_NUM_VALUE")
///         {
///             Map(x => x.Value, "HVALUE", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Максимальное значение номера лицевого счета (для генерации короткого номера лс)"</summary>
    public class PersonalAccountNumberHiValueMap : BaseImportableEntityMap<PersonalAccountNumberHiValue>
    {
        
        public PersonalAccountNumberHiValueMap() : 
                base("Максимальное значение номера лицевого счета (для генерации короткого номера лс)", "REGOP_PERS_ACC_NUM_VALUE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Value, "Значение").Column("HVALUE").NotNull();
        }
    }
}
