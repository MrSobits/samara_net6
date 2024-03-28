namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class DpkrDocumentRealityObjectMap : BaseEntityMap<DpkrDocumentRealityObject>
    {
        /// <inheritdoc />
        public DpkrDocumentRealityObjectMap()
            : base(typeof(DpkrDocumentRealityObject).FullName, "OVRHL_TAT_DPKR_DOCUMENT_REALITY_OBJECT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DpkrDocument, nameof(DpkrDocumentRealityObject.DpkrDocument)).Column("DPKR_DOCUMENTS_ID").Fetch().NotNull();
            this.Reference(x => x.RealityObject, nameof(DpkrDocumentRealityObject.RealityObject)).Column("REALITY_OBJECT_ID").Fetch().NotNull();
            this.Property(x => x.IsIncluded, nameof(DpkrDocumentRealityObject.IsIncluded)).Column("IS_INCLUDED").NotNull();
            this.Property(x => x.IsExcluded, nameof(DpkrDocumentRealityObject.IsExcluded)).Column("IS_EXCLUDED").NotNull();
        }
    }
}