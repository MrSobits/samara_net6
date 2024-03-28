/// <mapping-converter-backup>
namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;


    /// <summary>Маппинг для "Инициатор протокола МКД"</summary>
    public class ProtocolMKDIniciatorMap : BaseEntityMap<ProtocolMKDIniciator>
    {
        
        public ProtocolMKDIniciatorMap() : 
                base("Инициатор протокола МКД", "GKH_DICT_MKDPROTOCOL_INICIATOR")
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
