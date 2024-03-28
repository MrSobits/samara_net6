namespace Bars.Gkh.Map.Administration.FormatDataExport
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    public class FormatDataExportInfoMap : BaseEntityMap<FormatDataExportInfo>
    {
        /// <inheritdoc />
        public FormatDataExportInfoMap()
            : base(typeof(FormatDataExportInfo).FullName, "GKH_FORMAT_DATA_EXPORT_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.State, nameof(FormatDataExportInfo.State)).Column("STATE").NotNull();
            this.Property(x => x.ObjectType, nameof(FormatDataExportInfo.ObjectType)).Column("OBJECT_TYPE").NotNull();
            this.Property(x => x.LoadDate, nameof(FormatDataExportInfo.LoadDate)).Column("LOAD_DATE").NotNull();
        }
    }
}
