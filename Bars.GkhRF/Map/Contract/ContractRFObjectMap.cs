/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
///     using Bars.GkhRf.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Объект договора рег. фонда"
///     /// </summary>
///     public class ContractRfObjectMap : BaseGkhEntityMap<ContractRfObject>
///     {
///         public ContractRfObjectMap() : base("RF_CONTRACT_OBJECT")
///         {
///             Map(x => x.TypeCondition, "TYPE_CONDITION").Not.Nullable().CustomType<TypeCondition>();
///             Map(x => x.IncludeDate, "INCLUDE_DATE");
///             Map(x => x.ExcludeDate, "EXCLUDE_DATE");
/// 
///             References(x => x.ContractRf, "CONTRACT_RF_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Объект договора рег. фонда"</summary>
    public class ContractRfObjectMap : BaseEntityMap<ContractRfObject>
    {
        
        /// <summary>
        /// Конструктор
        /// </summary>
        public ContractRfObjectMap() : 
                base("Объект договора рег. фонда", "RF_CONTRACT_OBJECT")
        {
        }
        
        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeCondition, "Тип состояния объекта").Column("TYPE_CONDITION").NotNull();
            this.Property(x => x.IncludeDate, "Дата включения в договор").Column("INCLUDE_DATE");
            this.Property(x => x.ExcludeDate, "Дата исключения из договора").Column("EXCLUDE_DATE");
            this.Property(x => x.TotalArea, "Общая площадь жилых и нежилых помещений в доме").Column("TOTAL_AREA");
            this.Property(x => x.AreaLiving, "Общая площадь жилых помещений в доме").Column("AREA_LIV");
            this.Property(x => x.AreaNotLiving, "Общая площадь нежилых помещений в доме").Column("AREA_NOT_LIV");
            this.Property(x => x.AreaLivingOwned, "Общая площадь жилых помещений находящихся в собственности граждан").Column("AREA_LIV_OWN");
            this.Property(x => x.Note, "Примечание").Column("NOTE");
            
            this.Reference(x => x.ContractRf, "Договор рег. фонда").Column("CONTRACT_RF_ID").NotNull().Fetch();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").Fetch();
        }
    }
}
