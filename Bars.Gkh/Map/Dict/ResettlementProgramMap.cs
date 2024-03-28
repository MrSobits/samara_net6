/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities.Dicts;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Программа переселения"
///     /// </summary>
///     public class ResettlementProgramMap : BaseGkhEntityMap<ResettlementProgram>
///     {
///         public ResettlementProgramMap()
///             : base("GKH_DICT_RESETTLE_PROGRAM")
///         {
///             Map(x => x.Description, "DESCRIPTION").Length(1000);
///             Map(x => x.MatchFederalLaw, "MATCH_FEDERAL_LAW");
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.StateProgram, "STATE").Not.Nullable().CustomType<StateResettlementProgram>();
///             Map(x => x.TypeProgram, "TYPE").Not.Nullable().CustomType<TypeResettlementProgram>();
///             Map(x => x.UseInExport, "USE_IN_EXPORT").Not.Nullable();
///             Map(x => x.Visibility, "VISIBILITY").Not.Nullable().CustomType<VisibilityResettlementProgram>();
/// 
///             References(x => x.Period, "PERIOD_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Программа переселения"</summary>
    public class ResettlementProgramMap : BaseImportableEntityMap<ResettlementProgram>
    {
        
        public ResettlementProgramMap() : 
                base("Программа переселения", "GKH_DICT_RESETTLE_PROGRAM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
            Property(x => x.MatchFederalLaw, "Соответствует ФЗ").Column("MATCH_FEDERAL_LAW");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.StateProgram, "Состояние программы").Column("STATE").NotNull();
            Property(x => x.TypeProgram, "Тип программы").Column("TYPE").NotNull();
            Property(x => x.UseInExport, "Используется при экспорте").Column("USE_IN_EXPORT").NotNull();
            Property(x => x.Visibility, "Видимость программы").Column("VISIBILITY").NotNull();
            Reference(x => x.Period, "Период").Column("PERIOD_ID").Fetch();
        }
    }
}
