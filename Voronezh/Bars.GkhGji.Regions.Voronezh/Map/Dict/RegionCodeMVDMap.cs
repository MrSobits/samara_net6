namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class RegionCodeMVDMap : BaseEntityMap<RegionCodeMVD>
    {
        
        public RegionCodeMVDMap() : 
                base("Код региона", "GJI_CH_DICT_REGCODE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME");
        }
    }
}
