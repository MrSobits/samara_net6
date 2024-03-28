namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197ActivityDirectionMap : BaseEntityMap<Protocol197ActivityDirection>
    {
		public Protocol197ActivityDirectionMap()
            : base("GJI_PROT197_ACTIV_DIRECT")
        {
            this.References(x => x.ActivityDirection, "ACTIVEDIRECT_ID", ReferenceMapConfig.NotNull);
            this.References(x => x.Protocol197, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
        }
    }
}