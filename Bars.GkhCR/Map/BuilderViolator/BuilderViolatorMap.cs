/// <mapping-converter-backup>
/// using Bars.GkhCr.Entities;
/// 
/// namespace Bars.GkhCr.Modules.ClaimWork.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     public class BuilderViolatorMap : BaseEntityMap<BuilderViolator>
///     {
///         public BuilderViolatorMap()
///             : base("CR_BUILDER_VIOLATOR")
///         {
///             References(x => x.BuildContract, "CONTRACT_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.CreationType, "TYPE_CREATION", true, (object)10);
///             Map(x => x.StartingDate, "START_DATE");
///             Map(x => x.CountDaysDelay, "COUNT_DAYS_DELAY");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; 
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Основание претензионно исковой работы для Договоров Подряда"</summary>
    public class BuilderViolatorMap : BaseEntityMap<BuilderViolator>
    {
        
        public BuilderViolatorMap() : 
                base("Основание претензионно исковой работы для Договоров Подряда", "CR_BUILDER_VIOLATOR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BuildContract, "Договор подряда").Column("CONTRACT_ID").NotNull().Fetch();
            Property(x => x.CreationType, "тип создания записи").Column("TYPE_CREATION").DefaultValue(BuildContractCreationType.Auto).NotNull();
            Property(x => x.StartingDate, "Дата начала отсчета").Column("START_DATE");
            Property(x => x.CountDaysDelay, "количество дней просрочки").Column("COUNT_DAYS_DELAY");
        }
    }
}
