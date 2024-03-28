
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Виды рисков"</summary>
    public class VideoOverwatchTypeMap : BaseEntityMap<VideoOverwatchType>
    {
        
        public VideoOverwatchTypeMap() : 
                base("Виды рисков", "GKH_DICT_TYPE_VIDEO_OVERWATCH")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(1000);
        }
    }
}
