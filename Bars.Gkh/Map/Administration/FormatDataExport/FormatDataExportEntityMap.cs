namespace Bars.Gkh.Map.Administration.FormatDataExport
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.FormatDataExport;

    public class FormatDataExportEntityMap : BaseEntityMap<FormatDataExportEntity>
    {
        /// <inheritdoc />
        public FormatDataExportEntityMap()
            : base(typeof(FormatDataExportEntity).FullName, "GKH_FORMAT_DATA_EXPORT_ENTITY")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.EntityId, nameof(FormatDataExportEntity.EntityId)).Column("ENTITY_ID");
            this.Property(x => x.ExternalGuid, nameof(FormatDataExportEntity.ExternalGuid)).Column("EXTERNAL_GUID");
            this.Property(x => x.EntityType, nameof(FormatDataExportEntity.EntityType)).Column("ENTITY_TYPE").NotNull();
            this.Property(x => x.ExportDate, nameof(FormatDataExportEntity.ExportDate)).Column("EXPORT_DATE");
            this.Property(x => x.ExportEntityState, nameof(FormatDataExportEntity.ExportEntityState)).Column("EXPORT_STATE").NotNull();
            this.Property(x => x.ErrorMessage, nameof(FormatDataExportEntity.ErrorMessage)).Column("ERROR_MESSAGE");
            this.Reference(x => x.FormatDataExportInfo, nameof(FormatDataExportEntity.FormatDataExportInfo)).Column("FORMAT_DATA_EXPORT_INFO_ID");
        }
    }
}
