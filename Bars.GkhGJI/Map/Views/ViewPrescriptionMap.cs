/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     public class ViewPrescriptionMap : PersistentObjectMap<ViewPrescription>
///     {
///         public ViewPrescriptionMap() : base("VIEW_GJI_PRESCRIPTION")
///         {
///             Map(x => x.CountRealityObject, "COUNT_RO");
///             Map(x => x.CountViolation, "COUNT_VIOL");
///             Map(x => x.InspectorNames, "INSPECTOR_NAMES");
///             Map(x => x.RealityObjectIds, "RO_IDS");
///             Map(x => x.MunicipalityNames, "MU_NAMES");
///             Map(x => x.MoNames, "MO_NAMES");
///             Map(x => x.PlaceNames, "PLACE_NAMES");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM");
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER");
///             Map(x => x.ContragentMuName, "CTR_MU_NAME");
///             Map(x => x.ContragentMuId, "CTR_MU_ID");
///             Map(x => x.ContragentName, "CONTRAGENT_NAME");
///             Map(x => x.TypeExecutant, "TYPE_EXEC_NAME");
///             Map(x => x.TypeBase, "TYPE_BASE").CustomType<TypeBase>();
///             Map(x => x.TypeDocumentGji, "TYPE_DOC").CustomType<TypeDocumentGji>();
///             Map(x => x.MunicipalityId, "MU_ID");
///             Map(x => x.PrescriptionGjiId, "DOCUMENT_ID");
///             Map(x => x.InspectionId, "INSPECTION_ID");
///             Map(x => x.DateRemoval, "DATE_REMOVAL");
///             Map(x => x.DisposalId, "DISP_ID");
/// 
///             References(x => x.State, "STATE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewPrescription"</summary>
    public class ViewPrescriptionMap : PersistentObjectMap<ViewPrescription>
    {
        
        public ViewPrescriptionMap() : 
                base("Bars.GkhGji.Entities.ViewPrescription", "VIEW_GJI_PRESCRIPTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CountRealityObject, "Количество домов").Column("COUNT_RO");
            Property(x => x.CountViolation, "Количество нарушений").Column("COUNT_VIOL");
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.RealityObjectAddresses, "Адреса жилых домов").Column("RO_ADDRESSES");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.DocumentDate, "Дата дкоумента").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номер документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            Property(x => x.ContragentName, "Контрагент (Исполнитель)").Column("CONTRAGENT_NAME");
            Property(x => x.TypeExecutant, "Тип исполнителя").Column("TYPE_EXEC_NAME");
            Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            Property(x => x.ViolationList, "НПА нарушений").Column("NPA_VIOLATIONS");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            Property(x => x.PrescriptionGjiId, "Предписание").Column("DOCUMENT_ID");
            Property(x => x.InspectionId, "Идентификатор Основания проверки").Column("INSPECTION_ID");
            Property(x => x.DateRemoval, "Дата самого позднего срока устранения нарушений (во вкладке \"Нарушения\")").Column("DATE_REMOVAL");
            Property(x => x.DisposalId, "Распоряжение на проверку этого предписания").Column("DISP_ID");
            Property(x => x.AppealDate, "Дата обращения").Column("APPEAL_DATE");
            Property(x => x.AppealNumber, "Номер обращения").Column("APPEAL_NUMBER");
            Property(x => x.INN, "INN").Column("INN");
            Property(x => x.AppealDescription, "Номер обращения").Column("APPEAL_DESCRIPTION");
            Property(x => x.ControlType, "Тип Контроля надзора").Column("CONTROL_TYPE");
            Property(x => x.TypePrescriptionExecution, "TypePrescriptionExecution").Column("TYPE_EXECUTION");
            Property(x => x.CancelledGJI, "CancelledGJI").Column("CANCELLED_BY_GJI");
            Property(x => x.PrescriptionState, "PrescriptionState").Column("PRESCRIPTION_STATE");
            Property(x => x.HasNotRemoovedViolations, "Есть неустраненные обращения").Column("NOT_REMOOVED");
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
