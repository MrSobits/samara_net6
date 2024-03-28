/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Views
/// {
///     using B4.DataAccess;
/// 
///     using Enums;
///     using Entities;
///     using Gkh.Enums;
/// 
///     public class ViewHeatSeasonDocMap : PersistentObjectMap<ViewHeatSeasonDoc>
///     {
///         public ViewHeatSeasonDocMap() : base("VIEW_GJI_HEATSEASON_DOC")
///         {
///             Map(x => x.TypeDocument, "TYPE_DOCUMENT").CustomType<HeatSeasonDocType>();
///             Map(x => x.HeatingSystem, "HEATING_SYSTEM").CustomType<HeatingSystem>();
///             Map(x => x.TypeHouse, "TYPE_HOUSE").CustomType<TypeHouse>();
///             Map(x => x.Address, "ADDRESS");
///             Map(x => x.MunicipalityName, "MUNICIPALITY_NAME");
///             Map(x => x.ManOrgName, "MANORG_NAME");
///             Map(x => x.PeriodId, "HEATSEASON_PERIOD_ID");
///             Map(x => x.ConditionHouse, "CONDITION_HOUSE").CustomType<ConditionHouse>();
/// 
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewHeatSeasonDoc"</summary>
    public class ViewHeatSeasonDocMap : PersistentObjectMap<ViewHeatSeasonDoc>
    {
        
        public ViewHeatSeasonDocMap() : 
                base("Bars.GkhGji.Entities.ViewHeatSeasonDoc", "VIEW_GJI_HEATSEASON_DOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeDocument, "Тип документа подготовки к отопительному сезону").Column("TYPE_DOCUMENT");
            Property(x => x.HeatingSystem, "Система отопления").Column("HEATING_SYSTEM");
            Property(x => x.TypeHouse, "Тип дома").Column("TYPE_HOUSE");
            Property(x => x.Address, "Адрес").Column("ADDRESS");
            Property(x => x.MunicipalityName, "Муниципальное образование").Column("MUNICIPALITY_NAME");
            Property(x => x.ManOrgName, "Управляющая организация").Column("MANORG_NAME");
            Property(x => x.PeriodId, "Идентификатор периода отопительного сезона").Column("HEATSEASON_PERIOD_ID");
            Property(x => x.ConditionHouse, "Состояние дома").Column("CONDITION_HOUSE");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
