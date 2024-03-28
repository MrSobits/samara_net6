namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Mapping for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public class DpkrDocumentProgramVersionMap : BaseEntityMap<DpkrDocumentProgramVersion>
    {
        /// <inheritdoc />
        public DpkrDocumentProgramVersionMap()
            : base(typeof(DpkrDocumentProgramVersion).FullName, "OVRHL_DPKR_DOCUMENT_PRG_VERSION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DpkrDocument, "Документ ДПКР").Column("DPKR_DOCUMENT_ID");
            this.Reference(x => x.ProgramVersion, "Версия программы").Column("PRG_VERSION_ID");
        }
    }
}