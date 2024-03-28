/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using B4.DataAccess;
///     using Enums;
///     using Entities;
///     using Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг View "Проверка по поручения руководства"
///     /// </summary>
///     public class ViewBaseDispHeadMap : PersistentObjectMap<ViewBaseDispHead>
///     {
///         public ViewBaseDispHeadMap() : base("VIEW_GJI_INS_DISPHEAD")
///         {
///             Map(x => x.DisposalTypeSurveys, "DISP_TYPES");
///             Map(x => x.InspectorNames, "INSPECTORS");
///             Map(x => x.RealityObjectCount, "RO_COUNT");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.InspectionNumber, "INSPECTION_NUMBER");
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.HeadFio, "HEAD_FIO");
///             Map(x => x.HeadId, "HEAD_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.DispHeadDate, "DISPHEAD_DATE");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.PersonInspection, "PERSON_INSPECTION").CustomType<PersonInspection>();
///             Map(x => x.TypeJurPerson, "TYPE_JUR_PERSON").CustomType<TypeJurPerson>();
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
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseDispHead"</summary>
    public class ViewBaseDispHeadMap : PersistentObjectMap<ViewBaseDispHead>
    {
        
        public ViewBaseDispHeadMap() : 
                base("Bars.GkhGji.Entities.ViewBaseDispHead", "VIEW_GJI_INS_DISPHEAD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DisposalTypeSurveys, "Типы обследований").Column("DISP_TYPES");
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTORS");
            Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            Property(x => x.MunicipalityId, "Мунниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.HeadFio, "ФИО руководителя").Column("HEAD_FIO");
            Property(x => x.HeadId, "Идентификатор руководителя").Column("HEAD_ID");
            Property(x => x.ContragentName, "Юридическое лицо").Column("CONTRAGENT_NAME");
            Property(x => x.DispHeadDate, "Дата проверки").Column("DISPHEAD_DATE");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION");
            Property(x => x.TypeJurPerson, "Тип контрагента").Column("TYPE_JUR_PERSON");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
