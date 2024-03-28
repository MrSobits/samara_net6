/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг View "Планвоая проверка юр.лиц"
///     /// </summary>
///     public class ViewBaseJurPersonMap : PersistentObjectMap<ViewBaseJurPerson>
///     {
///         public ViewBaseJurPersonMap() : base("VIEW_GJI_INS_JURPERS")
///         {
///             Map(x => x.DisposalNumber, "DISP_NUMBER");
///             Map(x => x.InspectorNames, "inspectors");
///             Map(x => x.RealityObjectCount, "RO_COUNT");
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.RealityObjectAddress, "RO_ADDRESS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.PlanId, "PLAN_ID");
///             Map(x => x.PlanName, "PLAN_NAME");
///             Map(x => x.TypeFact, "TYPE_FACT").CustomType<TypeFactInspection>();
///             Map(x => x.CountDays, "COUNT_DAYS");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.InspectionNumber, "INSPECTION_NUMBER");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseJurPerson"</summary>
    public class ViewBaseJurPersonMap : PersistentObjectMap<ViewBaseJurPerson>
    {
        
        public ViewBaseJurPersonMap() : 
                base("Bars.GkhGji.Entities.ViewBaseJurPerson", "VIEW_GJI_INS_JURPERS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DisposalNumber, "Номер распоряжения").Column("DISP_NUMBER");
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTORS");
            Property(x => x.ZonalInspectionNames, "Отделы").Column("ZONAL_INSPECTIONS");
            Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            Property(x => x.RealityObjectIds, "Строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.RealityObjectAddress, "Адреса домов разделенных \';\'").Column("RO_ADDRESS");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.ContragentName, "Муниципальное образование контрагента").Column("CONTRAGENT_NAME");
            Property(x => x.PlanId, "Идентификатор плана проверок").Column("PLAN_ID");
            Property(x => x.PlanName, "Наименование плана проверок").Column("PLAN_NAME");
            Property(x => x.TypeFact, "Факт проверки ЮЛ").Column("TYPE_FACT");
            Property(x => x.CountDays, "Количество дней проверки").Column("COUNT_DAYS");
            Property(x => x.DateStart, "Дата начала проверки").Column("DATE_START");
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
