namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewBaseStatement"</summary>
    public class ViewBaseStatementMap : PersistentObjectMap<ViewBaseStatement>
    {
        
        public ViewBaseStatementMap() : 
                base("Bars.GkhGji.Entities.ViewBaseStatement", "VIEW_GJI_INS_STATEMENT")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.IsDisposal, "Наличие распоряжения").Column("IS_DISPOSAL");
            this.Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            this.Property(x => x.MunicipalityNames, "Наименования муниципальных образований жилых домов").Column("MU_NAMES");
            this.Property(x => x.MoNames, "Наименования муниципальных образований жилых домов").Column("MO_NAMES");
            this.Property(x => x.PlaceNames, "Наименования населенных пунктов жилых домов").Column("PLACE_NAMES");
            this.Property(x => x.MunicipalityId, "Муниципальное образование первого жилого дома").Column("MU_ID");
            this.Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION");
            this.Property(x => x.TypeJurPerson, "Тип контрагента").Column("TYPE_JUR_PERSON");
            this.Property(x => x.InspectionNumber, "Номер проверки").Column("INSPECTION_NUMBER");
            this.Property(x => x.ContragentName, "Контрагент (в отношении)").Column("CONTRAGENT_NAME");
            this.Property(x => x.DocumentNumber, "Номер обращения").Column("DOCUMENT_NUMBER");
            this.Property(x => x.RealObjAddresses, "Адреса домов").Column("RO_ADR");
            this.Property(x => x.RequestType, "Тип запроса").Column("REQUEST_TYPE");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}
