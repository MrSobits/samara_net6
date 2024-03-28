/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Enums;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг "ViewHeatingSeason"
///     /// </summary>
///     public class ViewHeatingSeasonMap : PersistentObjectMap<ViewHeatingSeason>
///     {
///         public ViewHeatingSeasonMap() : base("VIEW_GJI_HEATING_SEASON")
///         {
///             Map(x => x.ManOrgName, "MANORG_NAME");
///             Map(x => x.TypeContract, "TYPE_CONTRACT_NAME");
///             Map(x => x.ActFlushing, "ACT_FLUSHING");
///             Map(x => x.ActPressing, "ACT_PRESSING");
///             Map(x => x.ActVentilation, "ACT_VENTILATION");
///             Map(x => x.ActChimney, "ACT_CHIMNEY");
///             Map(x => x.Passport, "PASSPORT");
///             Map(x => x.DateHeat, "DATE_HEAT");
///             Map(x => x.AreaMkd, "AREA_MKD");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.Floors, "FLOORS");
///             Map(x => x.MaximumFloors, "MAXIMUM_FLOORS");
///             Map(x => x.NumberEntrances, "NUMBER_ENTRANCES");
///             Map(x => x.HeatSeasonPeriodId, "PERIOD_ID");
///             Map(x => x.HeatSeasonPeriodName, "PERIOD_NAME");
///             Map(x => x.HeatingSystem, "HEATING_SYSTEM").CustomType<HeatingSystem>();
///             Map(x => x.HeatingSeasonId, "HEAT_SEASON_ID");
///             Map(x => x.Municipality, "MUNICIPALITY_NAME");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.TypeHouse, "TYPE_HOUSE").CustomType<TypeHouse>();
///             Map(x => x.ConditionHouse, "CONDITION_HOUSE").CustomType<ConditionHouse>();
///             Map(x => x.ResidentsEvicted, "RESIDENTS_EVICTED");
///             Map(x => x.DateCommissioning, "DATE_COMMISSIONING");
///             
///             References(x => x.Period, "HEAT_SEAS_PERIOD_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "ViewHeatingSeason"</summary>
    public class ViewHeatingSeasonMap : PersistentObjectMap<ViewHeatingSeason>
    {
        
        public ViewHeatingSeasonMap() : 
                base("ViewHeatingSeason", "VIEW_GJI_HEATING_SEASON")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ManOrgName, "Управляющая организация").Column("MANORG_NAME");
            Property(x => x.TypeContract, "Типы договоров").Column("TYPE_CONTRACT_NAME");
            Property(x => x.ActFlushing, "Документ \"Акт промывки системы отопления\"").Column("ACT_FLUSHING");
            Property(x => x.ActPressing, "Документ \"Акт опресовки системы отопления\"").Column("ACT_PRESSING");
            Property(x => x.ActVentilation, "Документ \"Акт проверки вентиляционных каналов\"").Column("ACT_VENTILATION");
            Property(x => x.ActChimney, "Документ \"Акт проверки дымоходов\"").Column("ACT_CHIMNEY");
            Property(x => x.Passport, "Документ \"Паспорт готовности\"").Column("PASSPORT");
            Property(x => x.DateHeat, "Дата включения тепла").Column("DATE_HEAT");
            Property(x => x.AreaMkd, "Площадь жилого дома").Column("AREA_MKD");
            Property(x => x.Address, "Адрес").Column("ADDRESS");
            Property(x => x.Floors, "Минимальная этажность").Column("FLOORS");
            Property(x => x.MaximumFloors, "Максимальная этажность").Column("MAXIMUM_FLOORS");
            Property(x => x.NumberEntrances, "Количество подъездов").Column("NUMBER_ENTRANCES");
            Property(x => x.HeatSeasonPeriodId, "Идентификатор периода отопительного сезона").Column("PERIOD_ID");
            Property(x => x.HeatSeasonPeriodName, "Наименование периода отопительного сезона").Column("PERIOD_NAME");
            Property(x => x.HeatingSystem, "Отопительная система").Column("HEATING_SYSTEM");
            Property(x => x.HeatingSeasonId, "Идентификатор отопительного сезона").Column("HEAT_SEASON_ID");
            Property(x => x.Municipality, "Муниицпальное образование").Column("MUNICIPALITY_NAME");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования").Column("MU_ID");
            Property(x => x.TypeHouse, "Тип дома").Column("TYPE_HOUSE");
            Property(x => x.ConditionHouse, "Состояние дома").Column("CONDITION_HOUSE");
            Property(x => x.ResidentsEvicted, "Жильцы выселены").Column("RESIDENTS_EVICTED");
            Property(x => x.DateCommissioning, "Дата сдачи в эксплуатацию").Column("DATE_COMMISSIONING");
            Reference(x => x.Period, "Период отопительного сезона").Column("HEAT_SEAS_PERIOD_ID").Fetch();
        }
    }
}
