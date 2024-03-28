namespace Bars.GkhGji.Regions.Tatarstan.Map.Views
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Views;

    public class ViewTatDisposalMap : PersistentObjectMap<ViewTatDisposal>
    {
        public ViewTatDisposalMap() : 
                base(nameof(ViewTatDisposal), "VIEW_GJI_TAT_DISPOSAL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.TypeSurveyNames, "Типы обследования").Column("TSURVEYS_NAME");
            this.Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            this.Property(x => x.IsActCheckExist, "Создан акт проверки общий").Column("ACT_CHECK_EXIST");
            this.Property(x => x.InspectorNames, "ФИО инспекторов").Column("INSPECTOR_NAMES");
            this.Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            this.Property(x => x.RealityObjectAddresses, "Адреса жилых домов").Column("RO_ADDRESSES");
            this.Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            this.Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            this.Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            this.Property(x => x.ContragentName, "Контрагент").Column("CONTRAGENT");
            this.Property(x => x.DateEnd, "Дата окончания обследования").Column("DATE_END");
            this.Property(x => x.DateStart, "Дата начала обследования").Column("DATE_START");
            this.Property(x => x.DisposalGjiId, "Распоряжение").Column("DOCUMENT_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNum, "Номер документа (целая часть)").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            this.Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            this.Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            this.Property(x => x.KindCheckName, "Наименование вида проверки").Column("KIND_CHECK_NAME");
            this.Property(x => x.InspectionId, "Основание проверки").Column("INSPECTION_ID");
            this.Property(x => x.TypeDisposal, "Тип документа ГЖИ").Column("TYPE_DISPOSAL");
            this.Property(x => x.TypeAgreementProsecutor, "TypeAgreementProsecutor").Column("TYPE_AGRPROSECUTOR").NotNull();
            this.Property(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE");
            this.Property(x => x.HasActSurvey, "Флаг наличия акта обследования").Column("HAS_ACT_SURVEY");
            this.Property(x => x.ErpRegistrationNumber, "Учетный номер проверки в ЕРП").Column("REGISTRATION_NUMBER_ERP");
            this.Property(x => x.ControlTypeName, "Наименование вида контроля").Column("CONTROL_TYPE_NAME");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
            this.Reference(x => x.License, "Лицензия УО").Column("LICENSE").Fetch();
        }
    }
}