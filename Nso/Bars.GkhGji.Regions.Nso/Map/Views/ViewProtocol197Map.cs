namespace Bars.GkhGji.Regions.Nso.Map
{
	using Bars.B4.DataAccess;
	using Bars.B4.Modules.Mapping.Mappers;
	using Bars.GkhGji.Enums;
	using Bars.GkhGji.Regions.Nso.Entities;

	public class ViewProtocol197Map : PersistentObjectMap<ViewProtocol197>
    {
		public ViewProtocol197Map() :
			base("Bars.GkhGji.Regions.Nso.Entities.ViewProtocol197", "VIEW_GJI_PROTOCOL197")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CountViolation, "Количество нарушений").Column("COUNT_VIOL");
            Property(x => x.InspectorNames, "Инспекторы").Column("INSPECTOR_NAMES");
            Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            Property(x => x.ContragentName, "Контрагент (исполнитель)").Column("CONTRAGENT_NAME");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentNum, "Целая часть номер документа").Column("DOCUMENT_NUM");
            Property(x => x.DocumentNumber, "номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            Property(x => x.TypeExecutant, "Тип исполнителя").Column("TYPE_EXEC_NAME");
            Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");

            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}