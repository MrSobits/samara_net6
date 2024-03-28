namespace Bars.GisIntegration.Smev.Map
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Smev.Entity;

    public class FileMetadataMap : BaseEntityMap<FileMetadata>
    {
        /// <inheritdoc />
        public FileMetadataMap() : base("FILE_METADATA")
        {
            this.Schema("SYSTEM");
            
            this.Map(x=> x.Name, "NAME");
            this.Map(x=> x.Extension, "EXTENSION");
            this.Map(x=> x.CachedName, "CACHED_NAME");
            this.Map(x=> x.LastAccess, "LAST_ACCESS");
            this.Map(x=> x.Checksum, "CHECKSUM");
            this.Map(x=> x.Size, "FILE_SIZE");
            this.Map(x=> x.ChecksumHashAlgorithm, "CHECKSUM_HASH_ALGORITHM");
            this.Map(x=> x.IsTemporary, "IS_TEMPORARY");
            this.Map(x=> x.Uid, "UID");
        }
    }
}