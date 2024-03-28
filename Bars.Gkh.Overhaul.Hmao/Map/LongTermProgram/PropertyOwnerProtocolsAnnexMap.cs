
namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Протоколы собственников помещений МКД"</summary>
    public class PropertyOwnerProtocolsAnnexMap : BaseImportableEntityMap<PropertyOwnerProtocolsAnnex>
    {
        
        public PropertyOwnerProtocolsAnnexMap() : 
                base("Протоколы собственников помещений МКД", "OVRHL_PROP_OWN_PROTOCOLS_ANNEX")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID").Fetch();
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.FileInfo, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
