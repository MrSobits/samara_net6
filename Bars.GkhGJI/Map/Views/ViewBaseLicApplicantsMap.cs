/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг View "Проверка по соискателям лицензии"
///     /// </summary>
///     public class ViewBaseLicApplicantsMap : PersistentObjectMap<ViewBaseLicApplicants>
///     {
///         public ViewBaseLicApplicantsMap()
///             : base("VIEW_GJI_INS_LICENSE_APP")
///         {
///             Map(x => x.IsDisposal, "IS_DISPOSAL");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.PersonInspection, "PERSON_INSPECTION").CustomType<PersonInspection>();
///             Map(x => x.TypeJurPerson, "TYPE_JUR_PERSON").CustomType<TypeJurPerson>();
///             Map(x => x.InspectionNumber, "INSPECTION_NUMBER");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.RealObjAddresses, "RO_ADR");
///             Map(x => x.ReqNumber, "REG_NUMBER");
///             Map(x => x.ContragentId, "CTR_ID");
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseLicApplicants"</summary>
    public class ViewBaseLicApplicantsMap : PersistentObjectMap<ViewBaseLicApplicants>
    {
        
        public ViewBaseLicApplicantsMap() : 
                base("Bars.GkhGji.Entities.ViewBaseLicApplicants", "VIEW_GJI_INS_LICENSE_APP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.IsDisposal, "Наличие распоряжения").Column("IS_DISPOSAL");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION");
            Property(x => x.TypeJurPerson, "Тип контрагента").Column("TYPE_JUR_PERSON");
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            Property(x => x.ContragentName, "Контрагент (в отношении)").Column("CONTRAGENT_NAME");
            Property(x => x.RealObjAddresses, "Адреса домов").Column("RO_ADR");
            Property(x => x.ReqNumber, "Номер обращения").Column("REG_NUMBER");
            Property(x => x.ContragentId, "Id юр. лица").Column("CTR_ID");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
