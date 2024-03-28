/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     public class ViewRealityObjectMap : PersistentObjectMap<ViewRealityObject>
///     {
///         public ViewRealityObjectMap() : base("VIEW_GKH_REALITY_OBJECT")
///         {
///             Map(x => x.ExternalId, "EXTERNAL_ID");
///             Map(x => x.RealityObjectId, "REALITY_OBJECT_ID");
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.FullAddress, "FULL_ADDRESS");
///             Map(x => x.TypeHouse, "TYPE_HOUSE").CustomType<TypeHouse>();
///             Map(x => x.ConditionHouse, "CONDITION_HOUSE");
///             Map(x => x.DateDemolition, "DATE_DEMOLITION");
///             Map(x => x.Floors, "FLOORS");
///             Map(x => x.NumberEntrances, "NUMBER_ENTRANCES");
///             Map(x => x.NumberLiving, "NUMBER_LIVING");
///             Map(x => x.NumberApartments, "NUMBER_APARTMENTS");
///             Map(x => x.AreaMkd, "AREA_MKD");
///             Map(x => x.AreaLiving, "AREA_LIVING");
///             Map(x => x.PhysicalWear, "PHYSICAL_WEAR");
///             Map(x => x.NumberLifts, "NUMBER_LIFTS");
///             Map(x => x.HeatingSystem, "HEATING_SYSTEM").CustomType<HeatingSystem>();
///             Map(x => x.TypeRoof, "TYPE_ROOF").CustomType<TypeRoof>();
///             Map(x => x.DateLastRenovation, "DATE_LAST_RENOVATION");
///             Map(x => x.DateCommissioning, "DATE_COMMISSIONING");
///             Map(x => x.CodeErc, "CODE_ERC");
///             Map(x => x.IsInsuredObject, "IS_INSURED_OBJECT");
///             Map(x => x.RoofingMaterialId, "ROOFING_MATERIAL_ID");
///             Map(x => x.WallMaterialId, "WALL_MATERIAL_ID");
///             Map(x => x.RoofingMaterial, "ROOFING_MATERIAL_NAME");
///             Map(x => x.WallMaterial, "WALL_MATERIAL_NAME");
///             Map(x => x.Municipality, "MUNICIPALITY_NAME");
///             Map(x => x.MunicipalityId, "MUNICIPALITY_ID");
///             Map(x => x.Settlement, "STL_NAME");
///             Map(x => x.SettlementId, "SETTLEMENT_ID");
///             Map(x => x.ManOrgNames, "MANORG_NAME");
///             Map(x => x.TypeContracts, "TYPE_CONTRACT_NAME");
///             Map(x => x.GkhCode, "GKH_CODE");
///             Map(x => x.IsBuildSocialMortgage, "IS_BUILD_SOC_MORTGAGE").CustomType<YesNo>();
///             Map(x => x.IsRepairInadvisable, "IS_REPAIR_INADVISABLE");
///             Map(x => x.IsNotInvolvedCr, "IS_NOT_INVOLVED_CR");
///             Map(x => x.District, "DISTRICT");
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Представление "Жилой дом""</summary>
    public class ViewRealityObjectMap : PersistentObjectMap<ViewRealityObject>
    {
        
        public ViewRealityObjectMap() : 
                base("Представление \"Жилой дом\"", "VIEW_GKH_REALITY_OBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.RealityObjectId, "Идентификатор жилого дома").Column("REALITY_OBJECT_ID");
            Property(x => x.Address, "Адрес").Column("ADDRESS");
            Property(x => x.FullAddress, "Полный адрес").Column("FULL_ADDRESS");
            Property(x => x.TypeHouse, "Тип дома").Column("TYPE_HOUSE");
            Property(x => x.ConditionHouse, "Состояние дома").Column("CONDITION_HOUSE");
            Property(x => x.DateDemolition, "Дата сноса").Column("DATE_DEMOLITION");
            Property(x => x.Floors, "Этажность").Column("FLOORS");
            Property(x => x.NumberEntrances, "Количество подъездов").Column("NUMBER_ENTRANCES");
            Property(x => x.NumberLiving, "Количество проживающих").Column("NUMBER_LIVING");
            Property(x => x.NumberApartments, "Количество квартир").Column("NUMBER_APARTMENTS");
            Property(x => x.AreaMkd, "Площадь МКД").Column("AREA_MKD");
            Property(x => x.AreaLiving, "Площадь жилая, всего (кв. м.)").Column("AREA_LIVING");
            Property(x => x.PhysicalWear, "Физические повреждение").Column("PHYSICAL_WEAR");
            Property(x => x.NumberLifts, "Количество лифтов").Column("NUMBER_LIFTS");
            Property(x => x.HeatingSystem, "Отопительная система").Column("HEATING_SYSTEM");
            Property(x => x.TypeRoof, "Тип крыши").Column("TYPE_ROOF");
            Property(x => x.DateLastRenovation, "Дата последнего кап ремонта").Column("DATE_LAST_RENOVATION");
            Property(x => x.DateCommissioning, "Дата сдачи объекта в эксплуатацию").Column("DATE_COMMISSIONING");
            Property(x => x.CodeErc, "Код ЕРЦ").Column("CODE_ERC");
            Property(x => x.IsInsuredObject, "Объект застрахован").Column("IS_INSURED_OBJECT");
            Property(x => x.HasVidecam, "HAS_VIDECAM").Column("HAS_VIDECAM");
            Property(x => x.RoofingMaterialId, "Идентификатор материала крыши").Column("ROOFING_MATERIAL_ID");
            Property(x => x.WallMaterialId, "Идентификатор материала стен").Column("WALL_MATERIAL_ID");
            Property(x => x.RoofingMaterial, "Материал крыши").Column("ROOFING_MATERIAL_NAME");
            Property(x => x.WallMaterial, "Материал стен").Column("WALL_MATERIAL_NAME");
            Property(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_NAME");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования (для фильтрации по операторам)").Column("MUNICIPALITY_ID");
            Property(x => x.Settlement, "Муниципальное образование").Column("STL_NAME");
            Property(x => x.SettlementId, "Идентификатор муниципального образования (для фильтрации по операторам)").Column("SETTLEMENT_ID");
            Property(x => x.ManOrgNames, "Управляющие организации").Column("MANORG_NAME");
            Property(x => x.TypeContracts, "Типы договоров (управление домом)").Column("TYPE_CONTRACT_NAME");
            Property(x => x.GkhCode, "Внутрнний Код дома").Column("GKH_CODE");
            Property(x => x.IsBuildSocialMortgage, "Дом построен по соц.ипотеке(да/нет)").Column("IS_BUILD_SOC_MORTGAGE");
            Property(x => x.IsRepairInadvisable, "IsRepairInadvisable").Column("IS_REPAIR_INADVISABLE");
            Property(x => x.IsNotInvolvedCr, "Дом не участвует в КР (в программе кап. ремонта)").Column("IS_NOT_INVOLVED_CR");
            Property(x => x.District, "Округ").Column("DISTRICT");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
