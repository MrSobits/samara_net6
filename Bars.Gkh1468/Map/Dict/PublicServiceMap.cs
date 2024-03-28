namespace Bars.Gkh1468.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh1468.Entities.PublicService"</summary>
    public class PublicServiceMap : BaseImportableEntityMap<PublicService>
    {
        
        public PublicServiceMap() : 
                base("Bars.Gkh1468.Entities.PublicService", "GKH_PUBLIC_SERV")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Name, "Name").Column("NAME").Length(500);
        }
    }
}
