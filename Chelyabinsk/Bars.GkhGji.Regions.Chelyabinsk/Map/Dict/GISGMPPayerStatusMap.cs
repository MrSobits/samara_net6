namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class GISGMPPayerStatusMap : BaseEntityMap<GISGMPPayerStatus>
    {
        
        public GISGMPPayerStatusMap() : 
                base("Статус плательщика", "GJI_CH_DICT_PAYER_STATUS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
        }
    }
}
