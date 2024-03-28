/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class LoadProgramMap : BaseImportableEntityMap<LoadProgram>
///     {
///         public LoadProgramMap()
///             : base("OVRHL_LOADED_PROGRAM")
///         {
///             Map(x => x.IndexNumber, "INDEX_NUMBER", true);
///             Map(x => x.Locality, "LOCALITY", true);
///             Map(x => x.Street, "STREET", true);
///             Map(x => x.House, "HOUSE", true);
///             Map(x => x.Housing, "HOUSING");
///             Map(x => x.Address, "ADDRESS", true);
///             Map(x => x.CommissioningYear, "YEAR_COMMISSIONING", true);
///             Map(x => x.CommonEstateobject, "COMMON_ESTATE_OBJECT", true);
///             Map(x => x.Wear, "WEAR", true);
///             Map(x => x.LastOverhaulYear, "YEAR_LAST_OVERHAUL", true);
///             Map(x => x.PlanOverhaulYear, "YEAR_PLAN_OVERHAUL", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Сущность записи загруженной программы"</summary>
    public class LoadProgramMap : BaseImportableEntityMap<LoadProgram>
    {
        
        public LoadProgramMap() : 
                base("Сущность записи загруженной программы", "OVRHL_LOADED_PROGRAM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IndexNumber, "Порядковый номер").Column("INDEX_NUMBER").NotNull();
            Property(x => x.Locality, "Населенный пункт").Column("LOCALITY").Length(250).NotNull();
            Property(x => x.Street, "Улица").Column("STREET").Length(250).NotNull();
            Property(x => x.House, "Дом").Column("HOUSE").Length(250).NotNull();
            Property(x => x.Housing, "Корпус").Column("HOUSING").Length(250);
            Property(x => x.Address, "Адрес").Column("ADDRESS").Length(250).NotNull();
            Property(x => x.CommissioningYear, "Год ввода в эксплуатацию").Column("YEAR_COMMISSIONING").NotNull();
            Property(x => x.CommonEstateobject, "Объект общего имущества").Column("COMMON_ESTATE_OBJECT").Length(250).NotNull();
            Property(x => x.Wear, "Износ").Column("WEAR").Length(250).NotNull();
            Property(x => x.LastOverhaulYear, "Дата последнего капитального ремонта").Column("YEAR_LAST_OVERHAUL").NotNull();
            Property(x => x.PlanOverhaulYear, "Плановый год проведения капитального ремонта").Column("YEAR_PLAN_OVERHAUL").NotNull();
        }
    }
}
