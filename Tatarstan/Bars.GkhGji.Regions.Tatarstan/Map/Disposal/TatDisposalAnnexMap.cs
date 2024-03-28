using Bars.GkhGji.Regions.Tatarstan.Entities;

namespace Bars.GkhGji.Regions.Tatarstan.Map.Disposal
{
    using Bars.Gkh.Map;

    public class TatDisposalAnnexMap : GkhJoinedSubClassMap<TatDisposalAnnex>
    {
        public TatDisposalAnnexMap() : base( "GJI_TAT_DISPOSAL_ANNEX")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ErknmGuid, "Идентификатор в ЕРКНМ").Column("ERKNM_GUID").Length(36);
            this.Reference(x => x.ErknmTypeDocument, "Тип документа ЕРКНМ").Column("ERKNM_TYPE_DOCUMENT_ID").Fetch();
        }
    }
}
