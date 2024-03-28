namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewResolutionRospotrebnadzor"</summary>
    public class ViewResolutionRospotrebnadzorMap : PersistentObjectMap<ViewResolutionRospotrebnadzor>
    {
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "VIEW_GJI_RESOLUTION_ROSPOTREBNADZOR";

        public ViewResolutionRospotrebnadzorMap() :
                base("Bars.GkhGji.Entities.ViewResolution", ViewResolutionRospotrebnadzorMap.TableName)
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.SumPays, "Сумма оплат штрафов").Column("SUM_PAYS");
            this.Property(x => x.RealityObjectIds, "строка идентификаторов жилых домов вида /1/2/4/").Column("RO_IDS");
            this.Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            this.Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            this.Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            this.Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            this.Property(x => x.OfficialName, "ФИО ДЛ вынесшего постановление").Column("OFFICIAL_NAME");
            this.Property(x => x.OfficialId, "Идентификатор ДЛ вынесшего постановление").Column("OFFICIAL_ID");
            this.Property(x => x.PenaltyAmount, "Сумма штрафа").Column("PENALTY_AMOUNT");
            this.Property(x => x.InspectionId, "Идентификатор основания проверки").Column("INSPECTION_ID");
            this.Property(x => x.TypeBase, "Тип основания проверки").Column("TYPE_BASE");
            this.Property(x => x.TypeDocumentGji, "Тип документа ГЖИ").Column("TYPE_DOC");
            this.Property(x => x.TypeExecutant, "Тип исполнителя").Column("TYPE_EXEC_NAME");
            this.Property(x => x.Sanction, "Санкция").Column("SANCTION_NAME");
            this.Property(x => x.ContragentMuName, "Контрагент МО Name").Column("CTR_MU_NAME");
            this.Property(x => x.ContragentMuId, "Контрагент МО Id").Column("CTR_MU_ID");
            this.Property(x => x.ContragentName, "Контрагент (исполнитель)").Column("CONTRAGENT_NAME");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNum, "Целая часть номера документа").Column("DOCUMENT_NUM");
            this.Property(x => x.DocumentNumber, "номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.ResolutionId, "Постановление").Column("DOCUMENT_ID");
            this.Property(x => x.DeliveryDate, "Дата вручения").Column("DELIVERY_DATE");
            this.Property(x => x.Paided, "Штраф оплачен").Column("PAIDED");
            this.Property(x => x.RoAddress, "Адреса по нарушениям").Column("RO_ADDRESS");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
        }
    }
}
