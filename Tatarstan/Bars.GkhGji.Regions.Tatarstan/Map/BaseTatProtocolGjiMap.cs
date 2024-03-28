namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class BaseTatProtocolGjiMap : JoinedSubClassMap<BaseTatProtocolGji>
    {
        public BaseTatProtocolGjiMap() :
            base("Основание Протокол ГЖИ", "GJI_INSPECTION_PROTGJI")
        {
        }

        protected override void Map()
        {
        }
    }
}
