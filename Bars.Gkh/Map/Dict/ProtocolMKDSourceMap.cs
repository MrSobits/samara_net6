/// <mapping-converter-backup>
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;


    /// <summary>Маппинг для "Источник протокола МКД"</summary>
    public class ProtocolMKDSourceMap : BaseEntityMap<ProtocolMKDSource>
    {
        
        public ProtocolMKDSourceMap() : 
                base("Источник протокола МКД", "GKH_DICT_MKDPROTOCOL_SOURCE")
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
