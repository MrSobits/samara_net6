/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     /// <summary>
///     /// Маппинг View "Инспекционная проверка"
///     /// </summary>
///     public class ViewBaseInsCheckMap : PersistentObjectMap<ViewBaseInsCheck>
///     {
///         public ViewBaseInsCheckMap()
///             : base("view_gji_ins_inschek")
///         {
///             Map(x => x.DisposalNumber, "DISP_NUMBER");
///             Map(x => x.InspectorNames, "inspectors");
///             Map(x => x.RealityObjectCount, "RO_COUNT");
///             Map(x => x.RealityObjectAddress, "RO_ADDRESS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.PlanId, "PLAN_ID");
///             Map(x => x.PlanName, "PLAN_NAME");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.TypeFact, "TYPE_FACT").CustomType<TypeFactInspection>();
///             Map(x => x.InspectionNumber, "INSPECTION_NUMBER");
///             Map(x => x.InsCheckDate, "INSCHECK_DATE");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseInsCheck"</summary>
    public class ViewBaseInsCheckMap : PersistentObjectMap<ViewBaseInsCheck>
    {
        
        public ViewBaseInsCheckMap() : 
                base("Bars.GkhGji.Entities.ViewBaseInsCheck", "VIEW_GJI_INS_INSCHEK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DisposalNumber, "Номер распоряжения").Column("DISP_NUMBER");
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTORS");
            Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            Property(x => x.RealityObjectAddress, "Адреса домов разделенных \';\'").Column("RO_ADDRESS");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.ContragentName, "Юридическое лицо").Column("CONTRAGENT_NAME");
            Property(x => x.PlanId, "Идентификатор плана").Column("PLAN_ID");
            Property(x => x.PlanName, "Наименование плана").Column("PLAN_NAME");
            Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.TypeFact, "Факт проверки").Column("TYPE_FACT");
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            Property(x => x.InsCheckDate, "Дата проверки").Column("INSCHECK_DATE");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
