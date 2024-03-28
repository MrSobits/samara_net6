namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Mapping for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public class DpkrDocumentRealityObjectMap : BaseEntityMap<DpkrDocumentRealityObject>
    {
        /// <inheritdoc />
        public DpkrDocumentRealityObjectMap()
            : base(typeof(DpkrDocumentRealityObject).FullName, "OVRHL_DPKR_DOCUMENT_REAL_OBJ")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DpkrDocument, "Документ ДПКР").Column("DPKR_DOCUMENT_ID");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID");
            this.Property(x => x.IsExcluded, "Исключен?").Column("IS_EXCLUDED");
        }
    }
}