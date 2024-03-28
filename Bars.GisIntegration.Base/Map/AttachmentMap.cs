namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Маппинг для "Bars.GisIntegration.Base.Entities.Attachment"
    /// </summary>
    public class AttachmentMap : BaseEntityMap<Attachment>
    {
        public AttachmentMap() :
            base("Bars.GisIntegration.Base.Entities.Attachment", "GI_ATTACHMENT")
        {
        }

        protected override void Map()
        {
            Property(x => x.SourceFileInfoId, "SourceFileInfoId").Column("SRC_FILE_INFO_ID");
            Reference(x => x.FileInfo, "FileInfo").Column("FILE_INFO_ID").NotNull().Fetch();
            Property(x => x.Guid, "Guid").Column("GUID").Length(50);
            Property(x => x.Hash, "Hash").Column("HASH").Length(200);
            Property(x => x.Name, "Name").Column("NAME").Length(500);
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            Property(x => x.FileStorageName, "FileStorageName").Column("FILE_STORAGE_NAME");
        }
    }
}