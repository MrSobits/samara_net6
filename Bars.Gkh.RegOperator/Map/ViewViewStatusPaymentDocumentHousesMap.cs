namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewDisposal"</summary>
    public class ViewViewStatusPaymentDocumentHousesMap : PersistentObjectMap<ViewStatusPaymentDocumentHouses>
    {
        
        public ViewViewStatusPaymentDocumentHousesMap() : 
                base("Bars.GkhGji.Entities.ViewStatusPaymentDocumentHouses", "VIEW_GIS_GKH_PAY_DOC")
        {
        }
        
        protected override void Map()
        {

            this.Property(x => x.Name, "Муниципальное образование").Column("MO_NAME");
            this.Property(x => x.Period, "Идентификатор периода").Column("PERIOD");
            this.Property(x => x.Address, "Адресс").Column("ADDRESS");
            this.Property(x => x.Account, "Кол-во лицевых счетов").Column("LS_COUNT");
            this.Property(x => x.State, "Статус").Column("STATUS");

        }
    }
}
