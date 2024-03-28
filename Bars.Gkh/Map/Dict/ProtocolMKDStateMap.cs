/// <mapping-converter-backup>
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;


    /// <summary>Маппинг для "Статус протокола МКД"</summary>
    public class ProtocolMKDStateMap : BaseEntityMap<ProtocolMKDState>
    {
        
        public ProtocolMKDStateMap() : 
                base("Статус протокола МКД", "GKH_DICT_MKDPROTOCOL_STATE")
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
