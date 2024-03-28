namespace Bars.GkhGji.Regions.Nso.Map
{
    using B4.DataAccess.ByCode;
    using Entities;

    public class Protocol197ActivityDirectionMap : BaseEntityMap<Protocol197ActivityDirection>
    {
		public Protocol197ActivityDirectionMap()
            : base("GJI_PROT197_ACTIV_DIRECT")
        {
            References(x => x.ActivityDirection, "ACTIVEDIRECT_ID", ReferenceMapConfig.NotNull);
            References(x => x.Protocol197, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
        }
    }
}